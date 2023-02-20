using CityEvent.Core.Entity;
using CityEvent.Core.Interface;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CityEvent.Infra.Data.Repository
{
    public class EventReservationRepository : IEventReservationRepository
    {
        private string _stringconnection { get; set; }

        public EventReservationRepository()
        {
            _stringconnection = Environment.GetEnvironmentVariable("DATABASE_CONFIG");
        }

        public List<EventReservationEntity> BookingInquiry(string personName, string title)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(personName))
            {
                throw new ArgumentException("Parameters personName and title are required.");
            }

            string query = "SELECT * FROM EventReservation INNER JOIN CityEvent ON CityEvent.IdEvent = EventReservation.IdEvent " +
                "WHERE PersonName = @personName AND Title LIKE @title;";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using var transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@personName", personName);
                param.Add("@title", "%" + title + "%");

                List<EventReservationEntity> entity = conn.Query<EventReservationEntity>(query, param, transaction).ToList();

                transaction.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"An error occurred while inserting the event: {ex.Message}");
                throw;
            }

        }

        public bool InsertReservation(int idEvent, string personName, int quantity)
        {
            if (idEvent == null || string.IsNullOrEmpty(personName) || quantity == null)
            {
                throw new ArgumentException("All parameters are required");
            }
            string query = "INSERT INTO EventReservation (idEvent, personName, quantity) VALUES (@idEvent, @personName, @quantity)";
            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@idEvent", idEvent);
                param.Add("@personName", personName);
                param.Add("@quantity", quantity);

                int affectedLines = conn.Execute(query, param, transaction);

                transaction.Commit();

                return (affectedLines > 0);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"An error occurred while inserting the event: {ex.Message}");
                throw;
            }
            finally
            {
                conn.Close();
            }

        }

        public bool UpdateReserv(int idReservation, int quantity)
        {
            if (idReservation == null || quantity == null)
            {
                throw new ArgumentException("All parameters are required");
            }

            string query = "UPDATE EventReservation SET quantity = @quantity WHERE idReservation = @idReservation";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@idReservation", idReservation);
                param.Add("@quantity", quantity);

                int affectedLines = conn.Execute(query, param, transaction);

                transaction.Commit();

                return (affectedLines > 0);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"An error occurred while inserting the event: {ex.Message}");
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeleteReservation(int idReservation)
        {
            if (idReservation == null)
            {
                throw new ArgumentException("Parameter idReservation is required");
            }

            string query = "DELETE FROM EventReservation WHERE idReservation = @idReservation";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@idReservation", idReservation);

                int affectedLines = conn.Execute(query, param, transaction);

                transaction.Commit();

                return (affectedLines > 0);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"An error occurred while inserting the event: {ex.Message}");
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}