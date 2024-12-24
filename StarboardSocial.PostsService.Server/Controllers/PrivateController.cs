using FluentResults;
using Microsoft.AspNetCore.Mvc;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.PostsService.Domain.Services;
using StarboardSocial.PostsService.Server.ViewModels;
using StarboardSocial.UserService.Server.Helpers;

namespace StarboardSocial.PostsService.Server.Controllers;

[ApiController]
[Route("private")]
public class PrivateController(IPrivateService privateService) : ControllerBase
{
    private readonly IPrivateService _privateService = privateService;

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            string userId = UserIdHelper.GetUserId(Request);
            Result<List<Post>> result = await _privateService.GetPosts(userId);
            
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        
    }
    
    [HttpGet]
    [Route("{postId}")]
    public async Task<IActionResult> GetPost([FromRoute] string postId)
    {
        try
        {
            string userId = UserIdHelper.GetUserId(Request);
            Result<Post> result = await _privateService.GetPost(userId, postId);
            
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateViewModel postCreateViewModel)
    {
        try
        {
            string userId = UserIdHelper.GetUserId(Request);
            Post post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = postCreateViewModel.Title,
                Description = postCreateViewModel.Description,
                Image = new()
                {
                    Id = Guid.NewGuid(),
                    Image = new MemoryStream(Convert.FromBase64String(postCreateViewModel.ImageBase64)),
                    ImageExtension = postCreateViewModel.ImageExtension
                },
                PostedAt = DateTimeOffset.Now
            };
            Result<Post> result = await _privateService.CreatePost(post);
            
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }
    
    /*[HttpPut]
    [Route("{postId}")]
    public async Task<IActionResult> UpdatePost([FromRoute] string postId, [FromBody] PostEditViewModel postEditViewModel)
    {
        try
        {
            string userId = UserIdHelper.GetUserId(Request);
            Post post = new()
            {
                Id = Guid.Parse(postId),
                UserId = userId,
                Title = postEditViewModel.Title,
                Description = postEditViewModel.Description,
                ImageUrl = postEditViewModel.ImageUrl,
                PostedAt = postEditViewModel.PostedAt
            };
            Result<Post> result = await _privateService.UpdatePost(post);
            
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        
    }*/
    
    [HttpDelete]
    [Route("{postId}")]
    public async Task<IActionResult> DeletePost([FromRoute] string postId)
    {
        try
        {
            string userId = UserIdHelper.GetUserId(Request);
            Result result = await _privateService.DeletePost(userId, postId);
            
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }
}