using MongoDB.Bson.Serialization.Attributes;

namespace StarboardSocial.PostsService.Data.DTOs;

public class PostEntity
{
    [BsonId]
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string ImageUrl { get; init; }
    public required DateTimeOffset PostedAt { get; init; }
}