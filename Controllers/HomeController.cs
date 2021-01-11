using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User { Id = 1, Username = "employee", Password = "employee", Role = "employee" };
            var manager = new User { Id = 2, Username = "manager", Password = "manager", Role = "manager" };
            var category = new Category { Id = 1, Title = "Informática " };
            var product = new Product { Id = 1, Category = category, Title = "Teclado", Price = 134, Description = "Teclado gamer" };

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);

            await context.SaveChangesAsync();

            return Ok(
                new
                {
                    message = "Dados Configurados"
                }
            );

        }
    }
}
