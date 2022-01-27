using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessLife_SO_Mobile.Models
{
     public class Dietas
    {
        
        public int IdDieta { get; set; }

        
        public string Descripcion { get; set; }

        public virtual ICollection<DietaDetalles> DietaDetalles { get; set; }

    }
}
