using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OESK
{
    public class SQLiteConnection : DbContext
    {
        public SQLiteConnection(string connString) : base(connString)
        { }

        public DbSet<TableAlgorithm> TableAlgorithm { get; set; }
        public DbSet<TableTest> TableTest { get; set; }
        public DbSet<TableTestResult> TableTestResult { get; set; }
        public DbSet<TableText> TableText { get; set; }
    }
}
