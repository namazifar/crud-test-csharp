using MC2.CurdTest.Application.Customers.Commands;
using MC2.CurdTest.Application.Customers.Queries.GetAllCustomers;
using MC2.CurdTest.Application.Customers.Queries.GetCustomerByIdQuery;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CustomerController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCustomerListQuery()));
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddCustomer(AddCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetCustomerByIdQuery { Id = id }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCustomerCommand { Id = id }));
        }
    }
}
