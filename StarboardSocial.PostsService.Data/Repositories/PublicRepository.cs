using FluentResults;
using MongoDB.Bson;
using MongoDB.Driver;
using StarboardSocial.PostsService.Data.DTOs;
using StarboardSocial.PostsService.Data.Mappers;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.UserService.Domain.DataInterfaces;

namespace StarboardSocial.UserService.Data.Repositories;

public class PublicRepository(IMongoDatabase mongoDatabase) : IPublicRepository
{
    private readonly IMongoCollection<PostEntity> _postsCollection = mongoDatabase.GetCollection<PostEntity>("posts");

    

    public async Task<Result<List<Post>>> GetFeed(int page, int pageSize)
    {
        SortDefinition<PostEntity> sort = Builders<PostEntity>.Sort.Descending("PostedAt");
        List<PostEntity> postEntities = await _postsCollection
            .Find(FilterDefinition<PostEntity>.Empty)
            .Sort(sort)
            .Skip(page * pageSize)
            .Limit(pageSize)
            .ToListAsync();
        List<Post> posts = postEntities.Select(postEntity => postEntity.ToPost()).ToList();
        return Result.Ok(posts);
    }
}