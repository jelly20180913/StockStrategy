using System.Data.Entity;
namespace WebApiService.Models.DbContextFactory
{
    public partial class DynamicEntity : DbContext
    {
        public DynamicEntity(string connectionString)
            : base(connectionString)
        {
        }
    }
}