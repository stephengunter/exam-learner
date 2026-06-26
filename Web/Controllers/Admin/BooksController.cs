using ApplicationCore.Authorization;
using ApplicationCore.Consts;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.Views;
using ApplicationCore.Web.Controllers;
using Infrastructure.Helpers;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers.Admin;

public class BooksController : BaseAdminController
{
   private readonly IBookService _bookService;     
   public BooksController(IBookService booksService)
   {
      _bookService = booksService; 
   }
   [HttpGet("init")]
   public async Task<ActionResult<BooksIndexModel>> Init()
   {
      bool active = true;
      var request = new BooksFetchRequest(active);

      return new BooksIndexModel(request);
   }
   [HttpGet]
   public async Task<ActionResult<IEnumerable<Book>>> Index(bool active = true)
   {
      var list = await _bookService.FetchAsync();

      if (list.HasItems())
      {
         list = list.Where(x => x.Active == active);

         list = list.GetOrdered();
      }
      return list.ToList();
   }


   [HttpGet("create")]
   public ActionResult<BookAddRequest> Create() => new BookAddRequest(new BookAddForm());


   [HttpPost]
   public async Task<ActionResult<Book>> Store([FromBody] BookAddForm model)
   {
      await ValidateRequestAsync(model, 0);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var entity = new Book();
      model.SetValuesTo(entity);
      if (model.Active) entity.Order = 0;
      else entity.Order = -1;

      entity = await _bookService.CreateAsync(entity, User.Id());

      return Ok(entity);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<Book>> Details(int id)
   {
      var entity = await _bookService.GetByIdAsync(id);
      if (entity == null) return NotFound();
      return entity;
   }
   [HttpGet("edit/{id}")]
   public async Task<ActionResult<BookEditRequest>> Edit(int id)
   {
      var entity = await _bookService.GetByIdAsync(id);
      if (entity == null) return NotFound();

      var form = new BookEditForm();
      entity.SetValuesTo(form);

      form.CanRemove = !entity.Active;

      return new BookEditRequest(form);
   }

   [HttpPut("{id}")]
   public async Task<ActionResult> Update(int id, [FromBody] BookEditForm model)
   {
      var entity = await _bookService.GetByIdAsync(id);
      if (entity == null) return NotFound();

      await ValidateRequestAsync(model, id);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      model.SetValuesTo(entity);
      if (model.Active) entity.Order = 0;
      else entity.Order = -1;

      await _bookService.UpdateAsync(entity, User.Id());

      return NoContent();
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> Remove(int id)
   {
      var entity = await _bookService.GetByIdAsync(id);
      if (entity == null) return NotFound();
     
      await _bookService.RemoveAsync(entity, User.Id());

      return NoContent();
   }

   async Task ValidateRequestAsync(BookBaseForm model, int id)
   {
      var labels = new BookLabels();
      if (String.IsNullOrEmpty(model.Title)) ModelState.AddModelError(nameof(model.Title), ValidationMessages.Required(labels.Title));

   }


}