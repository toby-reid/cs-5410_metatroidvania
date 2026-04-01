using Godot;
using System;

namespace Global
{
    public partial class GameProgression : Resource
    {
        [Flags]
        public enum Ability : byte
        {
            MoveRight = 1 << 0,
            MoveLeft = 1 << 1,
            Jump = 1 << 2,
            DropFromPlatform = 1 << 3,
            UseLadders = 1 << 4,
            WaterPhysics = 1 << 5,
            BaseAttack = 1 << 6,
            All = byte.MaxValue
        }

        [Flags]
        public enum UI : byte
        {
            HealthBar = 1 << 0,
            CoinCount = 1 << 1,
            OxygenMeter = 1 << 2,
            InGameMenu = 1 << 3,
            LanguageSetting = 1 << 4,
            RedChannel = 1 << 5,
            GreenChannel = 1 << 6,
            BlueChannel = 1 << 7,
            All = byte.MaxValue
        }

        [Flags]
        public enum Gameplay : byte
        {
            EnemiesMove = 1 << 0,
            FrameRate = 1 << 1,
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
        [Export] public Ability PlayerAbilities { get; private set; } = Ability.All;
        [Export] public UI UIFeatures { get; private set; } = UI.All;
        [Export] public Gameplay GameplayFeatures { get; private set; } = Gameplay.All;
        [Export] public Soundtrack Soundtracks { get; private set; } = Soundtrack.All;
        [Export] public Animation Animations { get; private set; } = Animation.All;
        [Export]
        public ushort CoinCount
        {
            get;
            set
            {
                if (HasUnlock(UI.CoinCount))
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

        public bool HasUnlock(Ability ability) => PlayerAbilities.HasFlag(ability);
        public bool HasUnlock(UI uiFeature) => UIFeatures.HasFlag(uiFeature);
        public bool HasUnlock(Gameplay gameplayFeature) => GameplayFeatures.HasFlag(gameplayFeature);
        public bool HasUnlock(Soundtrack track) => Soundtracks.HasFlag(track);
        public bool HasUnlock(Animation animation) => Animations.HasFlag(animation);

        public void Reset(bool resetTutorial = false)
        {
            if (resetTutorial)
            {
                CompletedTutorial = false;
            }
            PlayerAbilities = 0;
            UIFeatures = 0;
            GameplayFeatures = 0;
            Soundtracks = 0;
            Animations = 0;
            CoinCount = 0;
            Save();
        }

        public void Unlock(
            Ability abilities = 0,
            UI uiFeatures = 0,
            Gameplay gameplayFeatures = 0,
            Soundtrack tracks = 0,
            Animation animations = 0
        )
        {
            PlayerAbilities |= abilities;
            UIFeatures |= uiFeatures;
            GameplayFeatures |= gameplayFeatures;
            Soundtracks |= tracks;
            Animations |= animations;
            Save();
        }
        public void Unlock(Ability ability) => Unlock(abilities: ability);
        public void Unlock(UI uiFeature) => Unlock(uiFeatures: uiFeature);
        public void Unlock(Gameplay gameplayFeature) => Unlock(gameplayFeatures: gameplayFeature);
        public void Unlock(Soundtrack track) => Unlock(tracks: track);
        public void Unlock(Animation animation) => Unlock(animations: animation);
        public void UnlockAll() => Unlock(Ability.All, UI.All, Gameplay.All, Soundtrack.All, Animation.All);

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
