﻿using Census.Ef;
using CsvHelper;
using CsvHelper.Configuration;
using Fusi.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Census.Import
{
    public sealed class CsvImporter
    {
        private readonly string _connectionString;
        private readonly ITextFilter _filter;
        private readonly Regex _yearRegex;
        private readonly char[] _otherSeps;

        /// <summary>
        /// Gets or sets a value indicating whether dry run is enabled.
        /// </summary>
        public bool IsDryRunEnabled { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvImporter"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        public CsvImporter(string connectionString)
        {
            _connectionString = connectionString ??
                throw new ArgumentNullException(nameof(connectionString));
            _filter = new WhitespaceTextFilter();

            _yearRegex = new Regex(@"^\s*(\d+)\s*$");

            // separators for altri soci (inconsistent)
            _otherSeps = new[] { ',', '/' };
        }

        /// <summary>
        /// Gets the target database SQL schema.
        /// </summary>
        /// <returns>SQL code representing the schema definition</returns>
        public static string GetTargetSchema()
        {
            using (StreamReader reader = new StreamReader(
                Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Census.Import.Assets.Schema.sql"),
                Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private void LogMaxLengthErrors(object entity)
        {
            var errors = MaxLengthValidator.GetErrors(entity);
            if (errors?.Count > 0)
            {
                foreach (var p in errors)
                {
                    Logger?.LogError($"Text too long in {p.Key}: " +
                        $"{p.Value.Item2.Length} > {p.Value.Item1} max " +
                        $"for \"{p.Value.Item2}\"");
                }
            }
        }

        private EfArchive GetArchive(string name, CensusDbContext db)
        {
            if (name == null) return null;

            EfArchive archive = db.Archives.FirstOrDefault(a => a.Name == name);
            if (archive == null)
            {
                archive = new EfArchive
                {
                    Name = name
                };
                db.Archives.Add(archive);
            }

            LogMaxLengthErrors(archive);
            return archive;
        }

        private EfBook GetBook(int archiveId, string location, CensusDbContext db)
        {
            if (location == null) return null;

            EfBook book = db.Books.FirstOrDefault(b => b.ArchiveId == archiveId
                && b.Location == location);
            if (book == null)
            {
                book = new EfBook
                {
                    ArchiveId = archiveId,
                    Location = location
                };
                db.Books.Add(book);
            }

            return book;
        }

        private EfFamily GetFamily(string name, CensusDbContext db)
        {
            if (name == null) return null;

            EfFamily family = db.Families.FirstOrDefault(f => f.Name == name);
            if (family == null)
            {
                family = new EfFamily
                {
                    Name = name
                };
                db.Families.Add(family);
            }

            LogMaxLengthErrors(family);
            return family;
        }

        private EfActType GetActType(string name, CensusDbContext db)
        {
            if (name == null) return null;

            EfActType type = db.ActTypes.FirstOrDefault(t => t.Name == name);
            if (type == null)
            {
                type = new EfActType
                {
                    Name = name
                };
                db.ActTypes.Add(type);
            }

            LogMaxLengthErrors(type);
            return type;
        }

        private EfActSubtype GetActSubtype(string name, int actTypeId,
            CensusDbContext db)
        {
            if (name == null) return null;

            EfActSubtype sub = db.ActSubtypes.FirstOrDefault(
                s => s.ActTypeId == actTypeId && s.Name == name);
            if (sub == null)
            {
                sub = new EfActSubtype
                {
                    Name = name,
                    ActTypeId = actTypeId
                };
                db.ActSubtypes.Add(sub);
            }

            LogMaxLengthErrors(sub);
            return sub;
        }

        private EfBookType GetBookType(string name, CensusDbContext db)
        {
            if (name == null) return null;

            EfBookType type = db.BookTypes.FirstOrDefault(t => t.Name == name);
            if (type == null)
            {
                type = new EfBookType
                {
                    Name = name
                };
                db.BookTypes.Add(type);
            }

            LogMaxLengthErrors(type);
            return type;
        }

        private EfBookSubtype GetBookSubtype(string name, int bookTypeId,
            CensusDbContext db)
        {
            if (name == null) return null;

            EfBookSubtype sub = db.BookSubtypes.FirstOrDefault(
                s => s.BookTypeId == bookTypeId && s.Name == name);
            if (sub == null)
            {
                sub = new EfBookSubtype
                {
                    Name = name,
                    BookTypeId = bookTypeId
                };
                db.BookSubtypes.Add(sub);
            }

            LogMaxLengthErrors(sub);
            return sub;
        }

        private EfCompany GetCompany(string name, string prevName,
            CensusDbContext db)
        {
            if (name == null) return null;

            EfCompany company = db.Companies.FirstOrDefault(c => c.Name == name);
            if (company == null)
            {
                company = new EfCompany
                {
                    Name = name
                };
                db.Companies.Add(company);
            }

            if (!string.IsNullOrEmpty(prevName))
            {
                EfCompany prevCompany =
                    db.Companies.FirstOrDefault(c => c.Name == prevName);
                if (prevCompany == null)
                {
                    prevCompany = new EfCompany
                    {
                        Name = name
                    };
                    db.Companies.Add(prevCompany);
                }
                company.Previous = prevCompany;
            }

            LogMaxLengthErrors(company);
            return company;
        }

        private EfPlace GetPlace(string name, CensusDbContext db)
        {
            if (name == null) return null;

            EfPlace place = db.Places.FirstOrDefault(c => c.Name == name);
            if (place == null)
            {
                place = new EfPlace
                {
                    Name = name
                };
                db.Places.Add(place);
            }

            LogMaxLengthErrors(place);
            return place;
        }

        private EfPerson GetPerson(string name, CensusDbContext db)
        {
            if (name == null) return null;

            EfPerson person = db.Persons.FirstOrDefault(p => p.Name == name);
            if (person == null)
            {
                person = new EfPerson
                {
                    Name = name
                };
                db.Persons.Add(person);
            }

            LogMaxLengthErrors(person);
            return person;
        }

        private Tuple<EfCategory, bool> GetCategory(string name, CensusDbContext db)
        {
            if (name == null) return null;

            bool unsure = false;
            if (name.EndsWith("?"))
            {
                unsure = true;
                name = name[0..^1];
            }

            EfCategory category = db.Categories.FirstOrDefault(c => c.Name == name);
            if (category == null)
            {
                category = new EfCategory
                {
                    Name = name
                };
            }

            LogMaxLengthErrors(category);
            return Tuple.Create(category, unsure);
        }

        private Tuple<EfProfession, bool> GetProfession(string name,
            CensusDbContext db)
        {
            if (name == null) return null;

            bool unsure = false;
            if (name.EndsWith("?"))
            {
                unsure = true;
                name = name[0..^1];
            }

            EfProfession profession =
                db.Professions.FirstOrDefault(c => c.Name == name);
            if (profession == null)
            {
                profession = new EfProfession
                {
                    Name = name
                };
            }

            LogMaxLengthErrors(profession);
            return Tuple.Create(profession, unsure);
        }

        private short ParseYear(string text)
        {
            Match m = _yearRegex.Match(text);
            return m.Success &&
                short.TryParse(m.Groups[1].Value, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out short n)
                ? n
                : (short)0;
        }

        private void AddPartner(string name, EfAct act, CensusDbContext db)
        {
            name = Filter(name, null);
            if (string.IsNullOrEmpty(name)) return;

            EfPerson person = GetPerson(name, db);
            if (person != null)
            {
                db.ActPartners.Add(new EfActPartner
                {
                    Act = act,
                    Partner = person
                });
            }
        }

        private string Filter(string name, string defValue)
        {
            name = _filter.Apply(name);
            if (string.IsNullOrEmpty(name) || name == "?")
                return defValue;
            return name;
        }

        /// <summary>
        /// Imports data from the specified files.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="cancel">The cancellation token.</param>
        /// <param name="progress">The optional progress reporter.</param>
        /// <exception cref="ArgumentNullException">filePath</exception>
        public void Import(string filePath, CancellationToken cancel,
            IProgress<ProgressReport> progress = null)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            ProgressReport report = progress != null ? new ProgressReport() : null;

            using CensusDbContext db = new CensusDbContext(
                _connectionString, "mysql");
            using CsvReader reader = new CsvReader(
                new StreamReader(filePath, Encoding.UTF8),
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    IgnoreBlankLines = true,
                    Delimiter = "\t",
                    TrimOptions = TrimOptions.Trim,
                    HasHeaderRecord = true
                });
            foreach (CsvRecord record in reader.GetRecords<CsvRecord>())
            {
                // archive
                EfArchive archive = GetArchive(
                    Filter(record.Archive, "-"), db);
                // act type
                EfActType actType = GetActType(
                    Filter(record.ActType, "-"), db);
                // book type
                EfBookType bookType = GetBookType(
                    Filter(record.BookType, "-"), db);

                // we must now immediately save, as we depend on these
                // for book's and subtypes identification
                if (!IsDryRunEnabled) db.SaveChanges();

                // book
                EfBook book = GetBook(archive.Id,
                    Filter(record.Location, ""), db);
                book.Type = bookType;
                book.Subtype = GetBookSubtype(
                    Filter(record.BookSubtype, "-"), bookType.Id, db);
                book.WritePlace =
                    GetPlace(Filter(record.BookPlace, null), db);
                book.Writer =
                    GetPerson(Filter(record.BookWriter, null), db);
                book.Description = Filter(record.BookDescription, null);
                book.Incipit = Filter(record.BookIncipit, null);
                book.StartYear = ParseYear(record.BookStartYear);
                book.EndYear = ParseYear(record.BookEndYear);
                book.Edition = Filter(record.BookEdition, null);
                book.Note = Filter(record.BookNote, null);
                book.File = Filter(record.BookFile, null);
                LogMaxLengthErrors(book);

                // act
                EfAct act = new EfAct
                {
                    Label = _filter.Apply(Filter(record.Label, null)),
                    Book = book,
                    Type = actType,
                    Subtype = GetActSubtype(
                        Filter(record.ActSubtype, "-"), actType.Id, db),
                    Family = GetFamily(Filter(record.Family, null), db),
                    Company = GetCompany(
                        Filter(record.Company, null),
                        Filter(record.PrevCompany, null), db),
                    Place = GetPlace(Filter(record.Place, null), db)
                };
                db.Acts.Add(act);

                // act's categories
                if (!string.IsNullOrEmpty(record.Category))
                {
                    foreach (string rc in record.Category.Split(" - ",
                            StringSplitOptions.RemoveEmptyEntries))
                    {
                        Tuple<EfCategory, bool> cu =
                            GetCategory(Filter(rc, null), db);
                        if (cu == null) continue;

                        db.ActCategories.Add(new EfActCategory
                        {
                            Category = cu.Item1,
                            Act = act,
                            Unsure = cu.Item2
                        });
                    }
                }

                // act's professions
                if (!string.IsNullOrEmpty(record.Profession))
                {
                    foreach (string rp in record.Profession.Split(" - ",
                            StringSplitOptions.RemoveEmptyEntries))
                    {
                        Tuple<EfProfession, bool> pu =
                            GetProfession(Filter(rp, null), db);
                        if (pu == null) continue;

                        db.ActProfessions.Add(new EfActProfession
                        {
                            Profession = pu.Item1,
                            Act = act,
                            Unsure = pu.Item2
                        });
                    }
                }

                // act's partners
                if (!string.IsNullOrEmpty(record.Partner1))
                    AddPartner(record.Partner1, act, db);

                if (!string.IsNullOrEmpty(record.Partner2))
                    AddPartner(record.Partner2, act, db);

                if (!string.IsNullOrEmpty(record.PartnerEtc))
                {
                    foreach (string partner in record.PartnerEtc.Split(
                        _otherSeps, StringSplitOptions.RemoveEmptyEntries))
                    {
                        AddPartner(partner, act, db);
                    }
                }

                LogMaxLengthErrors(act);

                if (!IsDryRunEnabled) db.SaveChanges();

                if (cancel.IsCancellationRequested) return;
                if (progress != null && ++report.Count % 10 == 0)
                {
                    report.Message = report.Count.ToString(
                        CultureInfo.InvariantCulture);
                    progress.Report(report);
                }
            }
        }
    }
}