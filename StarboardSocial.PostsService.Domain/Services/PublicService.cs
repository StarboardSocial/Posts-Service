using FluentResults;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.UserService.Domain.DataInterfaces;

namespace StarboardSocial.PostsService.Domain.Services;

public interface IPublicService
{
    Task<Result<List<Post>>> GetFeed(int page, int pageSize);
}

public class PublicService(IPublicRepository publicRepository) : IPublicService
{
    private readonly IPublicRepository _publicRepository = publicRepository;
    
    public async Task<Result<List<Post>>> GetFeed(int page, int pageSize) => await _publicRepository.GetFeed(page, pageSize);
}