using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

[Route("categories")]
public class CategoryController : Controller 
{

  [HttpGet]
  public async Task<ActionResult<List<Category>>> Get() 
  {
    return Ok(new Category());
  }
  
  [HttpGet]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> GetById(int id) 
  {
    return new Category();
  }

  [HttpPost]
  public async Task<ActionResult<Category>> Post([FromBody] Category category) 
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    return Ok(category);
  }

  [HttpPut]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> Put(int id, [FromBody] Category category) 
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    return Ok(new Category());
  }

  [HttpDelete]
  [Route("{id:int}")]
  public async Task<ActionResult<Category>> Delete(int id) 
  {
    return Ok(new Category());
  }


}