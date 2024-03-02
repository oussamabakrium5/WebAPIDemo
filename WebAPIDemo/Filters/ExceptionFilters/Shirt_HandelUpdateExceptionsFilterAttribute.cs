using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Repositories;

namespace WebAPIDemo.Filters.ExceptionFilters
{
    public class Shirt_HandelUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ShirtRepository _shirtRepository;
        public Shirt_HandelUpdateExceptionsFilterAttribute(ShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            // RouteData do it as string
            var strShirtId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strShirtId, out int shirtId))
            {
                if (!_shirtRepository.ShirtExists(shirtId))
                {
                    context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist anymore.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}
