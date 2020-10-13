using System.Collections.Generic;
using Fusi.Tools.Data;

namespace Census.Core
{
    public interface ICensusRepository
    {
        DataPage<ActInfo> GetActs(ActFilter filter);

        IList<LookupItem> Lookup(DataEntityType type, string filter, int top);

        Act GetAct(int id);
    }
}
