using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Linq;

namespace SW.Smarten
{

    class MartenLogger : IMartenLogger, IMartenSessionLogger
    {
        private readonly ILogger logger;

        public MartenLogger(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ConsoleMartenLogger>();
        }

        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            return this;
        }

        public void SchemaChange(string sql)
        {
            Console.WriteLine("Executing DDL change:");
            Console.WriteLine(sql);
            Console.WriteLine();
        }

        public void LogSuccess(NpgsqlCommand command)
        {

            logger.LogInformation(command.CommandText);
            //Console.WriteLine(command.CommandText);
            //foreach (var p in command.Parameters.OfType<NpgsqlParameter>())
            //{
            //    Console.WriteLine($"  {p.ParameterName}: {p.Value}");
            //}
        }

        public void LogFailure(NpgsqlCommand command, Exception ex)
        {
            Console.WriteLine("Postgresql command failed!");
            Console.WriteLine(command.CommandText);
            foreach (var p in command.Parameters.OfType<NpgsqlParameter>())
            {
                Console.WriteLine($"  {p.ParameterName}: {p.Value}");
            }
            Console.WriteLine(ex);
        }

        public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
        {
            var lastCommit = commit;
            Console.WriteLine(
                $"Persisted {lastCommit.Updated.Count()} updates, {lastCommit.Inserted.Count()} inserts, and {lastCommit.Deleted.Count()} deletions");
        }
    }
}