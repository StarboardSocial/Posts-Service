using FluentResults;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.UserService.Domain.DataInterfaces;

namespace StarboardSocial.PostsService.Domain.Services;

public interface IPrivateService
{
    Task<Result<List<Post>>> GetPosts(string userId);
    Task<Result<Post>> GetPost(string userId, string postId);
    Task<Result> DeletePost(string userId, string postId);
    Task<Result<Post>> CreatePost(Post post);
    Task<Result<Post>> UpdatePost(Post post);
}

public class PrivateService(IPrivateRepository privateRepository) : IPrivateService
{
    private readonly IPrivateRepository _privateRepository = privateRepository;

    public async Task<Result<List<Post>>> GetPosts(string userId) => await _privateRepository.GetPosts(userId);

    public async Task<Result<Post>> GetPost(string userId, string postId) => await _privateRepository.GetPost(userId, postId);

    public async Task<Result> DeletePost(string userId, string postId) => await _privateRepository.DeletePost(userId, postId);

    public async Task<Result<Post>> CreatePost(Post post) => await _privateRepository.CreatePost(post);

    public async Task<Result<Post>> UpdatePost(Post post) => await _privateRepository.UpdatePost(post);
}