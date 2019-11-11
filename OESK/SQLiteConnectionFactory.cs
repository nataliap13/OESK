using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OESK
{
    //source
    //https://stackoverflow.com/questions/15963726/getting-context-is-not-constructible-add-a-default-constructor-or-provide-an-i
    class SQLiteConnectionFactory : IDbContextFactory<SQLiteConnection>
    {
        public SQLiteConnection Create()
        {
            return new SQLiteConnection("db.db");
        }
    }
}
