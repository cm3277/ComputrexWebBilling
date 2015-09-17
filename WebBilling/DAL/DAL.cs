using System.Data.Entity;
using WebBilling.Models;

namespace CSGOSideLoungeMVCTest4.DAL
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class CompXDbContext : DbContext
    {
        public CompXDbContext()
            : base("DefaultConnection")
        {
            this.Configuration.ValidateOnSaveEnabled = false;
            Database.SetInitializer<DbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<NarrationLine> NarrationLines { get; set; }
        public DbSet<Parts> Parts { get; set; }
        public DbSet<TechNotesLine> TechNotesLines { get; set; }
        public DbSet<Tolerances> Tolerances { get; set; }

        public System.Data.Entity.DbSet<WebBilling.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<WebBilling.Models.TechAccount> TechAccounts { get; set; }
    }

    /*public class MyDbInitializer : DropCreateDatabaseAlways<CompXDbContext>
    {
        protected override void Seed(CompXDbContext context)
        {
            base.Seed(context);
            //Can be used to auto populate any of the databases
            
            /*context.TechAccounts.Add(
              new TechAccount { firstName = "Mike", lastName = "Latsko", initials = "ML", permissionGroup = "Boss", userName = "mike", hashPassword = "pv+C/LdJ+TyhB6bZJu8aZ6Dvml7+Z55fBIMHqpY2YMs=" }
            );
            context.SaveChanges();*/
        //}
    //}
}