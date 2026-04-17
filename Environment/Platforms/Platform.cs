using Godot;
using System;

namespace Environment
{
    public partial class Platform : StaticBody2D
    {
        [Export]
        public bool IsOneWay
        {
            get;
            set
            {
                field = value;
                _collision.OneWayCollision = value;
            }
        }

        [ExportGroup("Set in object")]
        [Export]
        private CollisionShape2D _collision;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _collision.OneWayCollision = IsOneWay;
        }
    }
}
