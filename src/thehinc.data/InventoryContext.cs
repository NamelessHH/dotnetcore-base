using Microsoft.EntityFrameworkCore;
using thehinc.data.Contracts;

namespace thehinc.data {
    public class InventoryContext : DbContext, IInventoryContext {

        private string _conectionString;
        public InventoryContext (string ConnectionString) : base () { }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer (_conectionString);
        }
    }
}