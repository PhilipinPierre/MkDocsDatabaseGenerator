using MkDocsDatabaseGenerator.Model;
using MkDocsDatabaseGenerator.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator
{
    public class DatabaseGenerator
    {
        private ICollection<Table> tables;

        public ICollection<Table> Tables
        {
            get { return tables; }
            private set { tables = value; }
        }

        private ICollection<Column> columns;

        public ICollection<Column> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        private ICollection<Reference> references;

        public ICollection<Reference> References
        {
            get { return references; }
            private set { references = value; }
        }

        private ICollection<ReferenceBy> referenceBies;

        public ICollection<ReferenceBy> ReferenceBies
        {
            get { return referenceBies; }
            private set { referenceBies = value; }
        }
        private String server;
        private String database;

        public DatabaseGenerator(string server, string database)
        {
            this.server = server;
            this.database = database;
            Init();
        }

        private void Init()
        {
            using (IData_Service service = new Data_Service(server: server, database: database))
            {
                // Retrieve data

                Tables = service.GetTables();
                Columns = service.GetColumns();
                References = service.GetReferences();
                ReferenceBies = service.GetReferenceBys();

                //Link data

                foreach (Table table in Tables)
                {
                    table.Columns = Columns.Where(t => t.TableName == table.TableName).ToList();
                    table.References = References.Where(r => r.TableName == table.TableName).ToList();
                    table.ReferenceBies = ReferenceBies.Where(r => r.TableName == table.TableName).ToList();
                }
                foreach (Column column in Columns)
                {
                    if (column.TableName != null)
                        column.Table = Tables.SingleOrDefault(t => t.TableName == column.TableName);
                    column.References = References.Where(r => r.TableName == column.TableName && r.ColumnName == column.ColumnName).ToList();
                    column.ReferenceBies = ReferenceBies.Where(r => r.TableName == column.TableName && r.ColumnName == column.ColumnName).ToList();
                }

                foreach (Reference reference in References)
                {
                    if (reference.TableName != null)
                        reference.Table = Tables.SingleOrDefault(t => t.TableName == reference.TableName);
                    if (reference.Referenced_TableName != null)
                        reference.Referenced_Table = Tables.SingleOrDefault(t => t.TableName == reference.Referenced_TableName);
                    if (!String.IsNullOrEmpty(reference.TableName) && !String.IsNullOrEmpty(reference.ColumnName))
                        reference.Column = Columns.SingleOrDefault(c => c.TableName == reference.TableName && c.ColumnName == reference.ColumnName);
                    if (!String.IsNullOrEmpty(reference.TableName) && !String.IsNullOrEmpty(reference.ColumnName))
                        reference.Referenced_Column = Columns.SingleOrDefault(c => c.TableName == reference.TableName && c.ColumnName == reference.ColumnName);
                }

                foreach (ReferenceBy reference in ReferenceBies)
                {
                    if (reference.TableName != null)
                        reference.Table = Tables.SingleOrDefault(t => t.TableName == reference.TableName);
                    if (reference.Referenced_By_TableName != null)
                        reference.Referenced_By_Table = Tables.SingleOrDefault(t => t.TableName == reference.Referenced_By_TableName);
                    if (!String.IsNullOrEmpty(reference.TableName) && !String.IsNullOrEmpty(reference.ColumnName))
                        reference.Column = Columns.SingleOrDefault(c => c.TableName == reference.TableName && c.ColumnName == reference.ColumnName);
                    if (!String.IsNullOrEmpty(reference.TableName) && !String.IsNullOrEmpty(reference.ColumnName))
                        reference.Referenced_By_Column = Columns.SingleOrDefault(c => c.TableName == reference.TableName && c.ColumnName == reference.ColumnName);
                }

            }
        }
    }
}
