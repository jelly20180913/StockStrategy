using System.Web.Http;
using Unity;
using Unity.WebApi;
using WebApiService.Models.DbContextFactory;
using Unity.Injection;
using Unity.Lifetime;
using WebApiService.Models;
using WebApiService.Models.Repository;
using WebApiService.Services.Interface.Tables;
using WebApiService.Services.Implement.Tables;
using WebApiService.Services.Interface;
using WebApiService.Services.Implement;

namespace WebApiService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>(); 

		    string _ConnectionString = "name=StockWarehouseEntities"; 
			//string _ConnectionString = "name=AzureStockWarehouseEntities";
			var dbContext = new DynamicEntity(_ConnectionString);
            container.RegisterType<IDbContextFactory, DbContextFactory>(
           new HierarchicalLifetimeManager(),
           new InjectionConstructor(_ConnectionString));

            container.RegisterType<IStockService, StockService>();
            container.RegisterType<ITokenService, TokenService>();
            container.RegisterType<IStockGroupService, StockGroupService>();
            container.RegisterType<IStockIndexService, StockIndexService>();
            container.RegisterType<IStockInventoryService, StockInventoryService>();
            container.RegisterType<IStockLineNotifyService, StockLineNotifyService>();
            container.RegisterType<IStockFutureCodeService, StockFutureCodeService>();
			container.RegisterType<IHolidayService, HolidayService>();
			container.RegisterType<ILoginService, LoginService>();
			container.RegisterType<IStockStrategyService, StockStrategyService>();
			container.RegisterType<IStockPickingService, StockPickingService>();
			container.RegisterType<IStockBacktestService, StockBacktestService>();
			container.RegisterType<IStockResultService, StockResultService>();
			container.RegisterType<IStockHighLowService, StockHighLowService>();
			container.RegisterType<IStockGroupTrendService, StockGroupTrendService>();
			container.RegisterType<IStockThreeInstitutionalService, StockThreeInstitutionalService>();
			container.RegisterType<IStockGroupTotalCountService, StockGroupTotalCountService>();
			container.RegisterType<IStockEventNotifyService, StockEventNotifyService>();
			container.RegisterType<IStockRevenueService, StockRevenueService>();
			container.RegisterType<IStockEpsService, StockEpsService>();
			container.RegisterType<IStockChipsService, StockChipsService>();

			container.RegisterType<IRepository<StockGroup>, Repository<StockGroup>>();
            container.RegisterType<IRepository<StockIndex>, Repository<StockIndex>>();
            container.RegisterType<IRepository<StockInventory>, Repository<StockInventory>>();
            container.RegisterType<IRepository<Login>, Repository<Login>>();
            container.RegisterType<IRepository<Stock>, Repository<Stock>>();
            container.RegisterType<IRepository<StockLineNotify>, Repository<StockLineNotify>>();
            container.RegisterType<IRepository<StockFutureCode>, Repository<StockFutureCode>>();
			container.RegisterType<IRepository<Holiday>, Repository<Holiday>>();
			container.RegisterType<IRepository<StockPicking>, Repository<StockPicking>>();
			container.RegisterType<IRepository<StockBacktest>, Repository<StockBacktest>>();
			container.RegisterType<IRepository<StockResult>, Repository<StockResult>>();
			container.RegisterType<IRepository<StockHighLow>, Repository<StockHighLow>>();
			container.RegisterType<IRepository<StockGroupTrend>, Repository<StockGroupTrend>>();
			container.RegisterType<IRepository<StockThreeInstitutional>, Repository<StockThreeInstitutional>>();
			container.RegisterType<IRepository<StockGroupTotalCount>, Repository<StockGroupTotalCount>>();
			container.RegisterType<IRepository<StockEventNotify>, Repository<StockEventNotify>>();
			container.RegisterType<IRepository<StockRevenue>, Repository<StockRevenue>>();
			container.RegisterType<IRepository<StockEps>, Repository<StockEps>>();
			container.RegisterType<IRepository<StockChips>, Repository<StockChips>>();
			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}