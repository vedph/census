using Census.Core;
using Census.MySql;
using Fusi.Tools.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Census.Ef
{
    public sealed class CensusRepository : ICensusRepository
    {
        private readonly string _connectionString;
        private readonly ITextFilter _filter;
        private readonly IDbConnection _connection;
        private readonly ISqlQueryBuilder _queryBuilder;

        public CensusRepository(string connectionString)
        {
            _connectionString = connectionString ??
                throw new ArgumentNullException(nameof(connectionString));
            _connection = new MySqlConnection(_connectionString);
            _filter = new StandardTextFilter();
            _queryBuilder = new MySqlQueryBuilder(_filter);
        }

        private CensusDbContext GetContext() =>
            new CensusDbContext(_connectionString, "mysql");

        private void EnsureConnected()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public DataPage<ActInfo> GetActs(ActFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            EnsureConnected();
            var t = _queryBuilder.BuildActSearch(filter);
            using (IDbCommand cmdPage = _connection.CreateCommand())
            using (IDbCommand cmdTot = _connection.CreateCommand())
            {
                List<ActInfo> acts = new List<ActInfo>();

                // total
                cmdTot.CommandText = t.Item2;
                object result = cmdTot.ExecuteScalar();
                int total = result != DBNull.Value && result != null ?
                    (int)result : 0;

                // page
                if (total > 0)
                {
                    cmdPage.CommandText = t.Item1;
                    using var reader = cmdPage.ExecuteReader();
                    while (reader.Read())
                    {
                        acts.Add(new ActInfo
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            // TODO...
                        });
                    }
                }

                _connection.Close();

                return new DataPage<ActInfo>(
                    filter.PageNumber,
                    filter.PageSize,
                    total,
                    acts);
            }
        }

        public IList<Tuple<int, string>> Lookup(DataEntityType type,
            string filter, int top)
        {
            throw new NotImplementedException();
        }
    }
}
