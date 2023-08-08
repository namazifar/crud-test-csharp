using AutoMapper;
using AutoMapper.QueryableExtensions;
using MC2.CurdTest.Application.Customers.Models;
using MC2.CurdTest.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MC2.CurdTest.Application.Customers.Queries.GetAllCustomers
{
    public class CustomerListViewModel
    {
        public List<CustomerDto> Customers { get; set; }
    }
    public class GetAllCustomerListQuery : IRequest<CustomerListViewModel>
    {
        public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomerListQuery, CustomerListViewModel>
        {
            private readonly IMC2DbConetxt _context;
            private readonly IMapper _mapper;
            public GetAllCustomersQueryHandler(IMC2DbConetxt context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CustomerListViewModel> Handle(GetAllCustomerListQuery query, CancellationToken cancellationToken)
            {
                return new CustomerListViewModel()
                {
                    Customers = await _context.Customers
                    .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
                };
            }
        }
    }
}
