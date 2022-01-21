using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessLife_SO_Mobile.Models
{
    class Rutinas
    {
        [Key]
        public int IdRutina { get; set; }

        [Required]
        public string Descripcion { get; set; }

        
        public virtual ICollection<RutinaDetalles> RutinaDetalles { get; set; }
    }
}
