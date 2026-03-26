using Godot;

namespace Global
{
    public partial class PlayerState : Node
    {
        public enum Movement : byte
        {
            MoveLeft = 1 << 0,
            MoveRight = 1 << 1,
            UseLadders = 1 << 2,
            Jump = 1 << 3,
            Swim = 1 << 4,
            JumpOnEnemies = 1 << 5,
            RangedAttack = 1 << 6
        }
        public enum UIFeature : byte
        {
            HealthBar = 1 << 0,
            CoinCount = 1 << 1,
            OxygenMeter = 1 << 2,
            InGameMenu = 1 << 3,
            LanguageSetting = 1 << 4,
            RedChannel = 1 << 5,
            GreenChannel = 1 << 6,
            BlueChannel = 1 << 7
        }

        public static PlayerState Instance { get; private set; }

        public Movement Movements { get; private set; } = 0;
        public UIFeature UIFeatures { get; private set; } = 0;
        public const byte MaxHP = 3;
        public byte HP { get; private set; } = MaxHP;
        public ushort CoinCount { get; private set; } = 0;

        public override void _Ready()
        {
            Instance = this;
        }

        // TODO: Add methods for game progression, HP modification, & Coin Count
        // If we attempt to modify HP/coin count without those UIFeatures, send to BSOD room
    }
}
