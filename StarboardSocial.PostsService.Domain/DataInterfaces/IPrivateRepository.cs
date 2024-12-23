using FluentResults;
using StarboardSocial.PostsService.Domain.Models;

namespace StarboardSocial.UserService.Domain.DataInterfaces;

public interface IPrivateRepository
{
    Task<Result<List<Post>>> GetPosts(string userId);
    Task<Result<Post>> GetPost(string userId, string postId);
    Task<Result> DeletePost(string userId, string postId);
    Task<Result<Post>> CreatePost(Post post);
    Task<Result<Post>> UpdatePost(Post post);
}