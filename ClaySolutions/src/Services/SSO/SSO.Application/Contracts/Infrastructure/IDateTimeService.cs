using System;

namespace SSO.Application.Contracts.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}
