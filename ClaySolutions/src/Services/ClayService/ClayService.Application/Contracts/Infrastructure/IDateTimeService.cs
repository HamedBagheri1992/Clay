using System;

namespace ClayService.Application.Contracts.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}
