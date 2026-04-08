using Godot;
using System;

namespace Global
{
    public partial class PlayerState : Resource
    {
        // may change to ulong with soundtracks, but more likely those will just be separate
        [Flags]
        public enum Progression : uint
        {
            // Player movement
            MoveRight = 1 << 0,
            MoveLeft = 1 << 1,
            Jump = 1 << 2,
            DropFromPlatform = 1 << 3,
            UseLadders = 1 << 4,
            WaterPhysics = 1 << 5,
            BaseAttack = 1 << 6,

            // UI elements
            HealthBar = 1 << 7,
            CoinCount = 1 << 8,
            OxygenMeter = 1 << 9,
            InGameMenu = 1 << 10,
            LanguageSetting = 1 << 11,
            // Color channels
            RedChannel = 1 << 12,
            GreenChannel = 1 << 13,
            BlueChannel = 1 << 14,

            // Miscellaneous gameplay features
            EnemiesMove = 1 << 15,
            FrameRate = 1 << 16,

            // Soundtracks - not yet implemented.
            // These may end up being represented as a static string class or something similar.
            // We may also just use a placeholder value, Soundtrack = 1 << XX, with another value to represent the actual resource.

            // Animations - not yet implemented.
            // These may end up being represented as a static string class or something similar.
            // We may also just use a placeholder value, Animation = 1 << XX, with another value to represent the actual resource.

            All = uint.MaxValue
        }

        public static PlayerState Instance { get; private set; }

        public const string SaveFile = "user://prog.tres";
        public const byte MaxHP = 2;

        public event Action<Progression> OnProgression;
        public event Action<byte> OnHPChange;
        public event Action<ushort> OnCoinCountChange;

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
        [Export] public Progression Progress { get; private set; } = Progression.All;
        [Export]
        public ushort CoinCount
        {
            get;
            set
            {
                if (HasUnlock(Progression.CoinCount))
                {
                    field = value;
                    OnCoinCountChange(field);
                }
                else
                {
                    // TODO: Crash the game
                    GD.PrintErr("Placeholder: attempted to set coins without indicator");
                }
            }
        }
        [Export]
        public byte HP
        {
            get;
            set
            {
                if (HasUnlock(Progression.HealthBar))
                {
                    field = value;
                    OnHPChange(field);
                }
                else
                {
                    // TODO: Crash the game
                    GD.PrintErr("Placeholder: attempted to set HP without indicator");
                }
            }
        } = MaxHP;

        // Static constructor: invoked the first time this class is accessed
        static PlayerState()
        {
            Instance = new();
        }

        public bool HasUnlock(Progression unlock) => Progress.HasFlag(unlock);

        public void Reset(bool resetTutorial = false)
        {
            if (resetTutorial)
            {
                CompletedTutorial = false;
            }
            Progress = 0;
            CoinCount = 0;
            HP = MaxHP;
            Save();
        }

        public void Unlock(Progression unlock)
        {
            if (!HasUnlock(unlock))
            {
                Progress |= unlock;
                OnProgression(Progress);
                Save();
            }
            else
            {
                GD.PrintErr("Attempted to re-unlock ability/ies: " + unlock);
            }
        }
        public void UnlockAll() => Unlock(Progression.All);

        public void Save()
        {
            Error err = ResourceSaver.Save(this, SaveFile);
            if (err != Error.Ok)
            {
                GD.PrintErr("Failed to save game progression: " + err);
            }
        }
        public static bool Load()
        {
            if (FileAccess.FileExists(SaveFile))
            {
                PlayerState state = ResourceLoader.Load<PlayerState>(SaveFile, cacheMode: ResourceLoader.CacheMode.Ignore);
                if (state != null)
                {
                    GD.Print("Successfully loaded player state: " + state);
                    Instance = state;
                    return true;
                }
                GD.PrintErr("Failed to load game progression");
            }
            else
            {
                GD.Print("No save file found");
            }
            return false;
        }
    }
}
