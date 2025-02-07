﻿#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Model
{
    public class Table
    {
        public string Schema_Name { get; set; }
        public string TableName { get; set; }
        public string Name { get; set; }
        public string ImageFile { get; set; }
        public string ImageReferenceFile { get; set; }
        public string ImageReferenceByFile { get; set; }
        public string MdFile { get; set; }
        public string TableUml { get; set; }
        public string ReferencesUml { get; set; }
        public string ReferenceBiesUml { get; set; }

        public virtual ICollection<Column> Columns { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<ReferenceBy> ReferenceBies { get; set; }
    }
}