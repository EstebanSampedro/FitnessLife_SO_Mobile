using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessLife_SO_Mobile.Models
{
    class RutinaDetalles
    {
        [Key]
        public int IdRutinaDetalle { get; set; }

        [Required]
        public string DiaSemana { get; set; }

        [Required]
        public string Ejercicio { get; set; }

        [Required]
        public string Repeticiones { get; set; }

        public string ImagePath { get; set; }

        public int IdRutina { get; set; }

        public virtual Rutinas Rutinas { get; set; }
    }
}
