using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeTownNET8.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese un nombre para la categoría")]
        [RegularExpression(@"^[A-Z a-z ><\/]*$", ErrorMessage = "Solo se permiten letras!")]
        [Display(Name ="Nombre de Categoría")]
        public string Nombre { get; set; }
        [Display(Name = "Orden de visualización")]
        public int? Orden { get; set; }
    }
}
