using Godot;

namespace Global
{
    public partial class SceneChanger : Node
    {
        // this holds a UUID for the scene; safe refactoring without immediately loading the scene
        [Export(PropertyHint.File, "*.tscn")] 
        private string _mainMenuPath;
        
        public static SceneChanger Instance { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Instance = this;
        }

        public void ChangeScene(PackedScene scene)
        {
            Callable.From(() => GetTree().ChangeSceneToPacked(scene)).CallDeferred();
        }

        public void ChangeScene(string scenePath)
        {
            Callable.From(() => GetTree().ChangeSceneToFile(scenePath)).CallDeferred();
        }

        public void ReloadScene()
        {
            Callable.From(GetTree().ReloadCurrentScene).CallDeferred();
        }

        public void ExitGame(int exitCode = 0)
        {
            GetTree().Quit(exitCode);
        }

        public void GoToMainMenu() => ChangeScene(_mainMenuPath);
    }
}
