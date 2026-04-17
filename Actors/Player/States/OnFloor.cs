using Global;
using Godot;
using System;

using static Global.Constants.InputMap;

namespace Actors
{
    public partial class OnFloorState : IPCState
    {
        public void PhysicsProcess(Player player, double delta)
        {
            if (Input.IsActionJustPressed(Jump) && PlayerState.Instance.HasUnlock(PlayerState.Progression.Jump))
            {

            }
        }
    }
}
