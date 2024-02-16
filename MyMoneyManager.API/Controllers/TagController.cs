using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.Shared.ViewModels;
using System.Security.Claims;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMoneyManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }
    //GET: api/<TagController>
    [HttpGet]
    public async Task<IEnumerable<TagViewModel>> Get(CancellationToken cancellationToken)
    {
        return await _tagService.GetUserTags(HttpContext, cancellationToken);
    }

    // GET api/<TagController>/5
    [HttpGet("{id}")]
    public TagViewModel Get(int id, CancellationToken cancellationToken)
    {
        return _tagService.GetUserTag(HttpContext, id, cancellationToken);
    }

    // POST api/<TagController>
    [HttpPost]
    public async Task Post([FromBody] TagViewModel tagViewModel, CancellationToken cancellationToken)
    {
        await _tagService.CreateUserTag(HttpContext, tagViewModel, cancellationToken);
    }

    // PUT api/<TagController>/5
    [HttpPut("{id}")]
    public async Task Put(int id, [FromBody] TagViewModel tagViewModel, CancellationToken cancellationToken)
    {
        tagViewModel.Id = id;
        await _tagService.UpdateUserTag(HttpContext, tagViewModel, cancellationToken);
    }

    // DELETE api/<TagController>/5
    [HttpDelete("{id}")]
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        await _tagService.DeleteUserTag(HttpContext, id, cancellationToken);
    }
}
