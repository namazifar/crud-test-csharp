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
    public class UpdateCustomerCommand : IRequest<MC2Response>
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, MC2Response>
        {
            public UpdateCustomerCommandHandler(IMC2DbConetxt context)
            {
                _context = context;
            }

            private readonly IMC2DbConetxt _context;
            public async Task<MC2Response> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == command.Id.Value);

                if (customer is null)
                    return new MC2Response { ResultCode = ResponseType.badRequest, ResultDesc = "Customer not found" };


                if (customer.Email.ToLower() != command.Email.ToLower())
                {
                    var itemCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email.ToLower() == command.Email.ToLower());

                    if (itemCustomer is not null)
                        return new MC2Response { ResultCode = ResponseType.logicalError, ResultDesc = "this email address already exists" };
                }

                customer.FirstName = command.FirstName;
                customer.LastName = command.LastName;
                customer.DateOfBirth = command.DateOfBirth;
                customer.PhoneNumber = command.PhoneNumber;
                customer.Email = command.Email;
                customer.BankAccountNumber = command.BankAccountNumber;

                await _context.SaveChangesAsync(cancellationToken);

                return new MC2Response();
            }
        }
    }
}

