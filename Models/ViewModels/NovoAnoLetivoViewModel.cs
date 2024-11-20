// ViewModels/NovoAnoLetivoViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace HorariosIPBejaMVC.ViewModels
{
    public class NovoAnoLetivoViewModel
    {
        [Required]
        [StringLength(50)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public DateOnly DataInicio { get; set; }

        [Required]
        public DateOnly DataFim { get; set; }
    }
}
