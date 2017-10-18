using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.SQLite.EF6;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Migrations
{
    /// <summary>
    /// おれおれ実装
    /// </summary>
    public class SqliteMigrationSqlGenerator : MigrationSqlGenerator
    {
        #region Const

        private const string BATCH_TERMINATOR = ";\r\n";

        #endregion

        #region Property

        private List<MigrationStatement> MigrationStatements { get; } = new List<MigrationStatement>();

        #endregion

        /// <summary>
        /// Convert from MigrationOperations to SQLite commands.
        /// </summary>
        /// <param name="migrationOperations">migration operations</param>
        /// <param name="providerManifestToken">version token</param>
        /// <returns>commands for SQLite</returns>
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            this.ProviderManifest = this.getDbProviderManifest(providerManifestToken);

            foreach (dynamic operation in migrationOperations)
            {
                var ms = new MigrationStatement();
                ms.BatchTerminator = BATCH_TERMINATOR;
                ms.Sql = this.generateQuery(operation);
                this.MigrationStatements.Add(ms);
                //System.Windows.Forms.MessageBox.Show(ms.Sql);
            }

            return this.MigrationStatements;
        }

        private SqliteCommandBuilder createSqliteCommandBuilder()
        {
            return new SqliteCommandBuilder(this.ProviderManifest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerManifestToken"></param>
        /// <returns></returns>
        private DbProviderManifest getDbProviderManifest(string providerManifestToken)
        {
            return ((DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices))).GetProviderManifest(providerManifestToken);
        }

        private string generateQuery(MigrationOperation migrationOperation)
        {
            Debug.Assert(false);
            return string.Empty;
        }

        /// <summary>
        /// Create Table
        /// </summary>
        /// <param name="opeCreateTable"></param>
        /// <returns></returns>
        private string generateQuery(CreateTableOperation opeCreateTable)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("CREATE TABLE");
            scb.AppendEscapeDBO(opeCreateTable.Name);
            scb.NestStart();

            bool hasAutoIncrementColumn = false;
            foreach (ColumnModel column in opeCreateTable.Columns)
            {
                scb.AppendEscapeDBO(column.Name);

                if (column.IsIdentity)
                {
                    hasAutoIncrementColumn |= column.IsIdentity;
                    scb.Append("INTEGER").Append("CONSTRAINT");
                    scb.AppendConstraintName("PK", opeCreateTable.Name);
                    scb.Append("PRIMARY KEY AUTOINCREMENT,");
                }
                else
                {
                    scb.AppendColumnType(column);
                }
            }

            if (opeCreateTable.PrimaryKey != null && !hasAutoIncrementColumn)
            {
                scb.Append("PRIMARY KEY").AppendNestedColumnsStatement(opeCreateTable.PrimaryKey.Columns);
            }
            scb.TrimEndsComma().NestEnd();

            return scb.ToString();
        }

        /// <summary>
        /// Rename Table
        /// </summary>
        /// <param name="opeRenameTable"></param>
        /// <returns></returns>
        private string generateQuery(RenameTableOperation opeRenameTable)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("ALTER TABLE").AppendEscapeDBO(opeRenameTable.Name);
            scb.Append("RENAME TO").AppendEscapeDBO(opeRenameTable.NewName);

            return scb.ToString();
        }

        /// <summary>
        /// Drop Table
        /// </summary>
        /// <param name="opeDropTable"></param>
        /// <returns></returns>
        private string generateQuery(DropTableOperation opeDropTable)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("DROP TABLE").AppendEscapeDBO(opeDropTable.Name);

            return scb.ToString();
        }

        private string generateQuery(MoveTableOperation opeMoveTable)
        {
            throw new NotSupportedException("Move operations not supported by SQLite");
        }

        private string generateQuery(AddForeignKeyOperation opeAddForeignKey)
        {
            throw new NotSupportedException("Add ForeingKey not supported now");
        }

        private string generateQuery(DropForeignKeyOperation opeDropForeignKey)
        {
            throw new NotSupportedException("Drop ForeingKey not supported now");
        }

        /// <summary>
        /// Create Index
        /// </summary>
        /// <param name="opeCreateIndex"></param>
        /// <returns></returns>
        private string generateQuery(CreateIndexOperation opeCreateIndex)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("CREATE");
            if (opeCreateIndex.IsUnique)
            {
                scb.Append("UNIQUE");
            }
            scb.Append("INDEX").Append(opeCreateIndex.Name).Append("ON").AppendEscapeDBO(opeCreateIndex.Table);
            scb.AppendNestedColumnsStatement(opeCreateIndex.Columns);

            return scb.ToString();
        }

        /// <summary>
        /// Drop Index
        /// </summary>
        /// <param name="opeDropIndex"></param>
        /// <returns></returns>
        private string generateQuery(DropIndexOperation opeDropIndex)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("DROP").Append("INDEX").Append(opeDropIndex.Name);

            return scb.ToString();
        }

        /// <summary>
        /// Add Primary Key
        /// </summary>
        /// <param name="opeAddPK"></param>
        /// <returns></returns>
        private string generateQuery(AddPrimaryKeyOperation opeAddPK)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("ALTER TABLE").AppendEscapeDBO(opeAddPK.Table);
            scb.Append("ADD CONSTRAINT").Append(opeAddPK.Name);
            scb.Append("PRIMARY KEY");
            scb.AppendNestedColumnsStatement(opeAddPK.Columns);

            return scb.ToString();
        }

        /// <summary>
        /// Drop Primary Key
        /// </summary>
        /// <param name="opeDropPK"></param>
        /// <returns></returns>
        private string generateQuery(DropPrimaryKeyOperation opeDropPK)
        {
            var scb = new SqliteCommandBuilder(this.ProviderManifest);

            scb.Append("ALTER TABLE").AppendEscapeDBO(opeDropPK.Table);
            scb.Append("DROP CONSTRAINT").Append(opeDropPK.Name, false);

            return scb.ToString();
        }

        /// <summary>
        /// Add Column
        /// </summary>
        /// <param name="opeAddColumn"></param>
        /// <returns></returns>
        private string generateQuery(AddColumnOperation opeAddColumn)
        {
            var scb = this.createSqliteCommandBuilder();

            scb.Append("ALTER TABLE").AppendEscapeDBO(opeAddColumn.Table);
            scb.Append("ADD").AppendColumnType(opeAddColumn.Column);

            return scb.ToString();
        }

        private string generateQuery(RenameColumnOperation opeRenomearColuna)
        {
            throw new NotSupportedException("Rename column not supported by SQLite");
        }

        private string generateQuery(DropColumnOperation opeDropColumn)
        {
            throw new NotSupportedException("Drop column not supported by SQLite");
        }

        private string generateQuery(AlterColumnOperation migrationOperation)
        {
            throw new NotSupportedException("Alter column not supported by SQLite");
        }

        private string generateQuery(HistoryOperation migrationOperation)
        {
            var scb = this.createSqliteCommandBuilder();

            foreach (DbModificationCommandTree commandTree in migrationOperation.CommandTrees)
            {
                switch (commandTree.CommandTreeKind)
                {
                    case DbCommandTreeKind.Insert:
                        //scb.Append(QueryBuilder.GenerateInsertSql((DbInsertCommandTree)commandTree, out parameters, true));
                        break;
                    case DbCommandTreeKind.Delete:
                        //scb.Append(DmlBuilder.GenerateDeleteSql((DbDeleteCommandTree)commandTree, out parameters, true));
                        break;
                    case DbCommandTreeKind.Update:
                        //scb.Append(DmlBuilder.GenerateUpdateSql((DbUpdateCommandTree)commandTree, out parameters, true));
                        break;
                    case DbCommandTreeKind.Function:
                    case DbCommandTreeKind.Query:
                    default:
                        throw new InvalidOperationException(string.Format("Command tree of type {0} not supported in migration of history operations", commandTree.CommandTreeKind));
                }
                scb.Append(BATCH_TERMINATOR);
            }

            return scb.ToString();
        }
    }
}
