﻿using System;
using System.Collections.Generic;
using System.Text;
using Fusi.Tools.Data;

namespace Census.Core
{
    public interface ICensusRepository
    {
        DataPage<ActInfo> GetActs(ActFilter filter);

        IList<Tuple<int, string>> Lookup(DataEntityType type, string filter,
            int top);
        // TODO
    }
}
