using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Processors;

namespace PetFamily.Framework.Filters
{
    public class UserScopedDataFilter : IAsyncActionFilter
    {
        private readonly UserScopedDataProccessor _userScopedDataProccessor;

        public UserScopedDataFilter(UserScopedDataProccessor userScopedDataProccessor)
        {
            _userScopedDataProccessor = userScopedDataProccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var scopedDataResult = _userScopedDataProccessor.Process();
            if(scopedDataResult.IsFailure)
            {
                context.Result = scopedDataResult.Error.ToResponse();
                return;
            }

            context.HttpContext.Items.Add(ApplicationController.USER_SCOPED_DATA_KEY, scopedDataResult.Value);

            await next();
        }
    }
}
