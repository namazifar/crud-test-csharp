using MC2.CurdTest.Application.Customers.Commands;
using MC2.CurdTest.Application.Customers.Validator;
using MC2.CurdTest.Common;
using MC2.CurdTest.Domain.MC2Entities;
using MC2.CurdTest.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static MC2.CurdTest.Application.Customers.Commands.AddCustomerCommand;

namespace MC2.CrudTest.Tests.Customers.Commands
{
    public class AddCustomerCommandTests
    {

        [Fact]
        public async Task Handle_Should_Add_Customer_To_Context()
        {
            var contextOptions = new DbContextOptionsBuilder<MC2DbConetxt>()
              .UseInMemoryDatabase(databaseName: "TestDatabase")
              .Options;
            using (var context = new MC2DbConetxt(contextOptions))
            {

                var handler = new AddCustomerCommand.AddCustomerCommandHandler(context);

                var command = new AddCustomerCommand
                {
                    FirstName = "sajjad",
                    LastName = "rezaee",
                    DateOfBirth = DateTime.UtcNow.AddYears(-24),
                    PhoneNumber = "+989141145211",
                    Email = "sajjad@test.com",
                    BankAccountNumber = "497777994593332103",
                };

                var validation = await new AddCustomerValidator().ValidateAsync(command);
                validation.IsValid.ShouldBeTrue();

                var response = await handler.Handle(command, CancellationToken.None);

                response.ResultCode.ShouldBe(ResponseType.success);

                // Verify that no new customer was added
                context.Customers.Count().ShouldBe(1);
            }
        }


        [Fact]
        public async Task AddCustomer_WhenEmailExistsBefore_ShouldBeFailed()
        {
            // Arrange
            var command = new AddCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "+9891234567890",
                Email = "test@example.com",
                BankAccountNumber = "123456789"
            };

            var contextOptions = new DbContextOptionsBuilder<MC2DbConetxt>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new MC2DbConetxt(contextOptions))
            {
                // Seed the in-memory database with an existing customer
                context.Customers.Add(new Customer
                {
                    FirstName = "mm",
                    LastName = "zzz",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    PhoneNumber = "+9891834567890",
                    Email = "test@example.com",
                    BankAccountNumber = "4213385672133"
                });
                await context.SaveChangesAsync();
            }

            using (var context = new MC2DbConetxt(contextOptions))
            {
                var handler = new AddCustomerCommandHandler(context);

                // Act
                var response = await handler.Handle(command, CancellationToken.None);

                // Assert
                response.ResultCode.ShouldBe(ResponseType.logicalError);
                response.ResultDesc.ShouldBe("this email address already exists");
            }
        }

        [Fact]
        public async Task Should_Error_When_All_Properties_Are_ValidAsync()
        {
            var command = new AddCustomerCommand
            {
                FirstName = "mahan",
                LastName = "fani",
                Email = "mahan#example.com",
                PhoneNumber = "+123456789"
            };

            var validation = await new AddCustomerValidator().ValidateAsync(command);
            validation.IsValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_All_Properties_Are_ValidAsync()
        {
            var command = new AddCustomerCommand
            {
                FirstName = "yaser",
                LastName = "Daemi",
                Email = "Daemi@example.com",
                PhoneNumber = "+989369211249"
            };

            var validation = await new AddCustomerValidator().ValidateAsync(command);
            validation.IsValid.ShouldBeTrue();
        }
    }
}
