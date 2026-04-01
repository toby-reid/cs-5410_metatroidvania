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

        public const string SaveFile = "user://prog.tres";

        [Export]
        public bool CompletedTutorial
        {
            get;
            set
            {
                field = value;
                Save();
            }
        }
        // Don't want to save with every 'set', because we may be performing bulk 'set's
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
                    GD.Print("Coin count: " + field);
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
            Instance = Load();
        }

        public bool HasUnlock(Progression ability) => Progress.HasFlag(ability);
        public bool HasUnlock(ColorChannel channel) => ColorChannels.HasFlag(channel);
        public bool HasUnlock(Soundtrack track) => Soundtracks.HasFlag(track);
        public bool HasUnlock(Animation animation) => Animations.HasFlag(animation);

        public void ResetProgression(bool resetTutorial = false)
        {
            if (resetTutorial)
            {
                CompletedTutorial = false;
            }
            Progress = 0;
            ColorChannels = 0;
            Soundtracks = 0;
            Animations = 0;
            CoinCount = 0;
            Save();
        }

        public void UnlockAll()
        {
            Unlock(Progression.All, ColorChannel.All, Soundtrack.All, Animation.All);
        }
        public void Unlock(Progression ability = 0, ColorChannel channel = 0, Soundtrack track = 0, Animation animation = 0)
        {
            Progress |= ability;
            ColorChannels |= channel;
            Soundtracks |= track;
            Animations |= animation;
            Save();
        }
        #pragma warning disable CA1822  // "Member can be made static" ... no these can't
        public void Unlock(Progression ability) => Unlock(ability: ability);
        public void Unlock(ColorChannel channel) => Unlock(channel: channel);
        public void Unlock(Soundtrack track) => Unlock(track: track);
        public void Unlock(Animation animation) => Unlock(animation: animation);
        #pragma warning restore CA1822

        public void Save()
        {
            Error err = ResourceSaver.Save(this, SaveFile);
            if (err != Error.Ok)
            {
                GD.PrintErr("Failed to save game progression: " + err);
            }
        }
        private static GameProgression Load()
        {
            if (FileAccess.FileExists(SaveFile))
            {
                GameProgression progress = ResourceLoader.Load<GameProgression>(SaveFile, cacheMode: ResourceLoader.CacheMode.Ignore);
                if (progress != null)
                {
                    return progress;
                }
                GD.PrintErr("Failed to load game progression");
            }
            return new();
        }
    }
}
