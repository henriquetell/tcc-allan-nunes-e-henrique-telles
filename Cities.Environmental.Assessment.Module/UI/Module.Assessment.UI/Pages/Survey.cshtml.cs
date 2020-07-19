using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Module.Assessment.UI.Pages
{
    public class SurveyModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public SurveyModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Obrigatório")]
        public string State { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Obrigatório")]
        public string City { get; set; }

   

        [BindProperty]
        public string FreeResponse { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Obrigatório")]
        public int Quality { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Obrigatório")]
        public int Political { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Obrigatório")]
        public int Crimes { get; set; }



        public void OnGet()
        {

        }
    }
}
