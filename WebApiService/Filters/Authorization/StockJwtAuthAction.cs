using System.Web.Http.Controllers;

namespace WebApiService.Filters.Authorization
{
    public class StockJwtAuthAction : BaseJwtAuthAction
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.Secret = this.GetType().Name;
            base.OnActionExecuting(actionContext);
        }

    }
}