using FluentResults;
using MongoDB.Driver;
using StarboardSocial.PostsService.Data.DTOs;
using StarboardSocial.PostsService.Data.Mappers;
using StarboardSocial.PostsService.Domain.Models;
using StarboardSocial.UserService.Domain.DataInterfaces;

namespace StarboardSocial.UserService.Data.Repositories;

public class PrivateRepository(IMongoDatabase mongoDatabase) : IPrivateRepository
{
    private readonly IMongoCollection<PostEntity> _postsCollection = mongoDatabase.GetCollection<PostEntity>("posts");

    public async Task<Result<List<Post>>> GetPosts(string userId)
    {
        SortDefinition<PostEntity> sort = Builders<PostEntity>.Sort.Descending("PostedAt");
        List<PostEntity> postEntities = await _postsCollection.Find(post => post.UserId == userId).Sort(sort).ToListAsync();
        List<Post> posts = postEntities.Select(postEntity => postEntity.ToPost()).ToList();
        return Result.Ok(posts);
    }

    public async  Task<Result<Post>> GetPost(string userId, string postId)
    {
        PostEntity postEntity = await _postsCollection.Find(post => post.UserId == userId && post.Id == postId).FirstOrDefaultAsync();
        if (postEntity == null)
        {
            return Result.Fail<Post>($"Post with id {postId} not found for user {userId}");
        }

        return Result.Ok(postEntity.ToPost());
    }

    public async  Task<Result> DeletePost(string userId, string postId)
    {
        DeleteResult result = await _postsCollection.DeleteOneAsync(post => post.UserId == userId && post.Id == postId);
        if (!result.IsAcknowledged)
        {
            return Result.Fail($"Failed to delete post with id {postId} for user {userId}");
        }

        return Result.Ok();
    }

    public async  Task<Result<Post>> CreatePost(Post post)
    {
        PostEntity postEntity = post.ToPostEntity();
        await _postsCollection.InsertOneAsync(postEntity);
        return Result.Ok(postEntity.ToPost());
    }

    public async  Task<Result<Post>> UpdatePost(Post post)
    {
        PostEntity postEntity = post.ToPostEntity();
        ReplaceOneResult result = await _postsCollection.ReplaceOneAsync(p => p.Id == postEntity.Id && p.UserId == postEntity.UserId, postEntity);
        if (!result.IsAcknowledged)
        {
            return Result.Fail<Post>($"Failed to update post with id {post.Id}");
        }

        return Result.Ok(postEntity.ToPost());
    }
}