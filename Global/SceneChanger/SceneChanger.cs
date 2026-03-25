using Godot;
using System;

namespace Global
{
    public partial class SceneChanger : Node2D
    {
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
    }
}
