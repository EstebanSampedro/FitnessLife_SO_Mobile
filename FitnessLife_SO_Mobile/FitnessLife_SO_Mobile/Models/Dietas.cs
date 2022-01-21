using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessLife_SO_Mobile.Models
{
     class Dietas
    {
        [Key]
        public int IdDieta { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public virtual ICollection<DietaDetalles> DietaDetalles { get; set; }

    }
}
