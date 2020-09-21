using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using System;

[Route("categories")]
public class CategoryController : Controller 
{

  [HttpGet]
  public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context) 
  {
    var categories = await context.Categories.AsNoTracking().ToListAsync();
    return Ok(categories);
  }
  
  [HttpGet]
  [Route("{id:int}")] 
  public async Task<ActionResult<Category>> GetById(int id, [FromServices]DataContext context) 
  {
    var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    return Ok(category);
  }

  [HttpPost]
  public async Task<ActionResult<Category>> Post([FromBody] Category category, [FromServices]DataContext context) 
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    
    try
    {
      context.Categories.Add(category);
      await context.SaveChangesAsync();
      return Ok(category);
    }
    catch 
    {
        return BadRequest(new { message = "Não foi possivel criar a categoria"});
    }
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> Put(int id, [FromBody] Category category, [FromServices] DataContext context) 
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    try
    {
      context.Entry<Category>(category).State = EntityState.Modified;
      await context.SaveChangesAsync();
      return Ok(category);
    }
    catch (DbUpdateConcurrencyException)
    {
      return BadRequest(new {message = "Este registro ja foi atualizado"});
    }
    catch (Exception)
    {
      return BadRequest(new {message = "Não foi possivel atualizar a categoria"});
    }

  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> Delete(int id, [FromServices]DataContext context) 
  {
    
    var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    if (category == null)
      return NotFound(new { message = "Categoria não encontrada"});

    try
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(new {message = "Categoria removida com sucesso."});
    }
    catch (Exception)
    {
        return BadRequest(new {message = "Não foi possível remover a categoria."});
    }

  }


}