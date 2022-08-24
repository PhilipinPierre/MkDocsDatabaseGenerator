using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Model
{
    public class ReferenceBy
    {
        public string Schema_Name { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public bool ColumnNullable { get; set; }
        public string Referenced_By_TableName { get; set; }
        public string Referenced_By_FK_Name { get; set; }
        public string Referenced_By_ColumnName { get; set; }
        public bool Referenced_By_ColumnNameNullable { get; set; }


        public virtual Table Table { get; set; }
        public virtual Column Column { get; set; }
        public virtual Table Referenced_By_Table { get; set; }
        public virtual Column Referenced_By_Column { get; set; }
    }
}
