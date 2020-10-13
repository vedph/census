using Census.Core;
using Census.MySql;
using Fusi.Tools.Data;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        public Act GetAct(int id)
        {
            using var db = GetContext();

            EfAct efAct = db.Acts
                .AsNoTracking()
                .Include(a => a.Book)
                .ThenInclude(b => b.Archive)
                .Include(a => a.Book)
                .ThenInclude(b => b.Type)
                .Include(a => a.Book)
                .ThenInclude(b => b.Subtype)
                .Include(a => a.Book)
                .ThenInclude(b => b.Writer)
                .Include(a => a.Book)
                .ThenInclude(b => b.WritePlace)
                .Include(a => a.Type)
                .Include(a => a.Subtype)
                .Include(a => a.Company)
                .Include(a => a.Family)
                .Include(a => a.Place)
                .FirstOrDefault(a => a.Id == id);
            if (efAct == null) return null;

            Act act = new Act
            {
                Id = efAct.Id,
                Label = efAct.Label,
                Note = efAct.Note,
                Type = new LookupItem(efAct.TypeId, efAct.Type.Name),
                Subtype = new LookupItem(efAct.SubtypeId, efAct.Subtype.Name),
                Company = efAct.CompanyId != null?
                    new LookupItem(efAct.CompanyId.Value, efAct.Company.Name) : null,
                Family = efAct.FamilyId != null ?
                    new LookupItem(efAct.FamilyId.Value, efAct.Family.Name) : null,
                Place = efAct.PlaceId != null ?
                    new LookupItem(efAct.PlaceId.Value, efAct.Place.Name) : null
            };

            // categories
            var categories = db.ActCategories
                .AsNoTracking()
                .Include(ac => ac.Category)
                .Where(ac => ac.ActId == act.Id)
                .OrderBy(ac => ac.Category.Name);
            foreach (EfActCategory ac in categories)
            {
                act.Categories.Add(
                    new LookupItem(ac.CategoryId, ac.Category.Name));
            }

            // professions
            var professions = db.ActProfessions
                .AsNoTracking()
                .Include(ap => ap.Profession)
                .Where(ap => ap.ActId == act.Id)
                .OrderBy(ap => ap.Profession.Name);
            foreach (EfActProfession ap in professions)
            {
                act.Professions.Add(
                    new LookupItem(ap.ProfessionId, ap.Profession.Name));
            }

            // partners
            var partners = db.ActPartners
                .AsNoTracking()
                .Include(ap => ap.Partner)
                .Where(ap => ap.ActId == act.Id)
                .OrderBy(ap => ap.Partner.Name);
            foreach (EfActPartner ap in partners)
            {
                act.Partners.Add(
                    new LookupItem(ap.PartnerId, ap.Partner.Name));
            }

            // book
            EfBook efBook = efAct.Book;
            act.Book = new Book
            {
                Archive = new LookupItem(efBook.ArchiveId, efBook.Archive.Name),
                Type = new LookupItem(efBook.TypeId, efBook.Type.Name),
                Subtype = new LookupItem(efBook.SubtypeId, efBook.Subtype.Name),
                WritePlace = efBook.WritePlaceId != null
                    ? new LookupItem(efBook.WritePlaceId.Value, efBook.WritePlace.Name)
                    : null,
                Writer = efBook.WriterId != null
                    ? new LookupItem(efBook.WriterId.Value, efBook.Writer.Name)
                    : null,
                Location = efBook.Location,
                Description = efBook.Description,
                Incipit = efBook.Incipit,
                StartYear = efBook.StartYear,
                EndYear = efBook.EndYear,
                Edition = efBook.Edition,
                Note = efBook.Note,
                File = efBook.File
            };

            return act;
        }
    }
}
