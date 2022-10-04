using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Common;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using System;

namespace ClayService.API.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected UserIdentityBaseModel CurrentUser { get { return GetCurrentUser(); } }

        private UserIdentityBaseModel GetCurrentUser()
        {
            try
            {
                return new UserIdentityBaseModel
                {
                    UserName = HttpContext.User.Identity.GetUserName(),
                    FirstName = HttpContext.User.Identity.GetUserFirstName(),
                    LastName = HttpContext.User.Identity.GetUserLastName(),
                    DisplayName = HttpContext.User.Identity.GetUserDisplayName(),
                    Id = HttpContext.User.Identity.GetUserId<long>(),
                    Roles = HttpContext.User.Identity.GetUserClaimRoles()
                };
            }

            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
