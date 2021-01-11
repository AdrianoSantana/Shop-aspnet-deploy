using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// Endpoint => url
namespace Shop.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            return await context.Categories.AsNoTracking().ToListAsync();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            try
            {
                var categoriaProcurada = await context.Categories.AsNoTracking().FirstOrDefaultAsync(categoria => categoria.Id.Equals(id));
                if (categoriaProcurada == null)
                    return BadRequest(new { message = "Não existe nenhuma categoria com este Id" });

                return Ok(categoriaProcurada);
            }
            catch
            {
                return BadRequest(new { message = "Ocorreu um erro ao processar a requisição! " });
            }
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {

                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Put(
            int id,
            [FromBody] Category model,
            [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!model.Id.Equals(id))
                    return NotFound(new { message = "Categoria não encontrada" });

                context.Entry<Category>(model).State = EntityState.Modified;
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

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
        {
            try
            {
                var categoria = await context.Categories.FirstOrDefaultAsync(categoria => categoria.Id.Equals(id));
                if (categoria == null)
                    return NotFound(new { message = "Não existe categoria com este Id" });

                context.Remove(categoria);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria Removida com sucesso!" });
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }
    }
}
