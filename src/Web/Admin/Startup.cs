using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.Usuario;
using ApplicationCore.Configurations;
using ApplicationCore.DependencyInjection;
using ApplicationCore.Services;
using Framework.Configurations;
using Framework.Extenders;
using Framework.UI.Extenders;
using Framework.UI.MVC.MetadataProviders;
using Infrastructure.Configurations;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Admin
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup()
        {
            _configuration = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile(EnvironmentHelper.Desenvolvimento ? "appsettings.Development.json"
                  : EnvironmentHelper.Homologacao
                  ? "appsettings.Homologacao.json"
                  : "appsettings.Production.json", optional: true, reloadOnChange: true)
#if DEBUG
                          .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
#endif
                          .AddEnvironmentVariables()
              .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<InfrastructureConfig>(options => _configuration.GetSection(nameof(Infrastructure)).Bind(options))
                .Configure<ApplicationCoreConfig>(options => _configuration.GetSection("ApplicationCore").Bind(options))
                .Configure<AppConfig>(options => _configuration.GetSection(nameof(AppConfig)).Bind(options))
                .AddCore(sp => sp.GetService<IOptions<ApplicationCoreConfig>>().Value)
                .AddInfrastructure(sp => sp.GetService<IOptions<InfrastructureConfig>>().Value)
                .AddDbContext(_configuration.GetConnectionString("DefaultConnection"));

            AddServiceWeb(services);

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(serv =>
            {
                var httpContext = serv.GetService<IHttpContextAccessor>().HttpContext;
                var user = httpContext.GetAuthUsuario<UsuarioAuthViewModel>();
                if (!user.Autenticado)
                    return user;

                var permissoesAcoes = serv.GetService<GrupoUsuarioService>()
                    .ListarPermissaoAcaoPorUsuario(user.Identificao.Id, user.Identificao.IdGrupoUsuario);
                user.FillPermissoesAcoes(permissoesAcoes);
                return user;
            });

            ConfigureCookieSettings(services);

            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(new DisplayMetadataProvider(typeof(DisplayModelResource)));
                    options.AddBinders();
                })
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/Erro");
                app.UseStatusCodePagesWithRedirects("/Erro/StatusCode/{0}");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={(env.IsDevelopment() ? "60" : "360")}");
                }
            });

            app.UseAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("consultas", "{controller}/{numeroPagina}-{tamanhoPagina}", defaults: new { action = "Index" });
                endpoints.MapControllerRoute("default", "{controller=Login}/{action=Index}/{id?}");
            });
        }

        private static void AddServiceWeb(IServiceCollection services)
        {
            var servicesType = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(BaseServiceWeb).IsAssignableFrom(t) && t.BaseType != typeof(object));

            foreach (var type in servicesType)
                services.AddScoped(type);
        }

        private void ConfigureCookieSettings(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = $".AspNetCore.Admin.{_configuration["AppConfig:Admin:Cookie"]}.Cookies";
                    options.AccessDeniedPath = new PathString("/Login/Entrar");
                    options.LoginPath = new PathString("/Login/Entrar");
                    options.LogoutPath = new PathString("/Login/Sair");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
