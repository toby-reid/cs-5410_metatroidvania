using System;
using Godot;

namespace Global
{
    public partial class PlayerState : Node
    {
        public enum Progression : ushort
        {
            MoveLeft = 1 << 0,
            MoveRight = 1 << 1,
            UseLadders = 1 << 2,
            Jump = 1 << 3,
            Swim = 1 << 4,
            JumpOnEnemies = 1 << 5,
            RangedAttack = 1 << 6,
            SaveAndLoad = 1 << 7,
            Shield = 1 << 8,
            CoinCount = 1 << 9,
            OxygenMeter = 1 << 10,
            InGameMenu = 1 << 11,
            LanguageSetting = 1 << 12,
            RedChannel = 1 << 13,
            GreenChannel = 1 << 14,
            BlueChannel = 1 << 15,
            All = ushort.MaxValue
        }

        public static PlayerState Instance { get; private set; }

        // Keeping it at 'private set' because C# does not allow operator overload for enums,
        // so it's easier just to implement our own Add/Reset methods
        public Progression Abilities { get; private set; } = Progression.All;
        public ushort CoinCount
        {
            get;
            set
            {
                if (HasAbility(Progression.CoinCount))
                {
                    field = value;
                    GD.Print("Coin count: " + field);
                }
                else
                {
                    // Change when ready
                    GD.PrintErr("Placeholder: attempted to set coins without coin progression");
                }
            }
        } = 0;

        public override void _Ready()
        {
            Instance = this;
        }

        public bool HasAbility(Progression ability)
        {
            return (Abilities & ability) != 0;
        }

        public void AddAbility(Progression ability)
        {
            if (Enum.IsDefined(ability))
            {
                Abilities |= ability;
            }
            else
            {
                GD.PrintErr("Unrecognized enum value encountered: " + ability);
            }
        }

        public void ResetAbilities()
        {
            Abilities = 0;
        }
    }
}
