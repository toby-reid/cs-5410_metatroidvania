using Godot;
using System;

namespace Environment
{
    public partial class OneWayPlatform : StaticBody2D
    {
        [ExportGroup("Set in Object")]
        [Export(PropertyHint.File)] private string _platformLeftSpritePath;
        [Export(PropertyHint.File)] private string _platformCenterSpritePath;
        [Export(PropertyHint.File)] private string _platformRightSpritePath;
        [Export] private Sprite2D _placeholderSprite;
        [Export] private CollisionShape2D _collision;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (Scale.Y != 1)
            {
                GD.PrintErr("Platform Y-scale is unexpected value: " + Scale);
            }
            if (Scale.X <= 0 || Scale.X > byte.MaxValue)
            {
                GD.PrintErr("Platform X-scale is out of expected bounds: " + Scale);
            }
            byte tileCount = (byte)Scale.X;
            if (tileCount == 1)
            {
                // Just keep the placeholder "center" sprite
                return;
            }
            _placeholderSprite.QueueFree();
            _placeholderSprite = null;
            int startPos = 0;
            startPos += CreateSprite(new(), _platformLeftSpritePath);
            for (ushort i = 1; i < tileCount - 1; ++i)
            {
                startPos += CreateSprite(new(startPos, 0), _platformCenterSpritePath);
            }
            CreateSprite(new(startPos, 0), _platformRightSpritePath);
        }

        private int CreateSprite(Vector2 position, string spritePath)
        {
            Sprite2D sprite = new();
            sprite.Texture = ResourceLoader.Load<Texture2D>(spritePath);
            sprite.Centered = false;
            sprite.Position = position;
            sprite.Scale = new(1 / Scale.X, 1);
            AddChild(sprite);
            return sprite.Texture.GetWidth() / (int)Scale.X;
        }
    }
}
