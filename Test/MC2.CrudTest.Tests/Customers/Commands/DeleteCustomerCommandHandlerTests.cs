using MC2.CurdTest.Application.Customers.Commands;
using MC2.CurdTest.Common;
using MC2.CurdTest.Domain.MC2Entities;
using MC2.CurdTest.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using static MC2.CurdTest.Application.Customers.Commands.DeleteCustomerCommand;
using Xunit;
using Shouldly;

namespace MC2.CrudTest.Tests.Customers.Commands
{
    public class DeleteCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Mark_Customer_As_Deleted()
        {
            // Arrange
            var customerId = 1;
            var command = new DeleteCustomerCommand
            {
                Id = customerId,
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
                    FirstName = "naser",
                    LastName = "ahmadi",
                    DateOfBirth = DateTime.UtcNow.AddYears(-24),
                    PhoneNumber = "03141145211",
                    Email = "ahmadi@test.com",
                    BankAccountNumber = "797733994593232103",
                });
                await context.SaveChangesAsync();
            }

            using (var context = new MC2DbConetxt(contextOptions))
            {
                var handler = new DeleteCustomerCommandHandler(context);

                // Act
                var response = await handler.Handle(command, CancellationToken.None);

                // Assert
                response.ResultCode.ShouldBe(ResponseType.success);

                // Verify that the customer was marked as deleted
                var deletedCustomer = await context.Customers.FindAsync(customerId);
                deletedCustomer.ShouldNotBeNull();
                deletedCustomer.IsDeleted.ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Handle_Should_Return_Error_If_Customer_Not_Found()
        {
            // Arrange
            var customerId = 1;
            var command = new DeleteCustomerCommand
            {
                Id = customerId
            };

            var contextOptions = new DbContextOptionsBuilder<MC2DbConetxt>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new MC2DbConetxt(contextOptions))
            {
                var handler = new DeleteCustomerCommandHandler(context);

                // Act
                var response = await handler.Handle(command, CancellationToken.None);

                // Assert
                response.ResultCode.ShouldBe(ResponseType.badRequest);
                response.ResultDesc.ShouldBe("Customer not found");
            }
        }
    }
}
