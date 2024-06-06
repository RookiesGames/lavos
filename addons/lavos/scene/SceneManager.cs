using Godot;
using System.Threading.Tasks;

namespace Lavos.Scene;

public static class SceneManager
{
    #region Methods

    public static async Task<PackedScene> LoadSceneAsync(string path)
    {
        var scene = await Task.Run(() => GD.Load<PackedScene>(path));
        return scene;
    }

    public static PackedScene LoadScene(string path)
    {
        var scene = GD.Load<PackedScene>(path);
        return scene;
    }

    public static async Task ChangeSceneAsync(string path)
    {
        var scene = await LoadSceneAsync(path);
        ChangeScene(scene);
    }

    public static void ChangeScene(string path)
    {
        var scene = LoadScene(path);
        ChangeScene(scene);
    }

    public static void ChangeScene(PackedScene scene)
    {
        NodeTree.CleanTree();
        AddScene(scene);
    }

    public static Node AddScene(PackedScene scene)
    {
        return AddSceneToParent(scene, NodeTree.Instance);
    }

    public static Node AddSceneToParent(PackedScene scene, Node parent)
    {
        Assert.IsTrue(scene != null, $"Scene {scene.ResourceName} is nil");
        var node = parent.AddNode<Node>(scene);
        return node;
    }

    public static T AddSceneToParent<T>(PackedScene scene, Node parent) where T : Node
    {
        return (T)AddSceneToParent(scene, parent);
    }

    #endregion Methods
}