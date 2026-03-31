using Godot;
using System;

namespace Global
{
    public partial class GameProgression : Resource
    {
        [Flags]
        public enum Progression : ushort
        {
            MoveRight = 1 << 0,
            MoveLeft = 1 << 1,
            Jump = 1 << 2,
            BaseAttack = 1 << 3,
            HealthBar = 1 << 4,
            CoinCount = 1 << 5,
            UseLadders = 1 << 6,
            DropFromPlatform = 1 << 7,
            EnemiesMove = 1 << 8,
            WaterPhysics = 1 << 9,
            InGameMenu = 1 << 10,
            OxygenMeter = 1 << 11,
            LanguageSetting = 1 << 12,
            GameOptimization = 1 << 13,
            All = ushort.MaxValue
        }

        [Flags]
        public enum ColorChannel : byte
        {
            Red = 1 << 0,
            Green = 1 << 1,
            Blue = 1 << 2,
            All = byte.MaxValue
        }

        [Flags]
        public enum Soundtrack : byte
        {
            // Not implemented.
            // This may end up being a static string class or something similar, depending on future development.
            // It has been included for completion's sake in comparison with collectibles.md.
            All = byte.MaxValue
        }

        [Flags]
        public enum Animation : byte
        {
            // Not implemented.
            // This may end up being a static string class or something similar, depending on future development.
            // It has been included for completion's sake in comparison with collectibles.md.
            All = byte.MaxValue
        }

        public static GameProgression Instance { get; private set; }

        [Export] public Progression Progress { get; private set; } = Progression.All;
        [Export] public ColorChannel ColorChannels { get; private set; } = ColorChannel.All;
        [Export] public Soundtrack Soundtracks { get; private set; } = Soundtrack.All;
        [Export] public Animation Animations { get; private set; } = Animation.All;
        [Export]
        public ushort CoinCount
        {
            get;
            set
            {
                if (HasUnlock(Progression.CoinCount))
                {
                    field = value;
                }
                else
                {
                    // TODO: Crash the game
                    GD.PrintErr("Placeholder: attempted to set coins without indicator");
                }
            }
        }

        // Static constructor: invoked the first time this class is accessed
        static GameProgression()
        {
            // If the default value is different from All, change this to a *.tres resource load
            Instance = new();
        }

        public bool HasUnlock(Progression ability) => Progress.HasFlag(ability);
        public bool HasUnlock(ColorChannel channel) => ColorChannels.HasFlag(channel);
        public bool HasUnlock(Soundtrack track) => Soundtracks.HasFlag(track);
        public bool HasUnlock(Animation animation) => Animations.HasFlag(animation);

        public void Unlock(Progression ability)
        {
            if (Enum.IsDefined(ability))
            {
                Progress |= ability;
            }
            else
            {
                GD.PrintErr("Unrecognized progression: " + ability);
            }
        }
        public void Unlock(ColorChannel channel)
        {
            if (Enum.IsDefined(channel))
            {
                ColorChannels |= channel;
            }
            else
            {
                GD.PrintErr("Unrecognized channel: " + channel);
            }
        }
        public void Unlock(Soundtrack track)
        {
            if (Enum.IsDefined(track))
            {
                Soundtracks |= track;
            }
            else
            {
                GD.PrintErr("Unrecognized soundtrack: " + track);
            }
        }
        public void Unlock(Animation animation)
        {
            if (Enum.IsDefined(animation))
            {
                Animations |= animation;
            }
            else
            {
                GD.PrintErr("Unrecognized animation: " + animation);
            }
        }
    }
}
