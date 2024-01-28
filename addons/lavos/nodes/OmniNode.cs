using Godot;
using Godot.Collections;
using Lavos.Dependency;
using Lavos.Utils;

namespace Lavos.Nodes;

public sealed partial class OmniNode : NodeSingleton<OmniNode>
{
    [Export] PackedScene _scene;
    [Export] Array<Config> _configs;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

    public override void _Ready()
    {
        var container = this.AddNode<DependencyContainer>();
        this.AddNode<ServiceLocator>(container);
        this.AddNode<NodeTree>();
        //
        HandleConfigs(container);
        //
        SceneManager.ChangeScene(_scene);
    }

    void HandleConfigs(DependencyContainer container)
    {
        if (_configs?.Count > 0)
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
