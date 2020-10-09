using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers 
{
  [Route("v1/products")]
  public class ProductController : Controller 
  {

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get([FromServices]DataContext context) 
    {
      var product = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .ToListAsync();

      return Ok(product);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    public async Task<ActionResult<List<Product>>> GetByCategory([FromServices]DataContext context, int id) 
    {
      var products = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .Where(x => x.CategoryId == id)
          .ToListAsync();

      return products;
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id, [FromServices]DataContext context) 
    {
      var product = await context
          .Products
          .Include(x => x.Category)
          .AsNoTracking()
          .FirstOrDefaultAsync(x => x.Id == id);

      return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Post([FromBody] Product Product, [FromServices]DataContext context) 
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      
      try
      {
        context.Products.Add(Product);
        await context.SaveChangesAsync();
        return Ok(Product);
      }
      catch 
      {
          return BadRequest(new { message = "Não foi possivel criar o produto"});
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<Product>> Put(int id, [FromBody] Product Product, [FromServices] DataContext context) 
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Entry<Product>(Product).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(Product);
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new {message = "Este registro ja foi atualizado"});
      }
      catch (Exception)
      {
        return BadRequest(new {message = "Não foi possivel atualizar o produto"});
      }

    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<Product>> Delete(int id, [FromServices] DataContext context) 
    {
      
      var Product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
      if (Product == null)
        return NotFound(new { message = "Produto não encontrado"});

      try
      {
          context.Products.Remove(Product);
          await context.SaveChangesAsync();
          return Ok(new {message = "Produto removida com sucesso."});
      }
      catch (Exception)
      {
          return BadRequest(new {message = "Não foi possível remover a categoria."});
      }

    }


  }
}