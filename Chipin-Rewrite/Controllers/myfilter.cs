using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chipin_Rewrite.Controllers
{
    public class MyFilter : Attribute, IAsyncActionFilter
    {
        private readonly ChipinDbContext _context;


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            await next();
        }
    }
}
