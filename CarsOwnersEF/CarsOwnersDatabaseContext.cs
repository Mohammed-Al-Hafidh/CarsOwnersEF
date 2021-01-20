using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsOwnersEF
{
    class CarsOwnersDatabaseContext : DbContext
    {
        const string databasename = "CarsOwnersDatabase.mdf";
        static string DbPath = Path.Combine(Environment.CurrentDirectory, databasename);
        public CarsOwnersDatabaseContext() : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DbPath};Integrated Security=True;Connect Timeout=30")
        {

        }
        
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
