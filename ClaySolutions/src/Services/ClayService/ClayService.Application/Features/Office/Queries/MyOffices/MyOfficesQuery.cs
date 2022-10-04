using ClayService.Application.Features.Office.Queries.GetOffice;
using MediatR;
using System.Collections.Generic;

namespace ClayService.Application.Features.Office.Queries.MyOffices
{
    public class MyOfficesQuery : IRequest<List<OfficeDto>>
    {
        public long UserId { get; set; }

        public MyOfficesQuery(long userId)
        {
            UserId = userId;
        }
    }
}
