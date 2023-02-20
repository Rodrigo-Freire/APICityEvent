using CityEvent.Core.Entity;
using CityEvent.Core.Interface;
using CityEvent.Core.Service;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;

namespace APICityEvent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class EventReservationController : ControllerBase
    {
        private string _stringconnection { get; set; }
        public IEventReservationService _eventReservationService { get; set; }

        public EventReservationController(IEventReservationService eventReservationService)
        {
            _stringconnection = Environment.GetEnvironmentVariable("DATABASE_CONFIG");
            _eventReservationService = eventReservationService;
        }

        [HttpGet("GetReservantionNameTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<List<EventReservationEntity>> BookingInquiry(string personName, string title)
        {
            if (personName == null || title == null)
            {
                return BadRequest();
            }

            List<EventReservationEntity> entity = _eventReservationService.BookingInquiry(personName, title);
            if (entity.Count == 0)
            {
                return NotFound();
            }
            return Ok(entity);

        }

        [HttpPost("EventReservation/Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<EventReservationEntity> CreatReservation(int idEvent, string personName, int quantity)
        {
            EventReservationEntity entity = new();
            if (!_eventReservationService.InsertReservation(idEvent, personName, quantity))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(CreatReservation), entity);
        }

        [HttpPut("EventReservation/Put")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult UpdateReserv(int idReservation, int quantity)
        {
            if (!_eventReservationService.UpdateReserv(idReservation, quantity))
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult DeleteReservation(int idReservation)
        {
            if (idReservation == null)
            {
                return BadRequest();
            }

            if (!_eventReservationService.DeleteReservation(idReservation))
            {
                return NotFound();
            }
            return NoContent();
        }





    }
}