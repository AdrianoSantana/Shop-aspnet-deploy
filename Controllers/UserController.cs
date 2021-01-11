using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromBody] User model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                model.Role = "employee"; // Força o usuário ser sempre employee
                context.Users.Add(model);
                await context.SaveChangesAsync();
                model.Password = "";
                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar um usuário " });
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]

        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
                            .AsNoTracking()
                            .Where(usuario => usuario.Username.Equals(model.Username) && usuario.Password.Equals(model.Password))
                            .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos " });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Authorize(Roles = "manager")]

        public async Task<ActionResult<List<User>>> GetUsers([FromServices] DataContext context)
        {
            try
            {
                var users = await context.Users.AsNoTracking().ToListAsync();
                return users;
            }
            catch
            {
                return BadRequest(new { message = "Ocorreu um erro ao processar requisição." });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "manager")]

        public async Task<ActionResult> DeleteUsers([FromServices] DataContext context, int id)
        {
            try
            {
                var usuarioProcurado = context.Users.AsNoTracking().SingleOrDefault(usuario => usuario.Id.Equals(id));
                context.Remove(usuarioProcurado);
                await context.SaveChangesAsync();
                return Ok(new { message = "Usuário deletado com sucesso!" });
            }
            catch
            {
                return BadRequest(new { message = "Ocorreu um erro ao processar requisição." });
            }
        }
    }
}
