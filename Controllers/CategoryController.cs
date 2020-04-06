using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.Controllers
{
  [ApiController]
  [Route("v1/[controller]")]
  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices] Context context)
    {
      var categories = await context.Categories.ToListAsync();
      return categories;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Get([FromServices] Context context, int id)
    {
      var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

      if (category is null)
        return NotFound();

      return category;
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Category>> Post(
        [FromServices] Context context,
        [FromBody] Category model)
    {
      if (ModelState.IsValid)
      {
        context.Categories.Add(model);
        await context.SaveChangesAsync();
        return model;
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    [HttpPut]
    [Route("edit")]
    public async Task<ActionResult<Category>> Put(
        [FromServices] Context context,
        [FromBody] Category model)
    {
      if (ModelState.IsValid)
      {
        context.Categories.Update(model);
        await context.SaveChangesAsync();
        return Accepted(model);
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<ActionResult> Delete([FromServices] Context context, int id)
    {
      var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
      context.Categories.Remove(category);
      await context.SaveChangesAsync();
      return NoContent();
    }
  }
}