#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Model
{
    public class Column
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public Int16 ColumnLength { get; set; }
        public Byte ColumnPrecision { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsComputed { get; set; }
        public bool IsIdentity { get; set; }

        public virtual Table Table { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<ReferenceBy> ReferenceBies { get; set; }
    }
}