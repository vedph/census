using Fusi.Tools.Data;
using System;

namespace Census.MySql
{
    public interface ISqlQueryBuilder
    {
        /// <summary>
        /// Builds the SQL code corresponding to the specified filter and
        /// paging options.
        /// </summary>
        /// <param name="filter">The acts filter.</param>
        /// <returns>SQL code for both page and total.</returns>
        Tuple<string, string> Build(ActFilter filter);
    }
}
