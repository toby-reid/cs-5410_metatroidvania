using Godot;

namespace Global
{
    public partial class SceneChanger : Node
    {
        // this holds a UUID for the scene; safe refactoring without immediately loading the scene
        [Export(PropertyHint.File, "*.tscn")] 
        private string _mainMenuPath;
        [Export(PropertyHint.File, "*.tscn")]
        private string _gameOverPath;

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

        public void ChangeScene(Node scene)
        {
            // Loading an initialized scene is a bit more complicated
            Node old = GetTree().CurrentScene;
            GetTree().Root.AddChild(scene);
            GetTree().CurrentScene = scene;
            old.QueueFree();
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

        public void GoToGameOver(string error = null)
        {
            GameOverScreen gameOverScene = GD.Load<PackedScene>(_gameOverPath)
                .Instantiate<GameOverScreen>();
            gameOverScene.Init(error);
            ChangeScene(gameOverScene);
        }
    }
}
