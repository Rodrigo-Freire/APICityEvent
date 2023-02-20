using CityEvent.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Core.Interface
{
    public interface ICityEventService
    {
        public List<CityEventEntity> ShowAll();
        public List<CityEventEntity> GetEventTitle(string title);
        public List<CityEventEntity> FilterByDateOrLocal(string local, string dateHourEventString);
        public List<CityEventEntity> FilterByDateAndPrice(int lowPrice, int hightPrice, string dateHourEventString);
        public bool PostEvent(string title, string description, string dateHourEventString, string local, string address, decimal price, bool status);
        public bool ChageEvent(string idEventyString, string title, string description, string dateHourEventString, string local, string address, decimal price, bool status);
        public bool DeleteEvent(int idEvent);
    }
}