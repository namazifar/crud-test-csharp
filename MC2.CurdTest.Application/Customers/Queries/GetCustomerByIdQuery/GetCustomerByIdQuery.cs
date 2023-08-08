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

namespace MC2.CurdTest.Application.Customers.Queries.GetCustomerByIdQuery
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public int Id { get; set; }
        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
        {
            private readonly IMC2DbConetxt _context;
            private readonly IMapper _mapper;
            public GetCustomerByIdQueryHandler(IMC2DbConetxt context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerDto> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var customer = await _context.Customers
                            .Where(x => x.Id == query.Id)
                            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);
                if (customer == null)
                    return null;
                return customer;
            }


        }
    }
}
