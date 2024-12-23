namespace StarboardSocial.PostsService.Domain.Models;

public class Post
{
    public required Guid Id { get; init; }
    public required string UserId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string ImageUrl { get; init; }
    public required DateTimeOffset PostedAt { get; init; }
}