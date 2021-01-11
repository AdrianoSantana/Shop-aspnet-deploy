using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products
                            .Include(produto => produto.Category)
                            .AsNoTracking()
                            .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]

        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products
                            .Include(produto => produto.Category)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(produto => produto.Id.Equals(id));

            return Ok(product);
        }

        [HttpGet]
        [Route("categories/product/{id:int}")]
        [AllowAnonymous]

        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
                                        .Include(produto => produto.Category)
                                        .AsNoTracking()
                                        .Where(x => x.CategoryId.Equals(id))
                                        .ToListAsync();

            return products;
        }

        [HttpPost]
        [Authorize(Roles = "employee, manager")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Products.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "manager")]

        public async Task<ActionResult> Delete([FromServices] DataContext context, int id)
        {
            try
            {
                var produtoProcurado = await context.Products
                                            .AsNoTracking()
                                            .SingleOrDefaultAsync(produto => produto.Id.Equals(id));

                context.Remove(produtoProcurado);
                await context.SaveChangesAsync();
                return Ok(new { message = "Produto deletado com sucesso! " });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel completar requisição! " });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Put([FromServices] DataContext context, int id, [FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!model.Id.Equals(id))
                    return NotFound(new { message = "Produto não encontrada" });

                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado!" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel atualizar a categoria " });
            }
        }
    }
}
