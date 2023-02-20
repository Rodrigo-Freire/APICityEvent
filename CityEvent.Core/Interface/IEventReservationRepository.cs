using CityEvent.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Core.Interface
{
    public interface IEventReservationRepository
    {
        public List<EventReservationEntity> BookingInquiry(string personName, string title);
        public bool InsertReservation(int idEvent, string personName, int quantity);
        public bool UpdateReserv(int idReservation, int quantity);
        public bool DeleteReservation(int idReservation);
    }
}