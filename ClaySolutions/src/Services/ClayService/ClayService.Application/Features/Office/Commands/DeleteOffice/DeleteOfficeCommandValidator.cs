using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClayService.Application.Features.Office.Commands.DeleteOffice
{
    public class DeleteOfficeCommandValidator : AbstractValidator<DeleteOfficeCommand>
    {
        public DeleteOfficeCommandValidator()
        {
            RuleFor(p => p.Id)
              .GreaterThan(0)
              .WithMessage("{OfficeId} is required.");
        }
    }
}
