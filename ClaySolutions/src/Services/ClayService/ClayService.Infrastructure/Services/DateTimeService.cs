using ClayService.Application.Contracts.Infrastructure;
using System;

namespace ClayService.Infrastructure.Services
{
    internal class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}