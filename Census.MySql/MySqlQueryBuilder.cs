using Fusi.Tools.Data;
using System;
using System.Linq;
using System.Text;
using System.Globalization;
using Census.Core;

namespace Census.MySql
{
    public sealed class MySqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly ISqlTokenHelper _tokenHelper;
        private readonly ITextFilter _filter;

        public MySqlQueryBuilder(ITextFilter filter)
        {
            _tokenHelper = new MySqlTokenHelper();
            _filter = filter;
        }

        /// <summary>
        /// Wraps the specified non-keyword token according to the syntax
        /// of the SQL dialect being handled. For instance, in MySql this
        /// wraps a token into backticks, or in SQL Server into square brackets.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The wrapped token.</returns>
        private string ET(string token) => _tokenHelper.ET(token);

        /// <summary>
        /// Wraps the specified non-keyword tokens according to the syntax
        /// of the SQL dialect being handled. For instance, in MySql this
        /// wraps a token into backticks, or in SQL Server into square brackets.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns>The wrapped tokens separated by comma.</returns>
        private string ETS(params string[] tokens) =>
            string.Join(",", from k in tokens select ET(k));

        /// <summary>
        /// Wraps the specified non-keyword token and its prefix according to
        /// the syntax of the SQL dialect being handled. For instance, in MySql
        /// this wraps a token into backticks, or in SQL Server into square
        /// brackets.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="token">The token.</param>
        /// <param name="suffix">An optional suffix to be appended at the end
        /// of the result.</param>
        /// <returns>The wrapped token.</returns>
        private string ETP(string prefix, string token,
            string suffix = null) => _tokenHelper.ETP(prefix, token, suffix);

        /// <summary>
        /// Wraps the specified non-keyword tokens and their prefix according to
        /// the syntax of the SQL dialect being handled. For instance, in MySql
        /// this wraps a token into backticks, or in SQL Server into square
        /// brackets.
        /// </summary>
        /// <param name="prefix">The prefix common to all the tokens.</param>
        /// <param name="tokens">The tokens.</param>
        /// <returns>The wrapped tokens.</returns>
        private string ETPS(string prefix, params string[] tokens) =>
            string.Join(",", from k in tokens select ETP(prefix, k));

        /// <summary>
        /// SQL-encode the specified text, according to the SQL dialect.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="hasWildcards">if set to <c>true</c> [has wildcards].
        /// </param>
        /// <param name="wrapInQuotes">if set to <c>true</c> [wrap in quotes].
        /// </param>
        /// <param name="unicode">if set to <c>true</c> [unicode].</param>
        /// <returns>Encoded text.</returns>
        private string SQE(string text, bool hasWildcards = false,
            bool wrapInQuotes = false, bool unicode = true) =>
            _tokenHelper.SQE(text, hasWildcards, wrapInQuotes, unicode);

        private void AppendJoins(StringBuilder sb)
        {
            sb.AppendLine("INNER JOIN book ON act.bookId=book.id")
              .AppendLine("INNER JOIN archive ON book.archiveId=archive.id")
              .AppendLine("INNER JOIN actType ON act.typeId=actType.id")
              .AppendLine("INNER JOIN actSubtype ON act.subtypeId=actSubtype.id")
              .AppendLine("LEFT JOIN family ON act.familyId=family.id")
              .AppendLine("LEFT JOIN company ON act.companyId=company.id")
              .AppendLine("LEFT JOIN place ON act.placeId=place.id");
        }

        private void AppendJoins(ActFilter filter, StringBuilder sb)
        {
            if (filter.ArchiveId != 0)
            {
                sb.AppendLine("INNER JOIN book ON act.bookId=book.id")
                  .Append("INNER JOIN archive ON book.archiveId=archive.id");
            }
            else if (filter.BookYearMin != 0 || filter.BookYearMax != 0)
            {
                sb.Append("INNER JOIN book ON act.bookId=book.id");
            }
        }

