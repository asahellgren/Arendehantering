using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace DAL.Repositories
{
    internal static class TransientDapperExtensions
    {
        private static readonly RetryManager SqlRetryManager = GetDefaultRetryManager();
        private static readonly RetryPolicy SqlCommandRetryPolicy = SqlRetryManager.GetDefaultSqlCommandRetryPolicy();
        private static readonly RetryPolicy SqlConnectionRetryPolicy =
            SqlRetryManager.GetDefaultSqlConnectionRetryPolicy();

        private static RetryManager GetDefaultRetryManager()
        {
            const int retryCount = 3;
            const int minBackoffDelayMilliseconds = 2000;
            const int maxBackoffDelayMilliseconds = 8000;
            const int deltaBackoffMilliseconds = 2000;

            var exponentialBackoffStrategy =
                new ExponentialBackoff(
                    "exponentialBackoffStrategy",
                    retryCount,
                    TimeSpan.FromMilliseconds(minBackoffDelayMilliseconds),
                    TimeSpan.FromMilliseconds(maxBackoffDelayMilliseconds),
                    TimeSpan.FromMilliseconds(deltaBackoffMilliseconds)
                    );

            var manager = new RetryManager(
                new List<RetryStrategy>
                {
                    exponentialBackoffStrategy
                },
                exponentialBackoffStrategy.Name
                );

            return manager;
        }

        public static void OpenWithRetry(this SqlConnection cnn)
        {
            cnn.OpenWithRetry(SqlConnectionRetryPolicy);
        }

        public static IEnumerable<T> QueryWithRetry<T>(
            this SqlConnection cnn, string sql, object param = null, IDbTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, CommandType? commandType = null
            )
        {
            return SqlCommandRetryPolicy.ExecuteAction(
                () => cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType)
                );
        }

        public static int ExecuteWithRetry(
         this SqlConnection cnn, string sql, object param = null, IDbTransaction transaction = null,
        int? commandTimeout = null, CommandType? commandType = null
         )
        {
            return SqlCommandRetryPolicy.ExecuteAction(
                () => cnn.Execute(sql, param, transaction, commandTimeout, commandType)
                );
        }


    }
}
