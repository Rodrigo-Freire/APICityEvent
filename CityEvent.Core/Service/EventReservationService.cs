using CityEvent.Core.Entity;
using CityEvent.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Core.Service
{
    public class EventReservationService : IEventReservationService
    {
        private IEventReservationRepository _eventReservationRepository;

        public EventReservationService(IEventReservationRepository eventReservationRepository)
        {
            _eventReservationRepository = eventReservationRepository;
        }
        public List<EventReservationEntity> BookingInquiry(string personName, string title)
        {
            return _eventReservationRepository.BookingInquiry(personName, title);
        }

        public bool DeleteReservation(int idReservation)
        {
            return _eventReservationRepository.DeleteReservation(idReservation);
        }

        public bool InsertReservation(int idEvent, string personName, int quantity)
        {
            return _eventReservationRepository.InsertReservation(idEvent, personName, quantity);
        }

        public bool UpdateReserv(int idReservation, int quantity)
        {
            return _eventReservationRepository.UpdateReserv(idReservation, quantity);
        }
    }
}