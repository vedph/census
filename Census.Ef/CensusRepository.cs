using Census.Core;
using Census.MySql;
using Fusi.Tools.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

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

        private static int GetReaderInt(IDataReader reader, string name)
        {
            int i = reader.GetOrdinal(name);
            if (reader.IsDBNull(i)) return 0;
            return reader.GetInt32(i);
        }

        private static short GetReaderShort(IDataReader reader, string name)
        {
            int i = reader.GetOrdinal(name);
            if (reader.IsDBNull(i)) return 0;
            return reader.GetInt16(i);
        }

        private static string GetReaderString(IDataReader reader, string name)
        {
            int i = reader.GetOrdinal(name);
            if (reader.IsDBNull(i)) return null;
            return reader.GetString(i);
        }

        public DataPage<ActInfo> GetActs(ActFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            EnsureConnected();
            var t = _queryBuilder.BuildActSearch(filter);

            List<ActInfo> acts = new List<ActInfo>();
            int total;

            using (IDbCommand cmdPage = _connection.CreateCommand())
            using (IDbCommand cmdTot = _connection.CreateCommand())
            {
                // total
                cmdTot.CommandText = t.Item2;
                object result = cmdTot.ExecuteScalar();
                total = result != DBNull.Value && result != null ?
                    Convert.ToInt32(result) : 0;

                // page
                if (total > 0)
                {
                    cmdPage.CommandText = t.Item1;
                    using var reader = cmdPage.ExecuteReader();
                    while (reader.Read())
                    {
                        acts.Add(new ActInfo
                        {
                            Id = GetReaderInt(reader, "id"),
                            TypeId = GetReaderInt(reader, "typeId"),
                            TypeName = GetReaderString(reader, "typeName"),
                            SubtypeId = GetReaderInt(reader, "subtypeId"),
                            SubtypeName = GetReaderString(reader, "subtypeName"),
                            FamilyId = GetReaderInt(reader, "familyId"),
                            FamilyName = GetReaderString(reader, "familyName"),
                            CompanyId = GetReaderInt(reader, "companyId"),
                            CompanyName = GetReaderString(reader, "companyName"),
                            PlaceId = GetReaderInt(reader, "placeId"),
                            PlaceName = GetReaderString(reader, "placeName"),
                            Label = GetReaderString(reader, "label"),
                            ArchiveId = GetReaderInt(reader, "archiveId"),
                            ArchiveName = GetReaderString(reader, "archiveName"),
                            BookId = GetReaderInt(reader, "bookId"),
                            BookLocation = GetReaderString(reader, "location"),
                            BookDescription = GetReaderString(reader, "description"),
                            BookStartYear = GetReaderShort(reader, "startYear"),
                            BookEndYear = GetReaderShort(reader, "endYear"),
                            BookFile = GetReaderString(reader, "file")
                        });
                    }
                }
            }

            _connection.Close();

            return new DataPage<ActInfo>(
                filter.PageNumber,
                filter.PageSize,
                total,
                acts);
        }

        public IList<LookupItem> Lookup(DataEntityType type,
            string filter, int top)
        {
            EnsureConnected();
            string sql = _queryBuilder.BuildLookup(type, filter, top);
            List<LookupItem> items = new List<LookupItem>();

            using (IDbCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(new LookupItem
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }

            _connection.Close();
            return items;
        }
    }
}
