using Godot;

namespace Lavos.Physics;

public interface ICollisionReceiver
{
    bool IsEnabled { get; }

    void OnCollisionReceived(PhysicsBody2D other);
}