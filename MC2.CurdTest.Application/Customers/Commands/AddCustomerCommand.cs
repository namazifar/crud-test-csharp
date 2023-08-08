using MC2.CurdTest.Application.Interfaces;
using MC2.CurdTest.Common;
using MC2.CurdTest.Domain.MC2Entities;
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
    public class AddCustomerCommand : IRequest<MC2Response>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, MC2Response>
        {
            private readonly IMC2DbConetxt _context;
            public AddCustomerCommandHandler(IMC2DbConetxt context)
            {
                _context = context;
            }
            public async Task<MC2Response> Handle(AddCustomerCommand command, CancellationToken cancellationToken)
            {

                var itemCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email.ToLower() == command.Email.ToLower());

                if (itemCustomer is not null)
                    return new MC2Response { ResultCode = ResponseType.logicalError, ResultDesc = "this email address already exists" };


                Customer cust = new Customer()
                {
                    PhoneNumber = command.PhoneNumber,
                    BankAccountNumber = command.BankAccountNumber,
                    LastName = command.LastName,
                    FirstName = command.FirstName,
                    Email = command.Email,
                    DateOfBirth = command.DateOfBirth,
                    CreatedAt = DateTime.Now,
                };
                _context.Customers.Add(cust);
                await _context.SaveChangesAsync(cancellationToken);
                return new MC2Response();
            }
        }
    }
}
