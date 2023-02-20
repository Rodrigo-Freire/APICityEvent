using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Core.Entity
{
    public class CityEventEntity
    {
        public long idEvent { get; set; }
        [Required(ErrorMessage = "This field cannot be null")]
        public string title { get; set; }
        public string description { get; set; }
        [Required(ErrorMessage = "This field cannot be null")]
        public DateTime? dateHourEvent { get; set; }
        [Required(ErrorMessage = "This field cannot be null")]
        public string local { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        [Required(ErrorMessage = "This field cannot be null")]
        public bool status { get; set; }

    }
}