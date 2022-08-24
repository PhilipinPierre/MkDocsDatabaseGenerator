using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Model
{
    public class Schema
    {
        public string SchemaName { get; set; }

        public virtual ICollection<Table> Tables { get; set; }
    }
}
