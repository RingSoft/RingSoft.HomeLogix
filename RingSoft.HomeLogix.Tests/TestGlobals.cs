using System;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Main;
using RingSoft.HomeLogix.Sqlite;
using RingSoft.HomeLogix.Tests;

namespace RingSoft.DevLogix.Tests
{
    public class TestGlobals<TViewModel, TView> : DbMaintenanceTestGlobals<TViewModel, TView>
        where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public new HomeLogixTestDataRepository DataRepository { get; } 
            
        public TestGlobals() : base(new HomeLogixTestDataRepository(new TestDataRegistry()))
        {
            DataRepository = base.DataRepository as HomeLogixTestDataRepository;
        }

        public override void Initialize()
        {
            AppGlobals.UnitTesting = true;
            AppGlobals.Initialize();
            AppGlobals.LookupContext.Initialize(new SqliteHomeLogixDbContext(), DbPlatforms.Sqlite);
            AppGlobals.MainViewModel = new MainViewModel();

            base.Initialize();
        }

        public override void ClearData()
        {

            base.ClearData();
            LoadDatabase();
        }


        private void LoadDatabase()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
        }
    }
}
