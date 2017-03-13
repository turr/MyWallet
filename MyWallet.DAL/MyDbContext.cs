using MyWallet.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.DAL
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("qdm211529417_dbEntities")
        {
        }

        public DbSet<t_manager> t_manager { get; set; }
        //public DbSet<T_Role> role { get; set; }
        public DbSet<t_summary> t_summary { get; set; }
        public DbSet<t_record_type> t_record_type { get; set; }
        public DbSet<t_summary_record> t_summary_record { get; set; }
        public DbSet<t_loan_type> t_loan_type { get; set; }
        public DbSet<t_setting> t_setting { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
