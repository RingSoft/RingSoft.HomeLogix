using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HomeLogixLookupContext : LookupContext
    {
        public override DbDataProcessor DataProcessor => SqliteDataProcessor;

        public SqliteDataProcessor SqliteDataProcessor { get; }
        
        protected override DbContext DbContext => _dbContext;

        private DbContext _dbContext;

        public HomeLogixLookupContext()
        {
            SqliteDataProcessor = new SqliteDataProcessor();
            //_dbContext = 
        }

        protected override void InitializeLookupDefinitions()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetupModel()
        {
            throw new System.NotImplementedException();
        }

    }
}
