using backend.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests {
    public class DbContextFactory {
        
        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "BookDbTest").Options;

        public static DatabaseContext GetDatabaseContext() {
            return new DatabaseContext(dbContextOptions);
        }

    }
}
