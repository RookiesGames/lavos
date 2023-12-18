
namespace Lavos.Social;

public sealed class PlayerProfile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public LevelInfo LevelInfo { get; set; }
    public FriendStatus FriendStatus { get; }
    public FriendsListVisibility friendsListVisibilityStatus { get; }
}