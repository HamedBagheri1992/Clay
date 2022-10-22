using ClayService.Application.Contracts.Persistence;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

namespace ClayService.Application.Features.Tag.Commands.CreateTag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        private readonly ITagRepository _tagRepository;

        public CreateTagCommandValidator(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;

            RuleFor(p => p.TagCode)
               .NotEmpty()
               .NotNull()
               .WithMessage("{TagCode} is required.")
               .MustAsync(BeUniqueTagCode).WithMessage("The specified tagCode already exists.");
        }

        public async Task<bool> BeUniqueTagCode(string tagCode, CancellationToken arg2)
        {
            return await _tagRepository.IsUniqueTagCodeAsync(tagCode);
        }
    }
}
