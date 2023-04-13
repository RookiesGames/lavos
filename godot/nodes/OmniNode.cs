using Godot;
using Lavos.Dependency;
using Lavos.Scene;
using Lavos.Utils;

namespace Lavos.Nodes;

public sealed partial class OmniNode : NodeSingleton<OmniNode>
{
    [Export] PackedScene _scene = null;
    [Export] Config[] _configs = null;

    public override void _EnterTree()
    {
        _instance = this;
    }

    public override void _Ready()
    {
        var container = this.AddNode<DependencyContainer>();
        this.AddNode<ServiceLocator>(container);
        this.AddNode<NodeTree>();
        this.AddNode<SceneManager>();
        //
        HandleConfigs(container);
        //
        SceneManager.ChangeScene(_scene);
    }

    void HandleConfigs(DependencyContainer container)
    {
        if (_configs?.Length > 0)
        {
            CreateConfigs(container);
            InitializeConfigs(container);
        }
    }

    void CreateConfigs(DependencyContainer container)
    {
        foreach (var config in _configs)
        {
            config.Configure(container);
        }
    }

    void InitializeConfigs(DependencyContainer container)
    {
        foreach (var config in _configs)
        {
            config.Initialize(container);
        }
    }

    public static void RequestQuit()
    {
        Instance.GetTree().Quit();
    }
}
