#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Model
{
    public class Reference
    {
        public string Schema_Name { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public bool ColumnNullable { get; set; }
        public string FK_Name { get; set; }
        public string Referenced_TableName { get; set; }
        public string Referenced_ColumnName { get; set; }
        public bool Referenced_ColumnNullable { get; set; }

        public virtual Table Table { get; set; }
        public virtual Column Column { get; set; }
        public virtual Table Referenced_Table { get; set; }
        public virtual Column Referenced_Column { get; set; }
    }
}