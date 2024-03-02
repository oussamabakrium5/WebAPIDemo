using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Repositories;

namespace WebAPIDemo.Filters.ActionFilters
{
    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ShirtRepository _shirtRepository;

        public Shirt_ValidateShirtIdFilterAttribute(ShirtRepository shirtRepository)
        {
            _shirtRepository = shirtRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // we do id not ShirtId  public IActionResult GetShirtById(int id) not  public IActionResult GetShirtById(int ShirtId)
            // so we get the id and we put it in ShirtId var
            var ShirtId = context.ActionArguments["id"] as int?;
            if (ShirtId.HasValue)
            {
                if (ShirtId.Value <= 0)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId is invalid.");
                    //problemDetails is a way to normalaze err messages, the same format of message
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else if (!_shirtRepository.ShirtExists(ShirtId.Value))
                {
                    // if value >=1 and shirt not existe
                    context.ModelState.AddModelError("ShirtId", "Shirt doesn't existe");
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
