using ApplicationCore.Authorization;
using ApplicationCore.Consts;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Models.Identity;
using ApplicationCore.Services;
using ApplicationCore.Views;
using ApplicationCore.Web.Controllers;
using Azure.Core;
using Infrastructure.Helpers;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Models;

namespace Web.Controllers.Admin;

public class BookArticlesController : BaseAdminController
{
   private readonly IBookService _bookService;
   private readonly IBookChapterService _chapterService;
   private readonly IBookArticleService _service;
   public BookArticlesController(IBookArticleService service, IBookService booksService, IBookChapterService chapterService)
   {
      _service = service;
      _bookService = booksService;
      _chapterService = chapterService;
   }
   [HttpGet("init")]
   public async Task<ActionResult<BookArticlesIndexModel>> Init(int? book, int? chapter)
   {
      var request = new BookArticlesFetchRequest(book, chapter);
      var model = new BookArticlesIndexModel(request);

      var books = await _bookService.FetchAsync();
      model.Books = books.GetOrdered().ToList();
      if (!book.HasValue) return model;

      model.Book = books.FirstOrDefault(x => x.Id == book.Value);
      var chapters = await _chapterService.FetchAsync(model.Book);
      if (chapters.HasItems())
      {
         chapters = chapters.GetOrdered();
         model.Chapters = chapters.ToList();
         if (chapter.HasValue) model.Chapter = chapters.FirstOrDefault(x => x.Id == chapter.Value);
      }
      return model;
      //var chapters = new List<Book>();
      //var bookEntity = new Book();

      //if (books.HasItems())
      //{
      //   books = books.GetOrdered();

      //   if(book.HasValue) bookEntity = books.FirstOrDefault(x => x.Id == book);
      //   if (bookEntity is null) bookEntity = books.First();
      //   var bookChapters = await _chapterService.FetchAsync(bookEntity);
      //   if (bookChapters.HasItems())
      //   {
      //      chapters = bookChapters.ToList();
      //      request.Chapter = chapters.FirstOrDefault()!.Id;
      //   } 
      //   request.Book = bookEntity.Id;
      //}
      
      
      
   }
   [HttpGet]
   public async Task<ActionResult<IEnumerable<ArticleViewModel>>> Index(int chapter)
   {
      var chapterEntity = await _chapterService.GetByIdAsync(chapter);
      if (chapterEntity == null) return NotFound();
      if (chapterEntity.IsRootItem) return NotFound();

      var includes = new List<string>() { nameof(Article.Items) };

      var list = await _service.FetchAsync(chapterEntity, includes);

      if (list.HasItems())
      {
         //list = list.Where(x => x.Active == active);

         foreach (var entity in list) 
         {
            entity.Items = entity.Items.GetOrdered().ToList();
         }
        

         list = list.GetOrdered();
      }
      var views = list.MapViewModelList();
      return views;
   }


   [HttpGet("create")]
   public ActionResult<BookArticleAddRequest> Create()
      => new BookArticleAddRequest(new BookArticleAddForm(), new List<ArticleItemAddForm>());


   [HttpPost]
   public async Task<ActionResult<Article>> Store([FromBody] BookArticleAddRequest request)
   {
      await ValidateRequestAsync(request.Form, 0);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var entity = new Article();
      request.Form.SetValuesTo(entity);
      if (request.Form.Active) entity.Order = 0;
      else entity.Order = -1;

      if (request.Items.HasItems())
      {
         entity.Items = CreateArticleItems(request.Items);
      }

      entity = await _service.CreateAsync(entity, User.Id());

      return Ok(entity);
   }

   List<ArticleItem> CreateArticleItems(IEnumerable<ArticleItemAddForm> models)
   {
      var items = new List<ArticleItem>();
      foreach (var model in models)
      {
         var itemEntity = new ArticleItem();
         model.SetValuesTo(itemEntity);
         items.Add(itemEntity);
      }
      return items;  
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<Article>> Details(int id)
   {
      var entity = await _service.GetByIdAsync(id);
      if (entity == null) return NotFound();
      return entity;
   }
   [HttpGet("edit/{id}")]
   public async Task<ActionResult<BookArticleEditRequest>> Edit(int id)
   {
      var includes = new List<string>() { nameof(Article.Items) };
      var entity = await _service.GetByIdAsync(id, includes);
      if (entity == null) return NotFound();

      var form = new BookArticleEditForm();
      entity.SetValuesTo(form);

      var itemForms = new List<ArticleItemEditForm>();
      foreach (var item in entity.Items)
      { 
         var itemForm = new ArticleItemEditForm();
         item.SetValuesTo(itemForm);
         itemForms.Add(itemForm);
      }

      form.CanRemove = !entity.Active;

      return new BookArticleEditRequest(form, itemForms);
   }

   [HttpPut("{id}")]
   public async Task<ActionResult> Update(int id, [FromBody] BookArticleEditRequest request)
   {
      var includes = new List<string>() { nameof(Article.Items) };
      var entity = await _service.GetByIdAsync(id, includes);
      if (entity == null) return NotFound();

      await ValidateRequestAsync(request.Form, id);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      request.Form.SetValuesTo(entity);
      if (request.Form.Active) entity.Order = 0;
      else entity.Order = -1;


      if (request.Items.IsNullOrEmpty())
      {
         entity.Items = new List<ArticleItem>();
      }
      else
      {
         // 1. 刪除不存在的
         var toRemove = entity.Items
          .Where(x => !request.Items.Any(i => i.Id == x.Id))
          .ToList();

         foreach (var item in toRemove) 
         {
            entity.Items.Remove(item);
         }

         // 2. 更新 & 新增
         foreach (var item in request.Items)
         {
            var existing = entity.Items.FirstOrDefault(x => x.Id == item.Id);
            if (existing != null)
            {
               // update
               item.SetValuesTo(existing);
               existing.SetUpdated(User.Id());
            }
            else
            {
               // add
               var newItem = new ArticleItem();
               item.SetValuesTo(newItem);
               newItem.SetCreated(User.Id());
               entity.Items.Add(newItem);
            }
         }

      }

      await _service.UpdateAsync(entity, User.Id());

      return NoContent();
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> Remove(int id)
   {
      var entity = await _service.GetByIdAsync(id);
      if (entity == null) return NotFound();
     
      await _service.RemoveAsync(entity, User.Id());

      return NoContent();
   }

   async Task ValidateRequestAsync(BookArticleBaseForm model, int id)
   {
      var labels = new BookArticleLabels();
      if (String.IsNullOrEmpty(model.Title)) ModelState.AddModelError(nameof(model.Title), ValidationMessages.Required(labels.Title));

   }


}