using Backend.Helpers;
using Backend.Repository.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public abstract class BaseRepository<T> where T: BaseRepository<T> // IAsyncDisposable
    {
        public BaseRepository(IOptions<RepoOptions> repoOptions, ILogger<T> logger)
        {
            Check.NotNull(repoOptions, nameof(repoOptions));
            Check.NotNull(logger, nameof(logger));
            RepoOptions = repoOptions.Value;
            Logger = logger;
        }

        public RepoOptions RepoOptions { get; private set; }
        public ILogger<T> Logger { get; private set; }


        protected NpgsqlCommand CreateStoreProceduceCmd(string storeProcedureName)
        {
            Check.NotNull(storeProcedureName, nameof(storeProcedureName));
            Check.CallerLog<T>(Logger, LoggerExecutionPositions.Entrance, $"storeProcedureName: {storeProcedureName} - ");

            var command = new NpgsqlCommand(storeProcedureName);

            Check.CallerLog<T>(Logger, LoggerExecutionPositions.Exit);
            return command;
        }

        //public abstract ValueTask DisposeAsync();
    }
}
