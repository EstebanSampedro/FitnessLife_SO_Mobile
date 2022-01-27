using FitnessLife_SO_Mobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessLife_SO_Mobile.ViewModels
{
    public class DietaDetallesDetailViewModel
    {
        [Key]
        public int IdDietaDetalle { get; set; }

        [Required]
        public string DiaSemana { get; set; }

        [Required]
        public string HoraComida { get; set; }

        [Required]
        public string Plato { get; set; }

        [Required]
        public string Porcion { get; set; }

        public string ImagePath { get; set; }

        public int IdDieta { get; set; }

        public virtual Dietas Dietas { get; set; }
    }
}
