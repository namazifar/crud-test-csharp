using MC2.CurdTest.Application.Customers.Commands;
using MC2.CurdTest.Common;
using MC2.CurdTest.Domain.MC2Entities;
using MC2.CurdTest.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MC2.CurdTest.Application.Customers.Commands.UpdateCustomerCommand;
using Xunit;
using Shouldly;

namespace MC2.CrudTest.Tests.Customers.Commands
{
    public class UpdateCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Existing_Customer()
        {
            // Arrange
            var customerId = 1;
            var command = new UpdateCustomerCommand
            {
                Id = customerId,
                FirstName = "mona",
                LastName = "alami",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "+989122814456",
                Email = "alami@test.com",
                BankAccountNumber = "12345678912"
            };

            var contextOptions = new DbContextOptionsBuilder<MC2DbConetxt>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new MC2DbConetxt(contextOptions))
            {
                // Seed the in-memory database with an existing customer
                context.Customers.Add(new Customer
                {
                    Id = customerId,
                    FirstName = "mona",
                    LastName = "alami",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    PhoneNumber = "+989122814456",
                    BankAccountNumber = "12345678912",
                    Email = "existing@test.com" // Different email than the one being updated
                });
                await context.SaveChangesAsync();
            }

            using (var context = new MC2DbConetxt(contextOptions))
            {

                var handler = new UpdateCustomerCommandHandler(context);

                // Act
                var response = await handler.Handle(command, CancellationToken.None);

                // Assert
                response.ResultCode.ShouldBe(ResponseType.success);

                // Verify that the customer was updated with the new data
                var updatedCustomer = await context.Customers.FindAsync(customerId);
                updatedCustomer.ShouldNotBeNull();
                updatedCustomer.FirstName.ShouldBe(command.FirstName);
                updatedCustomer.LastName.ShouldBe(command.LastName);
                updatedCustomer.DateOfBirth.ShouldBe(command.DateOfBirth);
                updatedCustomer.PhoneNumber.ShouldBe(command.PhoneNumber);
                updatedCustomer.Email.ShouldBe(command.Email);
                updatedCustomer.BankAccountNumber.ShouldBe(command.BankAccountNumber);
            }
        }

        [Fact]
        public async Task Handle_Should_Return_Error_If_Customer_Not_Found()
        {
            // Arrange
            var customerId = 8;
            var command = new UpdateCustomerCommand
            {
                Id = customerId,
                FirstName = "mona",
                LastName = "alami",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "+9891234567890",
                Email = "mona.alami@example.com",
                BankAccountNumber = "123456789"
            };

            var contextOptions = new DbContextOptionsBuilder<MC2DbConetxt>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new MC2DbConetxt(contextOptions))
            {
                var handler = new UpdateCustomerCommandHandler(context);

                // Act
                var response = await handler.Handle(command, CancellationToken.None);

                // Assert
                response.ResultCode.ShouldBe(ResponseType.badRequest);
                response.ResultDesc.ShouldBe("Customer not found");
            }
        }
    }
}
