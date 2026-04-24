using Godot;
using System;

namespace Global
{
    public partial class PlayerState
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

        public bool CompletedTutorial { get; private set; }
        public Progression Progress { get; private set; } = Progression.All;
        public ushort CoinCount
        {
            get => _coinCount;
            set
            {
                if (HasUnlock(Progression.CoinCount))
                {
                    _coinCount = value;
                    OnCoinCountChange?.Invoke(value);
                }
                else
                {
                    SceneChanger.Instance.GoToGameOver("Resource 'coin_count' does not exist");
                }
            }
        }
        public byte HP
        {
            get => _hp;
            set
            {
                if (HasUnlock(Progression.HealthBar))
                {
                    _hp = value;
                    OnHPChange?.Invoke(value);
                }
                else
                {
                    SceneChanger.Instance.GoToGameOver("Resource 'hp' does not exist");
                }
            }
        }

        public string CurrentRoom { get; set; } = "uid://cmdqcfmcrtfh"; // TutorialRoom.tscn
        public string LastUsedDoorwayID { get; set; }

        private const Progression StartingProgress = Progression.MoveLeft;
        private const string StartingRoom = "uid://cubikmkoaafiy"; // CorruptedTutorial00

        private ushort _coinCount = 0;
        private byte _hp = MaxHP;

        // Static constructor: invoked the first time this class is accessed
        static PlayerState()
        {
            Instance = new();
        }

        public bool HasUnlock(Progression unlock) => Progress.HasFlag(unlock);

        public void CompleteTutorial()
        {
            CompletedTutorial = true;
            Progress = StartingProgress;
            CoinCount = 0;
            HP = MaxHP;
            CurrentRoom = StartingRoom;
            LastUsedDoorwayID = null;
            Save();
        }

        public void Unlock(Progression unlock)
        {
            if (!HasUnlock(unlock))
            {
                Progress |= unlock;
                OnProgression?.Invoke(Progress);
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
            using FileAccess file = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Write);
            if (file == null)
            {
                GD.PrintErr("Failed to save game progression");
                return;
            }
            file.Store32((uint)Progress);
            file.Store8(CompletedTutorial ? (byte)1 : (byte)0);
            file.Store16(CoinCount);
            file.Store8(HP);
            file.StorePascalString(CurrentRoom);
            file.StorePascalString(LastUsedDoorwayID ?? "");
            GD.Print("Successfully saved game");
            GD.Print(CurrentRoom);
        }
        public static bool SaveExists()
        {
            return FileAccess.FileExists(SaveFile);
        }
        public static void DeleteSaveFile()
        {
            if (SaveExists())
            {
                DirAccess.RemoveAbsolute(SaveFile);
            }
        }
        public static bool LoadIfExists()
        {
            if (SaveExists())
            {
                return Load();
            }
            RestoreDefaults();
            return true;
        }
        public static bool Load()
        {
            if (SaveExists())
            {
                using FileAccess file = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Read);
                if (file == null)
                {
                    GD.PrintErr("Failed to load game progression");
                    return false;
                }
                PlayerState state = new();
                state.Progress = (Progression)file.Get32();
                state.CompletedTutorial = file.Get8() != 0;
                state._coinCount = file.Get16();
                state._hp = file.Get8();
                state.CurrentRoom = file.GetPascalString();
                string lastDoorway = file.GetPascalString();
                state.LastUsedDoorwayID = (lastDoorway == "") ? null : lastDoorway;
                Instance = state;
                GD.Print("Successfully loaded player state");
                GD.Print(state.CurrentRoom);
                return true;
            }
            GD.Print("No save file found");
            return false;
        }
        public static void RestoreDefaults()
        {
            Instance = new();
        }
    }
}
