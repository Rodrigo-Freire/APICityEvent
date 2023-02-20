using CityEvent.Core.Entity;
using CityEvent.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;

namespace APICityEvent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CityEventController : ControllerBase
    {
        public ICityEventService _cityEventService { get; set; }
        private string _stringconnection { get; set; }

        public CityEventController(ICityEventService cityEventService)
        {
            _cityEventService = cityEventService;
            _stringconnection = Environment.GetEnvironmentVariable("DATABASE_CONFIG");
        }

        [HttpGet("ShowlAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<List<CityEventEntity>> GetAll()
        {
            List<CityEventEntity> allEvents = _cityEventService.ShowAll();
            if (allEvents == null)
            {
                return BadRequest();
            }

            return Ok(allEvents);

        }

        [HttpGet("FilterTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<List<CityEventEntity>> FilterTitle(string title)
        {
            if (title == null)
            {
                return BadRequest();
            }

            List<CityEventEntity> titleEvent = _cityEventService.GetEventTitle(title);
            if (titleEvent.Count == 0)
            {
                return NoContent();
            }
            return Ok(titleEvent);
        }

        [HttpGet("DateAndLocal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<List<CityEventEntity>> FilterByDateOrLocal(string local, string dateHourEventString)
        {
            if (local == null || dateHourEventString == null)
            {
                return BadRequest();
            }

            var cityEvent = _cityEventService.FilterByDateOrLocal(local, dateHourEventString);

            if (cityEvent.Count == 0)
            {
                return NotFound();
            }
            return Ok(cityEvent);

        }

        [HttpGet("DateAndPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<List<CityEventEntity>> FilterByDateAndPrice(int lowPrice, int hightPrice, string dateHourEventString)
        {
            if (lowPrice == null || hightPrice == null || dateHourEventString == null)
            {
                return BadRequest();
            }

            var citiEvent = _cityEventService.FilterByDateAndPrice(lowPrice, hightPrice, dateHourEventString);

            if (citiEvent.Count == 0)
            {
                return NotFound();
            }
            return Ok(citiEvent);
        }

        [HttpPost("CityEvent/PostEvents")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<CityEventEntity> PostEvent(string title, string description, string dateHourEventString, string local, string address, decimal price, bool status)
        {
            CityEventEntity entity = new();
            if (!_cityEventService.PostEvent(title, description, dateHourEventString, local, address, price, status))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(PostEvent), entity);
        }

        [HttpPut("CityEvent/Update")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult ChangeEvent(string idEventyString, string title, string description, string dateHourEventString, string local, string address, decimal price, bool status)
        {
            if (!_cityEventService.ChageEvent(idEventyString, title, description, dateHourEventString, local, address, price, status))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("RemoveEvent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteEvent(string idEventyString)
        {
            if (string.IsNullOrEmpty(idEventyString))
            {
                return BadRequest();
            }

            int.TryParse(idEventyString, out int idEvent);

            if (!_cityEventService.DeleteEvent(idEvent))
            {
                return BadRequest();
            }
            return NoContent();

        }



    }



}