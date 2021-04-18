using ApplicationCore.DependencyInjection;
using ApplicationCore.Respositories.ReadOnly;
using Ardalis.ListStartupServices;
using Framework.Configurations;
using Framework.UI.Extenders;
using Framework.UI.MVC.MetadataProviders;
using Infrastructure.Configurations;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shop.HealthChecks;
using Shop.Models;
using Shop.Resources;
using Shop.ViewModels.Carrinho;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;

namespace Shop
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCore()
                .Configure<InfrastructureConfig>(options => _configuration.GetSection(nameof(Infrastructure)).Bind(options))
                .Configure<FrameworkConfig>(options => _configuration.GetSection(nameof(Framework)).Bind(options))
                .Configure<AppConfig>(options => _configuration.GetSection(nameof(AppConfig)).Bind(options))
                .AddInfrastructure(sp => sp.GetService<IOptions<InfrastructureConfig>>().Value);

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(serv =>
            {
                var httpContext = serv.GetService<IHttpContextAccessor>().HttpContext;
                var user = httpContext.GetAuthUsuario<UsuarioAuthModel>();
                return user;
            });

            services.AddScoped(serv =>
            {
                var httpContext = serv.GetService<IHttpContextAccessor>().HttpContext;
                var user = httpContext.GetAuthUsuario<UsuarioAuthModel>();
                var itens = serv.GetService<ICarrinhoItemReadOnlyRepository>()
                                .ListarItensEmAberto(user.Identificao.Id)
                                .Select(ci => new CarrinhoItemViewModel(ci)).ToList();

                return new CarrinhoViewModel();
            });

            ConfigureCookieSettings(services);

            services.AddRouting(options =>
            {
                // Replace the type and the name used to refer to it with your own
                // IOutboundParameterTransformer implementation
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            services.AddMvc(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                         new SlugifyParameterTransformer()));

            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
           .AddCheck<HomePageHealthCheck>("home_page_health_check");

            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(services);

                config.Path = "/allservices";
            });

            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(new DisplayMetadataProvider(typeof(DisplayModelResource)));
                    options.AddBinders();
                });
        }

        private void ConfigureCookieSettings(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = $".AspNetCore.Shop.{_configuration["Shop:Cookie"]}.Cookies";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Login/Entrar";
                options.LogoutPath = "/Login/Sair";
                options.AccessDeniedPath = "/Login/Entrar";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(next => context =>
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");

                return next?.Invoke(context);
            });

            var culturePtBr = new CultureInfo("pt-BR");

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culturePtBr),
                SupportedUICultures = new[] { culturePtBr },
                SupportedCultures = new[] { culturePtBr }
            });


            app.UseDeveloperExceptionPage();
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                status = report.Status.ToString(),
                                errors = report.Entries.Select(e => new
                                {
                                    key = e.Key,
                                    value = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseShowAllServicesMiddleware();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller:slugify=Home}/{action:slugify=Index}/{id?}");
            });
        }
    }
}
