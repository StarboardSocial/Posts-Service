using FluentResults;
using StarboardSocial.PostsService.Domain.Models;

namespace StarboardSocial.UserService.Domain.DataInterfaces;

public interface IPublicRepository
{
    Task<Result<List<Post>>> GetFeed(int page, int pageSize);
}