using Godot;
using System;

namespace Lavos.Physics
{
    public interface ICollisionReceiver
    {
        void CollisionReceived(PhysicsBody2D other);
    }
}