using SSO.Application.Contracts.Infrastructure;
using System;

namespace SSO.Infrastructure.Services
{
    internal class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}