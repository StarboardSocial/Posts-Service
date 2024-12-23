namespace StarboardSocial.PostsService.Server.ViewModels;

public class PostEditViewModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string ImageUrl { get; init; }
    public required DateTimeOffset PostedAt { get; init; }
}