using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Core.Entity
{
    public class EventReservationEntity
    {
        public long idReservation { get; set; }
        public long idEvent { get; set; }
        public string personName { get; set; }
        public long quantity { get; set; }
    }
}