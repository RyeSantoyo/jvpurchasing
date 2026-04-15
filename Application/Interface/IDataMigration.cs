using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Application.Interface
{
    public interface IDataMigration
    {
        Task<int> MigratePOAsync();
    }
}