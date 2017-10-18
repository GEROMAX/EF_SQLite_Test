using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Migrations.Model;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Migrations
{
    public class SqliteCommandBuilder
    {
        #region Const

        private const string DATETIME_FORMAT = "yyyy-MM-dd hh:mm:ss";
        private const string ESCAPE_DBO = "dbo.";

        #endregion

        #region Property

        private StringBuilder CommandText { get; } = new StringBuilder();
        public DbProviderManifest ProviderManifest { get; private set; }

        #endregion

        public SqliteCommandBuilder(DbProviderManifest providerManifest)
        {
            this.ProviderManifest = providerManifest;
        }

        public SqliteCommandBuilder Append(string value, bool addWordBreak = true)
        {
            this.CommandText.Append(value + (addWordBreak ? " " : ""));
            return this;
        }

        public SqliteCommandBuilder AppendEscapeDBO(string value, bool addWordBreak = true)
        {
            return this.Append(value.Substring(value.StartsWith(ESCAPE_DBO, StringComparison.OrdinalIgnoreCase) ? 4 : 0), addWordBreak);
        }

        public bool IsEndsWithSpace()
        {
            return this.CommandText.ToString().EndsWith(" ");
        }

        public SqliteCommandBuilder NestStart()
        {
            this.CommandText.Append(this.IsEndsWithSpace() ? "(" : " (");
            return this;
        }

        public SqliteCommandBuilder NestEnd()
        {
            this.CommandText.Append(")");
            return this;
        }

        public SqliteCommandBuilder TrimEndsComma()
        {
            if (this.CommandText.ToString().EndsWith(", "))
            {
                this.CommandText.Remove(this.CommandText.Length - 2, 2);
            }
            return this;
        }

        public SqliteCommandBuilder AppendColumnType(ColumnModel cm, bool addComma = true)
        {
            var typeName = this.ProviderManifest.GetStoreType(cm.TypeUsage).EdmType.Name;
            var typeLength = string.Empty;
            switch (typeName)
            {
                case "decimal":
                case "numeric":
                    typeLength = string.Format(CultureInfo.InvariantCulture, "({0}, {1})",
                                               cm.Precision ?? 10,
                                               cm.Scale ?? 0);
                    break;
                case "binary":
                case "varbinary":
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                    typeLength = string.Format("({0})", cm.MaxLength ?? 255);
                    break;
                default:
                    break;
            }

            this.Append(typeName + typeLength);
            if (null != cm.DefaultValue)
            {
                this.Append("DEFAULT").AppendValueStatement((dynamic)cm.DefaultValue);
            }
            else if (!string.IsNullOrWhiteSpace(cm.DefaultValueSql))
            {
                this.Append("DEFAULT").Append(cm.DefaultValueSql);
            }
            this.Append((cm.IsNullable ?? true) ? "NULL" : "NOT NULL", false);

            return addComma ? this.Append(",") : this;
        }

        public SqliteCommandBuilder AppendConstraintName(string prefix, string objectName)
        {
            return this.Append(prefix + "_", false).AppendEscapeDBO(objectName);
        }

        public SqliteCommandBuilder AppendNestedColumnsStatement(IList<string> columnNames)
        {
            this.NestStart();
            foreach (string columnName in columnNames)
            {
                this.Append(columnName + ",");
            }
            return this.TrimEndsComma().NestEnd();
        }

        public SqliteCommandBuilder AppendValueStatement(string value)
        {
            return this.Append("'" + value + "'");
        }

        public SqliteCommandBuilder AppendValueStatement(byte[] values)
        {
            var sb = new StringBuilder();
            foreach (var byteData in values)
            {
                sb.Append(byteData.ToString("X2", CultureInfo.InvariantCulture));
            }
            return this.Append("x'" + sb + "'");
        }

        public SqliteCommandBuilder AppendValueStatement(bool value)
        {
            return this.Append(value ? "1" : "0");
        }

        public SqliteCommandBuilder AppendValueStatement(DateTime value)
        {
            return this.AppendValueStatement(value.ToString(DATETIME_FORMAT, CultureInfo.InvariantCulture));
        }

        public SqliteCommandBuilder AppendValueStatement(object value)
        {
            return this.Append(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }

        public override string ToString()
        {
            return this.CommandText.ToString();
        }
    }
}
