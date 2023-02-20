using CityEvent.Core.Entity;
using CityEvent.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Core.Service
{
    public class CityEventService : ICityEventService
    {
        private ICityEventRepository _cityEventRepository;

        public CityEventService(ICityEventRepository cityEventRepository)
        {
            _cityEventRepository = cityEventRepository;
        }

        public List<CityEventEntity> ShowAll()
        {
            return _cityEventRepository.ShowAll();
        }

        public List<CityEventEntity> GetEventTitle(string title)
        {
            return _cityEventRepository.GetEventTitle(title);
        }

        public List<CityEventEntity> FilterByDateOrLocal(string local, string dateHourEventString)
        {

            return _cityEventRepository.FilterByDateOrLocal(local, dateHourEventString);
        }

        public List<CityEventEntity> FilterByDateAndPrice(int lowPrice, int hightPrice, string dateHourEventString)
        {
            return _cityEventRepository.FilterByDateAndPrice(lowPrice, hightPrice, dateHourEventString);
        }

        public bool PostEvent(string title, string description, string dateHourEventString, string local, string address, decimal price, bool status)
        {
            return _cityEventRepository.PostEvent(title, description, dateHourEventString, local, address, price, status);
        }

        public bool ChageEvent(string idEventyString, string title, string description, string dateHourEventString, string local, string address, decimal price, bool status)
        {
            return _cityEventRepository.ChageEvent(idEventyString, title, description, dateHourEventString, local, address, price, status);
        }

        public bool DeleteEvent(int idEvent)
        {
            bool validate = _cityEventRepository.SearchReservation(idEvent);
            if (validate == false)
            {
                return _cityEventRepository.DeleteEvent(idEvent);
            }
            return _cityEventRepository.ChangeStatus(idEvent);
        }
    }
}