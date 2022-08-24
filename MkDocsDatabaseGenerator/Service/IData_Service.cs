using MkDocsDatabaseGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Service
{
    public interface IData_Service : IDisposable
    {

        public ICollection<Table> GetTables();
        public ICollection<Column> GetColumns();
        public ICollection<Reference> GetReferences();
        public ICollection<ReferenceBy> GetReferenceBys();

    }
}
