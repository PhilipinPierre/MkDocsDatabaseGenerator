using MkDocsDatabaseGenerator.Model;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator.Service
{
    public class Data_Service : IData_Service
    {
        private const String baseConnectionString = "server={0};database={1};Trusted_Connection=True;Encrypt=false;";

        private const String QueryDatabasesNaes = @"
            SELECT name
            FROM sys.databases
            WHERE database_id > 5;";

        private const String QueryTableInfo = @"
            SELECT DISTINCT
	            s.name as [Schema_Name]
	            , t.name as TableName
            FROM sys.schemas AS s
            JOIN sys.tables AS t ON t.schema_id = s.schema_id
            JOIN sys.columns AS c ON c.object_id = t.object_id
            join sys.types AS ty on ty.system_type_id = c.system_type_id
            WHERE t.name <> '__RefactorLog' AND t.name <> 'sysdiagrams'";

        private const String QueryColumnInfo = @"
            SELECT
	            s.name AS [SchemaName]
	            , tab.name AS TableName
	            , c.column_id as ColumnId
                , c.name AS ColumnName
                , typ.Name AS ColumnType
                , c.max_length AS ColumnLength
                , c.precision AS ColumnPrecision
                , ISNULL(i.is_primary_key, 0) AS IsPrimaryKey
                , c.is_nullable AS IsNullable
	            , c.is_computed AS IsComputed
	            , c.is_identity AS IsIdentity
            FROM sys.schemas AS s
            JOIN sys.tables AS tab ON tab.schema_id = s.schema_id
            JOIN sys.columns AS c ON c.object_id = tab.object_id
            INNER JOIN sys.types typ ON c.user_type_id = typ.user_type_id
            LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
            WHERE tab.name <> '__RefactorLog' AND tab.name <> 'sysdiagrams'";

        private const String QueryReference = @"
            SELECT
                sch.name AS [Schema_Name],
                tab1.name AS [Table_Name],
                col1.name AS [Column_name],
	            col1.is_nullable AS [Column_Nullable],
	            obj.name AS FK_Name,
                tab2.name AS [Referenced_Table],
                col2.name AS [Referenced_Column],
                col2.is_nullable AS [Referenced_Column_Nullable]
            FROM sys.foreign_key_columns fkc
            INNER JOIN sys.objects obj ON obj.object_id = fkc.constraint_object_id
            INNER JOIN sys.tables tab1 ON tab1.object_id = fkc.parent_object_id
            INNER JOIN sys.schemas sch ON tab1.schema_id = sch.schema_id
            INNER JOIN sys.columns col1 ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id
            INNER JOIN sys.tables tab2 ON tab2.object_id = fkc.referenced_object_id
            INNER JOIN sys.columns col2 ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id
            WHERE tab1.name <> '__RefactorLog' AND tab1.name <> 'sysdiagrams' AND tab2.name <> '__RefactorLog' AND tab2.name <> 'sysdiagrams'";

        private const String QueryReferenceBy = @"
            SELECT
                sch.name AS [Schema_Name]
                ,tab2.name AS [TableName]
                ,col2.name AS [Column_Name]
	            ,col2.is_nullable AS [Column_Nullable]
                ,tab1.name AS [Referenced_By_Table]
	            ,obj.name AS [Referenced_By_FK_Name]
                ,col1.name AS [Referenced_By_Column]
                ,col1.is_nullable AS [Referenced_By_Column_Nullable]
            FROM sys.foreign_key_columns fkc
            INNER JOIN sys.objects obj ON obj.object_id = fkc.constraint_object_id
            INNER JOIN sys.tables tab1 ON tab1.object_id = fkc.parent_object_id
            INNER JOIN sys.schemas sch ON tab1.schema_id = sch.schema_id
            INNER JOIN sys.columns col1 ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id
            INNER JOIN sys.tables tab2 ON tab2.object_id = fkc.referenced_object_id
            INNER JOIN sys.columns col2 ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id
            WHERE tab1.name <> '__RefactorLog' AND tab1.name <> 'sysdiagrams' AND tab2.name <> '__RefactorLog' AND tab2.name <> 'sysdiagrams'";

        private String database;

        private String server;

        private SqlConnection Connection { get; set; }

        public Data_Service(string server, string database)
        {
            this.database = database;
            this.server = server;
            this.Connection = new SqlConnection(String.Format(baseConnectionString, server, database));
        }

        public Data_Service(string server)
        {
            this.database = "master";
            this.server = server;
            this.Connection = new SqlConnection(String.Format(baseConnectionString, server, database));
        }

        public ICollection<String> GetDatabases()
        {
            this.Connection.Open();
            SqlCommand command = new SqlCommand(QueryTableInfo, Connection);
            SqlDataReader dataReader = command.ExecuteReader(); ;

            IList<String> values = new List<String>();
            while (dataReader.Read())
            {
                var databaseName = dataReader.GetString(0);
                values.Add(databaseName);
            }

            dataReader.Close();
            command.Dispose();
            this.Connection.Close();
            return values.Distinct().OrderBy(v => v).ToList();
        }

        public ICollection<Table> GetTables()
        {
            this.Connection.Open();
            SqlCommand command = new SqlCommand(QueryTableInfo, Connection);
            SqlDataReader dataReader = command.ExecuteReader(); ;

            IList<Table> values = new List<Table>();
            while (dataReader.Read())
            {
                var schemaName = dataReader.GetString(0);
                var tableName = dataReader.GetString(1);
                values.Add(new Table()
                {
                    Schema_Name = schemaName,
                    TableName = tableName,
                    Name = tableName.Replace('_', ' '),
                    ImageFile = tableName.ToLower() + ".svg",
                    ImageReferenceFile = tableName.ToLower() + "_link.svg",
                    ImageReferenceByFile = tableName.ToLower() + "_linkby.svg",
                    MdFile = tableName.ToLower() + ".md",
                });
            }

            dataReader.Close();
            command.Dispose();
            this.Connection.Close();
            return values.DistinctBy(v => v.TableName).OrderBy(v => v.TableName).ToList();
        }

        public ICollection<Column> GetColumns()
        {
            this.Connection.Open();
            SqlCommand command = new SqlCommand(QueryColumnInfo, Connection);
            SqlDataReader dataReader = command.ExecuteReader(); ;

            IList<Column> values = new List<Column>();
            while (dataReader.Read())
            {
                values.Add(new Column()
                {
                    SchemaName = dataReader.GetString(0),
                    TableName = dataReader.GetString(1),
                    ColumnId = dataReader.GetInt32(2),
                    ColumnName = dataReader.GetString(3),
                    ColumnType = dataReader.GetString(4),
                    ColumnLength = dataReader.GetInt16(5),
                    ColumnPrecision = dataReader.GetByte(6),
                    IsPrimaryKey = dataReader.GetBoolean(7),
                    IsNullable = dataReader.GetBoolean(8),
                    IsComputed = dataReader.GetBoolean(9),
                    IsIdentity = dataReader.GetBoolean(10),
                });
            }

            dataReader.Close();
            command.Dispose();
            this.Connection.Close();
            return values.DistinctBy(v => new { v.TableName, v.ColumnName }).OrderBy(v => v.TableName).ThenBy(v => v.ColumnName).ToList();
        }

        public ICollection<Reference> GetReferences()
        {
            this.Connection.Open();
            SqlCommand command = new SqlCommand(QueryReference, Connection);
            SqlDataReader dataReader = command.ExecuteReader(); ;

            IList<Reference> values = new List<Reference>();
            while (dataReader.Read())
            {
                values.Add(new Reference()
                {
                    Schema_Name = dataReader.GetString(0),
                    TableName = dataReader.GetString(1),
                    ColumnName = dataReader.GetString(2),
                    ColumnNullable = dataReader.GetBoolean(3),
                    FK_Name = dataReader.GetString(4),
                    Referenced_TableName = dataReader.GetString(5),
                    Referenced_ColumnName = dataReader.GetString(6),
                    Referenced_ColumnNullable = dataReader.GetBoolean(7),
                });
            }

            dataReader.Close();
            command.Dispose();
            this.Connection.Close();
            return values.DistinctBy(v => new { v.TableName, v.ColumnName, v.Referenced_TableName, v.Referenced_ColumnName }).ToList();
        }

        public ICollection<ReferenceBy> GetReferenceBys()
        {
            this.Connection.Open();
            SqlCommand command = new SqlCommand(QueryReferenceBy, Connection);
            SqlDataReader dataReader = command.ExecuteReader(); ;

            IList<ReferenceBy> values = new List<ReferenceBy>();
            while (dataReader.Read())
            {
                values.Add(new ReferenceBy()
                {
                    Schema_Name = dataReader.GetString(0),
                    TableName = dataReader.GetString(1),
                    ColumnName = dataReader.GetString(2),
                    ColumnNullable = dataReader.GetBoolean(3),
                    Referenced_By_TableName = dataReader.GetString(4),
                    Referenced_By_FK_Name = dataReader.GetString(5),
                    Referenced_By_ColumnName = dataReader.GetString(6),
                    Referenced_By_ColumnNameNullable = dataReader.GetBoolean(7),
                });
            }

            dataReader.Close();
            command.Dispose();
            this.Connection.Close();
            return values.DistinctBy(v => new { v.TableName, v.ColumnName, v.Referenced_By_TableName, v.Referenced_By_ColumnName }).ToList();
        }

        public void Dispose()
        {
            try
            {
                this.Connection.Dispose();
            }
            catch
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
        }
    }
}