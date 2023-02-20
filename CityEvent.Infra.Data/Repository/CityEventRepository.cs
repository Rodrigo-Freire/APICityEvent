using CityEvent.Core.Entity;
using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CityEvent.Core.Interface;
using System.Data;
using System.Diagnostics.SymbolStore;

namespace CityEvent.Infra.Data.Repository
{
    public class CityEventRepository : ICityEventRepository
    {
        private string _stringconnection { get; set; }

        public CityEventRepository()
        {
            _stringconnection = Environment.GetEnvironmentVariable("DATABASE_CONFIG");
        }

        public List<CityEventEntity> ShowAll()
        {
            string query = "SELECT * FROM CityEvent";

            using MySqlConnection conn = new(_stringconnection);


            try
            {
                return (conn.Query<CityEventEntity>(query).ToList());
            }
            catch
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<CityEventEntity> GetEventTitle(string title)
        {
            string query = "SELECT * FROM CityEvent WHERE title LIKE @title";

            DynamicParameters param = new();
            param.Add("title", "%" + title + "%");

            using MySqlConnection conn = new(_stringconnection);

            try
            {
                List<CityEventEntity> entity = conn.Query<CityEventEntity>(query, param).ToList();
                return entity;
            }
            catch
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<CityEventEntity> FilterByDateOrLocal(string local, string dateHourEventString)
        {
            DateTime dateHourEvent;

            if (!DateTime.TryParse(dateHourEventString, out dateHourEvent))
            {
                Console.WriteLine("A data e hora fornecidas são inválidas. O formato esperado é: 'dd/mm/yyyy'");
            }
            string query = "SELECT * FROM CityEvent WHERE CAST(dateHourEvent AS DATE) = CAST(@dateHourEvent AS DATE) AND local LIKE @local";

            DynamicParameters param = new();
            param.Add("dateHourEvent", dateHourEvent);
            param.Add("local", "%" + local + "%");

            using MySqlConnection conn = new(_stringconnection);

            try
            {
                List<CityEventEntity> entity = conn.Query<CityEventEntity>(query, param).ToList();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<CityEventEntity> FilterByDateAndPrice(int lowPrice, int hightPrice, string dateHourEventString)
        {
            DateTime dateHourEvent;

            if (!DateTime.TryParse(dateHourEventString, out dateHourEvent))
            {
                Console.WriteLine("A data e hora fornecidas são inválidas. O formato esperado é: 'dd/mm/yyyy");
            }

            string query = "SELECT * FROM CityEvent WHERE DATE(dateHourEvent) = @dateHourEvent AND price BETWEEN @lowPrice AND @hightPrice";
            DynamicParameters param = new();
            param.Add("@dateHourEvent", dateHourEvent);
            param.Add("@lowPrice", lowPrice);
            param.Add("@hightPrice", hightPrice);

            using MySqlConnection conn = new(_stringconnection);

            try
            {
                List<CityEventEntity> entity = conn.Query<CityEventEntity>(query, param).ToList();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public bool PostEvent(string title, string description, string dateHourEventString, string local, string address, decimal price, bool status)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(dateHourEventString) || string.IsNullOrEmpty(local))
            {
                throw new ArgumentException("Parameters title, dateHourEventString and local are required.");
            }
            DateTime dateHourEvent;

            if (!DateTime.TryParse(dateHourEventString, out dateHourEvent))
            {
                Console.WriteLine("The date provided is invalid. The expected format is: 'dd/mm/yyyy");
            }
            string query = "INSERT INTO CityEvent (title, description, dateHourEvent, local, address, price, status) " +
                "VALUES(@title, @description, @dateHourEvent, @local, @address, @price, @status)";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@title", title);
                param.Add("@description", description);
                param.Add("@dateHourEvent", dateHourEvent);
                param.Add("@local", local);
                param.Add("@address", address);
                param.Add("@price", price);
                param.Add("@status", status);

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

        public bool ChageEvent(string idEventyString, string title, string description, string dateHourEventString, string local, string address, decimal price, bool status)
        {
            if (string.IsNullOrEmpty(idEventyString) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(dateHourEventString) || string.IsNullOrEmpty(local))
            {
                throw new ArgumentException("Parameters title, dateHourEventString and local are required.");
            }

            if (!long.TryParse(idEventyString, out long idEvent))
            {
                throw new ArgumentException("Please set a idEvent valid.");
            }
            DateTime dateHourEvent;

            if (!DateTime.TryParse(dateHourEventString, out dateHourEvent))
            {
                throw new ArgumentException("The date provided is invalid. The expected format is: 'dd/mm/yyyy");
            }


            string query = "UPDATE CityEvent SET title = @title, description = @description, dateHourEvent = @dateHourEvent, local = @local, address = @address, price = @price, status = @status WHERE idEvent = @idEvent";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using var transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@title", title);
                param.Add("@description", description);
                param.Add("@dateHourEvent", dateHourEvent);
                param.Add("@local", local);
                param.Add("@address", address);
                param.Add("@price", price);
                param.Add("@status", status);
                param.Add("@idEvent", idEvent);

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

        public bool DeleteEvent(int idEvent)
        {
            string query = "DELETE FROM CityEvent WHERE idEvent = @idEvent";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using var transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@idEvent", idEvent);

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

        public bool ChangeStatus(int idEvent)
        {
            string query = "UPDATE CityEvent SET status = false WHERE idEvent = @idEvent";

            using MySqlConnection conn = new(_stringconnection);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            using var transaction = conn.BeginTransaction();

            try
            {
                DynamicParameters param = new();
                param.Add("@idEvent", idEvent);

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

        public bool SearchReservation(int idEvent)
        {
            string query = "SELECT * FROM EventReservation WHERE idEvent = @idEvent";
            DynamicParameters param = new();
            param.Add("@idEvent", idEvent);

            using MySqlConnection conn = new(_stringconnection);

            return conn.QueryFirstOrDefault(query, param) != null;
        }




    }
}