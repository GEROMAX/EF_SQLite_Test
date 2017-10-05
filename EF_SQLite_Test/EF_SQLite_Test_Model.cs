namespace EF_SQLite_Test
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EF_SQLite_Test_Model : DbContext
    {
        public EF_SQLite_Test_Model()
            : base("name=EF_SQLite_Test_Model")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
