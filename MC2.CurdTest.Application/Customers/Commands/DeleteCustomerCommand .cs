using MC2.CurdTest.Application.Interfaces;
using MC2.CurdTest.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MC2.CurdTest.Application.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<MC2Response>
    {
        public int Id { get; set; }

        public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, MC2Response>
        {
            private readonly IMC2DbConetxt _context;

            public DeleteCustomerCommandHandler(IMC2DbConetxt context)
            {
                _context = context;
            }
            public async Task<MC2Response> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == command.Id);
                if (customer == null)
                {
                    return new MC2Response { ResultCode = ResponseType.badRequest, ResultDesc = "Customer not found" };
                }

                customer.IsDeleted = true;

                await _context.SaveChangesAsync(cancellationToken);

                return new MC2Response();
            }
        }
    }
}
