using Godot;
using System;

namespace Lavos.Physics
{
    public interface ICollisionReceiver
    {
        void OnCollisionReceived(PhysicsBody2D other);
    }
}