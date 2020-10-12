using System;

namespace Census.Core
{
    public interface ISqlQueryBuilder
    {
        /// <summary>
        /// Builds the SQL code corresponding to the specified filter and
        /// paging options for a list of acts.
        /// </summary>
        /// <param name="filter">The acts filter.</param>
        /// <returns>SQL code for both page and total.</returns>
        Tuple<string, string> BuildActSearch(ActFilter filter);

        string BuildLookup(DataEntityType type, string filter, int top);
    }

    public enum DataEntityType
    {
        Act = 0,
        ActType,
        ActSubtype,
        Archive,
        Book,
        BookType,
        BookSubtype,
        Category,
        Company,
        Family,
        Person,
        Place,
        Profession
    }
}
