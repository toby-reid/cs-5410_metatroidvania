using Godot;
using System;

namespace Actors
{
    public interface IPCState
    {
        public void OnChangeToState(Player player) {}
        public void OnChangeFromState(Player player) {}
        public void PhysicsProcess(Player player, double delta);
    }
}
