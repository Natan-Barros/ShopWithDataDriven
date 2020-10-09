using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Shop.Services;

namespace Shop.Controllers 
{
  [Route("v1/users")]
  public class UserController : Controller 
  {
    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Post(
      [FromServices] DataContext context,
      [FromBody]User model)
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
          model.Role = "employee";

          context.Users.Add(model);
          await context.SaveChangesAsync();
          
          model.Password = "";

          return model;
      }
      catch (Exception)
      {
          return BadRequest(new { message = "Não foi possivel criar o usuário"});
      }
      
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Authenticate(
      [FromServices] DataContext context,
      [FromBody]User model)
    {

      var user = await context.Users
          .AsNoTracking()
          .Where(x => x.Username == model.Username && x.Password == model.Password)
          .FirstOrDefaultAsync();
      
      if(user == null)
        return NotFound(new {message = "Usuário ou senha inválidos"});
      
      var token = TokenService.GenereteToken(user);
      
      user.Password = "";
      
      return new 
      {
        user = user,
        token = token
      };
      
    }

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<List<User>>> GetAction([FromServices] DataContext context)
    {
        var users = await context
            .Users
            .AsNoTracking()
            .ToListAsync();

        return users;
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Put(
      [FromServices] DataContext context,
      int id,
      [FromBody]User model) 
    {
      if (!ModelState.IsValid)
          return BadRequest(ModelState);

      
      if (id != model.Id)
          return NotFound(new { message = "Usuário não encontrada"});

      try
      {
          context.Entry(model).State = EntityState.Modified;

          await context.SaveChangesAsync();

          return model;
      }
      catch (System.Exception)
      {
          return BadRequest(new {message = "Não foi possivel criar o usuario"});
      }
    }

  }
}