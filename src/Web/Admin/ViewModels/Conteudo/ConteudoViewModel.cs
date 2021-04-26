using Admin.Resources;
using ApplicationCore.Enuns;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels.Conteudo
{
    public class ConteudoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Titulo { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public string Assunto { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public int? IdConteudoChave { get; set; }

        public EConteudoChave ConteudoChave { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = nameof(ModelStateResource.Obrigatorio))]
        public EStatus Status { get; set; }

        public List<ConteudoAnexoViewModel> Anexos { get; set; } = new List<ConteudoAnexoViewModel>();
    }
}
