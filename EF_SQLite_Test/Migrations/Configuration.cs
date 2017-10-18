namespace EF_SQLite_Test.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.SQLite.EF6;
    using System.Diagnostics;
    using System.Data.Entity.Core.Common.CommandTrees;

    internal sealed class Configuration : DbMigrationsConfiguration<EF_SQLite_Test.EF_SQLite_Test_Model>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("System.Data.SQLite", new SqliteMigrationSqlGenerator());
        }

        protected override void Seed(EF_SQLite_Test.EF_SQLite_Test_Model context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
