using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.Controllers
{
  [ApiController]
  [Route("v1/[controller]")]
  public class BookController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Book>>> Index([FromServices] Context context)
    {
      var books = await context.Books.ToListAsync();
      return books;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Book>> Get([FromServices] Context context, int id)
    {
      var book = await context.Books
      .Include(c => c.Category)
      .FirstOrDefaultAsync(c => c.Id == id);
      if (book is null)
        return NotFound();

      return book;
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    public async Task<ActionResult<List<Book>>> GetByCategory([FromServices] Context context, int id)
    {
      var books = await context.Books.Include(c => c.Category).AsNoTracking().Where(c => c.CategoryId == id).ToListAsync();
      return books;
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Book>> Post(
        [FromServices] Context context,
        [FromBody] Book model)
    {
      if (ModelState.IsValid)
      {
        context.Books.Add(model);
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
    [FromBody] Book model)
    {
      if (ModelState.IsValid)
      {
        context.Books.Update(model);
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
      var book = await context.Books.FirstOrDefaultAsync(c => c.Id == id);
      context.Books.Remove(book);
      await context.SaveChangesAsync();
      return NoContent();
    }
  }
}