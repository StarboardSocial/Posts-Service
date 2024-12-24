namespace StarboardSocial.PostsService.Server.ViewModels;

public class PostCreateViewModel
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string ImageBase64 { get; init; }
    public required string ImageExtension { get; init; }
}