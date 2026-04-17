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
            DropThroughPlatform = 1 << 3,
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
                    SceneChanger.Instance.GoToGameOver("Resource 'coin_count' does not exist");
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
                    SceneChanger.Instance.GoToGameOver("Resource 'hp' does not exist");
                }
            }
        } = MaxHP;

        [ExportGroup("Tutorial room")]
        [Export(PropertyHint.FilePath, "TutorialRoom.tscn")] public string CurrentRoom { get; set; }
        [Export] public string LastUsedDoorwayID { get; set; }

        [ExportGroup("Starting room")]
        [Export(PropertyHint.FilePath, "StartingRoom.tscn")] private string _startingRoom;

        // Static constructor: invoked the first time this class is accessed
        static PlayerState()
        {
            Instance = new();
        }

        public bool HasUnlock(Progression unlock) => Progress.HasFlag(unlock);

        public void CompleteTutorial()
        {
            CompletedTutorial = true;
            Progress = 0;
            CoinCount = 0;
            HP = MaxHP;
            CurrentRoom = _startingRoom;
            LastUsedDoorwayID = null;
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
        public static void RestoreDefaults()
        {
            Instance = new();
        }
    }
}
