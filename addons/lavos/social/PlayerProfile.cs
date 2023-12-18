
namespace Lavos.Social;

public sealed class PlayerProfile
{
    public string Id { get; }
    public string Name { get; }
    public string Title { get; }
    public LevelInfo LevelInfo { get; }
    public FriendStatus FriendStatus { get; }
    public FriendsListVisibility FriendsListVisibility { get; }
}