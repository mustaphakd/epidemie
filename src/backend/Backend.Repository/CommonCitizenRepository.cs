using Backend.Core.Infrastructure;
using Backend.Helpers;
using Backend.Model;
using Backend.Repository.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Backend.Repository
{
    public class CommonCitizenRepository : BaseRepository<CommonCitizenRepository>
    {
        public CommonCitizenRepository(IOptions<RepoOptions> repoOptions, ILogger<CommonCitizenRepository> logger)
            : base(repoOptions, logger)
        {

        }

        //https://www.npgsql.org/doc/copy.html

        public void AddPhysicalContacts(IEnumerable<PhysicalContact> contacts)
        {
            Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Entrance, $"contacts: {contacts}");
            Check.NotNull(contacts, nameof(contacts));

            if (contacts.Count() < 1)
            {
                Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Exit, $"contacts does not have any item.");
                return;
            }

            using (var connection_ = new NpgsqlConnection(RepoOptions.PgsqlPassword))
            {
                try
                {
                    using (var writer = connection_.BeginBinaryImport("COPY physical_contacts (user_id, date, time, contact_type, coordinate) FROM STDIN (FORMAT BINARY)"))
                    {
                        foreach (var contact in contacts)
                        {
                            Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Body, $"Writting contact: {contact} to bulk copy");
                            var userId = Convert.ToInt32(contact.UserId);
                            var time = contact.DateTime.TimeOfDay;
                            var contactType = contact.Contacts.ToString();
                            var point = new NetTopologySuite.Geometries.Point(new NetTopologySuite.Geometries.Coordinate(contact.Latitude, contact.Longitude));
                            writer.StartRow();
                            writer.Write(userId, NpgsqlDbType.Integer);
                            writer.Write(contact.DateTime, NpgsqlDbType.Date);
                            writer.Write(time, NpgsqlDbType.Time);
                            writer.Write(contactType, NpgsqlDbType.Varchar);
                            writer.Write(point, NpgsqlDbType.Point);
                        }

                        writer.Complete();
                    }

                }
                catch (Exception exception)
                {
                    Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Body, $"Exception {exception}", LogLevel.Warning);

                    if (exception is BackendException)
                    {
                        throw exception;
                    }

                    throw new BackendException(Convert.ToInt32(HttpStatusCode.InternalServerError), exception.Message);
                }
            }

            Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Exit);
        }

        public Dictionary<DateTime, int> GetSynchronizedPhysicalContacts(int userId, IEnumerable<DateTime> dateTimes)
        {
            Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Entrance, $"userId: {userId}, dateTimes: {dateTimes}");
            Check.NotNull(userId, nameof(userId));
            Check.NotNull(dateTimes, nameof(dateTimes));

            var synchedDateTimes = new Dictionary<DateTime, int>();

            if (dateTimes.Count() < 1)
            {
                Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Exit, $"dateTimes does not have any item.");
                return synchedDateTimes;
            }

            using (var connection_ = new NpgsqlConnection(RepoOptions.PgsqlPassword))
            {
                try
                {
                    var query = $"SELECT id, date, time FROM (SELECT id, date, time FROM physical_contacts WHERE user_id = {userId}) WHERE ";

                    foreach (var dateTime in dateTimes)
                    {
                        var date = NpgsqlDate.ToNpgsqlDate(dateTime);
                        var time = NpgsqlTimeSpan.ToNpgsqlTimeSpan(dateTime.TimeOfDay);

                        query += $" (date = {date} AND time = {time}) OR";
                    }

                    query = query.TrimEnd('R').TrimEnd('O');

                    Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Body, $"constructed query: {query}");

                    var command = new NpgsqlCommand(query);
                    command.Connection = connection_;
                    command.CommandType = System.Data.CommandType.Text;
                    command.Prepare();

                    using (var reader = command.ExecuteReader())
                    {
                        Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Body, $"reading data from reader");

                        while(reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var date = reader.GetDate(1);
                            var time = reader.GetTimeSpan(2);

                            var dateTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
                            synchedDateTimes.Add(dateTime, id);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Body, $"Exception {exception}", LogLevel.Warning);

                    if (exception is BackendException)
                    {
                        throw exception;
                    }

                    throw new BackendException(Convert.ToInt32(HttpStatusCode.InternalServerError), exception.Message);
                }
            }

            Check.CallerLog<CommonCitizenRepository>(Logger, LoggerExecutionPositions.Exit, $"synchedDateTimes: {synchedDateTimes}");
            return synchedDateTimes;
        }

    }
}
