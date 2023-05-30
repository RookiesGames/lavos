using Godot;
using System.Threading.Tasks;

namespace Lavos.Scene;

public sealed partial class SceneManager : Node
{
    #region Methods

    public static async Task<PackedScene> LoadScene(string path)
    {
        var scene = await Task.Run<PackedScene>(() => GD.Load<PackedScene>(path));
        return scene;
    }

    public static async Task ChangeSceneAsync(string path)
    {
        var scene = await LoadScene(path);
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
        var node = scene.Instantiate();
        parent.AddChild(node);
        return node;
    }

    public static T AddSceneToParent<T>(PackedScene scene, Node parent) where T : Node
    {
        return (T)AddSceneToParent(scene, parent);
    }

    #endregion Methods
}