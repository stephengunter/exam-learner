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

public class BookChaptersController : BaseAdminController
{
   private readonly IBookService _bookService;
   private readonly IBookChapterService _bookChapterService;     
   public BookChaptersController(IBookService booksService, IBookChapterService bookChapterService)
   {
      _bookService = booksService;
      _bookChapterService = bookChapterService;
   }
   [HttpGet("init/{book}")]
   public async Task<ActionResult<BookChaptersIndexModel>> Init(int book)
   {
      var bookEntity = await _bookService.GetByIdAsync(book);
      if (bookEntity == null) return NotFound();
      var request = new BookChaptersFetchRequest(book);
      return new BookChaptersIndexModel(request, bookEntity);
   }
   [HttpGet]
   public async Task<ActionResult<IEnumerable<Book>>> Index(int book)
   {
      var list = await _bookChapterService.FetchAsync(new Book() { Id = book });

      if (list.HasItems())
      {
         list = list.GetOrdered();
      }
      return list.ToList();
   }


   [HttpGet("create")]
   public ActionResult<BookChapterAddRequest> Create() => new BookChapterAddRequest(new BookChapterAddForm());


   [HttpPost]
   public async Task<ActionResult<Book>> Store([FromBody] BookChapterAddForm model)
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
   public async Task<ActionResult<BookChapterEditRequest>> Edit(int id)
   {
      var entity = await _bookService.GetByIdAsync(id);
      if (entity == null) return NotFound();

      var form = new BookChapterEditForm();
      entity.SetValuesTo(form);

      form.CanRemove = !entity.Active;

      return new BookChapterEditRequest(form);
   }

   [HttpPut("{id}")]
   public async Task<ActionResult> Update(int id, [FromBody] BookChapterEditForm model)
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

   async Task ValidateRequestAsync(BookChapterBaseForm model, int id)
   {
      var labels = new BookChapterLabels();
      if (String.IsNullOrEmpty(model.Title)) ModelState.AddModelError(nameof(model.Title), ValidationMessages.Required(labels.Title));

   }


}