using FluentResults;
using Microsoft.AspNetCore.Mvc;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.PostsService.Domain.Services;

namespace StarboardSocial.PostsService.Server.Controllers;

[ApiController]
[Route("public")]
public class PublicController(IPublicService publicService) : ControllerBase
{
    private readonly IPublicService _publicService = publicService;

    [HttpGet]
    [Route("feed")]
    public async Task<IActionResult> GetFeed(int page = 0, int pageSize = 10)
    {
        Result<List<Post>> result = await _publicService.GetFeed(page, pageSize);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}