        private string BuildWhereSql(ActFilter filter)
        {
            StringBuilder sb = new StringBuilder();

            // archive
            if (filter.ArchiveId != 0)
            {
                sb.Append(ETP("archive", "id")).Append('=').Append(filter.ArchiveId);
            }

            // book
            if (filter.BookId != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "bookId")).Append('=').Append(filter.BookId);
            }
            if (filter.BookYearMin != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("book", "startYear")).Append(">=").Append(filter.BookYearMin);
            }
            if (filter.BookYearMax != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("book", "endYear")).Append("<=").Append(filter.BookYearMax);
            }

            // act
            if (!string.IsNullOrEmpty(filter.Description))
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "descriptionx"))
                    .Append(" LIKE '%")
                    .Append(SqlHelper.SqlEncode(filter.Description))
                    .Append("%'");
            }
            if (filter.ActTypeId != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "typeId")).Append('=').Append(filter.ActTypeId);
            }
            if (filter.FamilyId != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "familyId")).Append('=').Append(filter.FamilyId);
            }
            if (filter.CompanyId != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "companyId")).Append('=').Append(filter.CompanyId);
            }
            if (filter.PlaceId != 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "placeId")).Append('=').Append(filter.PlaceId);
            }
            if (!string.IsNullOrEmpty(filter.Label))
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("act", "labelx"))
                    .Append(" LIKE '%")
                    .Append(SqlHelper.SqlEncode(filter.Label))
                    .Append("%'");
            }

            // categories
            if (filter.CategoryIds?.Count > 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("actCategory", "categoryId"))
                  .Append(" IN(")
                  .Append(string.Join(",",
                    filter.CategoryIds.Select(n =>
                        n.ToString(CultureInfo.InvariantCulture))))
                  .Append(')');
            }

            // professions
            if (filter.ProfessionIds?.Count > 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("actProfession", "professionId"))
                  .Append(" IN(")
                  .Append(string.Join(",",
                    filter.ProfessionIds.Select(n =>
                        n.ToString(CultureInfo.InvariantCulture))))
                  .Append(')');
            }

            // partners
            if (filter.PartnerIds?.Count > 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.Append(ETP("actPartner", "partnerId"))
                  .Append(" IN(")
                  .Append(string.Join(",",
                    filter.PartnerIds.Select(n =>
                        n.ToString(CultureInfo.InvariantCulture))))
                  .Append(')');
            }

            sb.Insert(0, "WHERE" + Environment.NewLine);
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Appends the paging instructions.
        /// </summary>
        /// <param name="options">The paging options.</param>
        /// <param name="sb">The target string builder.</param>
        private void AppendPaging(PagingOptions options, StringBuilder sb)
        {
            sb.Append("LIMIT ")
              .AppendLine(
                options.PageSize.ToString(CultureInfo.InvariantCulture))
              .Append("OFFSET ")
              .Append(options.GetSkipCount())
              .AppendLine();
        }

        /// <summary>
        /// Builds the SQL code corresponding to the specified filter and
        /// paging options for a list of acts.
        /// </summary>
        /// <param name="filter">The acts filter.</param>
        /// <returns>
        /// SQL code for both page and total.
        /// </returns>
        /// <exception cref="ArgumentNullException">filter</exception>
        public Tuple<string, string> BuildGetActs(ActFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            StringBuilder sbPage = new StringBuilder();
            StringBuilder sbTot = new StringBuilder();

            // fields
            sbPage.AppendLine("SELECT DISTINCT")
                .Append(ETPS("act",
                    "id", "bookId",
                    "typeId", "subtypeId", "familyId", "companyId", "placeId",
                    "label"))
                .AppendLine(",")
                .Append(ETPS("book",
                    "location", "description", "startYear", "endYear", "file"))
                .AppendLine(",")
                .Append(ETP("archive", "id")).Append(" AS archiveId").AppendLine(",")
                .Append(ETP("archive", "name")).Append(" AS archiveName").AppendLine(",")
                .Append(ETP("actType", "name")).Append(" AS actTypeName").AppendLine(",")
                .Append(ETP("actSubtype", "name")).Append(" AS actSubtypeName").AppendLine(",")
                .Append(ETP("family", "name")).Append(" AS familyName").AppendLine(",")
                .Append(ETP("company", "name")).Append(" AS companyName").AppendLine(",")
                .Append(ETP("place", "name")).Append(" AS placeName").AppendLine();

            sbTot.Append("SELECT COUNT(DISTINCT ")
                 .Append(ETP("item", "id"))
                 .AppendLine(")");

            // from ... join ...
            sbPage.Append("FROM ").AppendLine(ET("act"));
            AppendJoins(sbPage);

            sbTot.Append("FROM ").AppendLine(ET("act"));
            AppendJoins(filter, sbTot);

            // where
            if (!filter.IsEmpty())
            {
                string whereSql = BuildWhereSql(filter);
                sbPage.Append(whereSql);
                sbTot.Append(whereSql);
            }

            // order by (for page only)
            sbPage.Append("ORDER BY ")
              .Append(ETP("book", "startYear"))
              .Append(',').Append(ET("companyName"))
              .Append(',').Append(ET("familyName"))
              .Append(',').Append(ETP("act", "label"))
              .Append(',').AppendLine(ETP("act", "id"));

            // paging (for page only)
            AppendPaging(filter, sbPage);

            return Tuple.Create(sbPage.ToString(), sbTot.ToString());
        }

        private static string GetTableName(DataEntityType type)
        {
            return new[]
            {
                "act",
                "actType",
                "actSubtype",
                "archive",
                "book",
                "bookType",
                "bookSubtype",
                "category",
                "company",
                "family",
                "person",
                "place",
                "profession"
            }[(int)type];
        }

        private static string GetTableLookupFieldName(DataEntityType type)
        {
            return new[]
            {
                "labelx",
                "namex",
                "namex",
                "namex",
                "locationx",
                "namex",
                "namex",
                "namex",
                "namex",
                "namex",
                "namex",
                "namex",
                "namex"
            }[(int)type];
        }

        public string BuildLookup(DataEntityType type, string filter, int top)
        {
            string filterx = _filter.Apply(filter);
            string table = GetTableName(type);
            string field = GetTableLookupFieldName(type);

            StringBuilder sb = new StringBuilder("SELECT ");
            sb.Append(ET("id"))
              .Append(',')
              .Append(ET(field))
              .Append(" AS n FROM ")
              .Append(ET(table))
              .Append(" WHERE ")
              .Append(ETP(table, field))
              .Append(" LIKE '%").Append(SqlHelper.SqlEncode(filter))
              .Append("%' ORDER BY ").Append(ET(field))
              .Append(" LIMIT ").Append(top).AppendLine(";");
            return sb.ToString();
        }
    }
}
