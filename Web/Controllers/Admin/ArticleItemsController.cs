using ApplicationCore.Authorization;
using ApplicationCore.Consts;
using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.Views;
using ApplicationCore.Web.Controllers;
using Autofac.Core;
using Azure.Core;
using Infrastructure.Helpers;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using Web.Models;

namespace Web.Controllers.Admin;

public class ArticleItemsController : BaseAdminController
{
   private readonly IArticleItemService _service;
   public ArticleItemsController(IArticleItemService service)
   {
      _service = service;
   }

   [HttpGet("create")]
   public ActionResult<ArticleItemAddForm> Create()
      => new ArticleItemAddForm();

   [HttpPost]
   public async Task<ActionResult<List<HighlightViewModel>>> Store([FromBody] ArticleItemHighlightAddForm form)
   {
      var entity = await _service.GetByIdAsync(form.ItemId);
      if (entity == null) return NotFound();

      await ValidateRequestAsync(form);
      if (!ModelState.IsValid) return BadRequest(ModelState);

      // ¸ÑªRÂÂªº
      var highlights = entity.Highlights.ParseHighlights();

      var highLight = new HighlightViewModel()
      {
         Text = form.Text.Trim()
      };
      highlights.Add(highLight);
      entity.Highlights = JsonSerializer.Serialize(highlights);

      await _service.UpdateAsync(entity, User.Id());

      return Ok(highlights);
   }
   async Task ValidateRequestAsync(ArticleItemHighlightBaseForm model)
   {
      var labels = new BookArticleLabels();
      if (String.IsNullOrEmpty(model.Text)) ModelState.AddModelError(nameof(model.Text), ValidationMessages.Required(labels.Title));

   }


}