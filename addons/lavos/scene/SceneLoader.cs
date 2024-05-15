
using Godot;
using System.Threading.Tasks;
using Lavos.Input;

namespace Lavos.Scene;

public sealed partial class SceneLoader : Node
{
    [Export]
    PackedScene _nextScene;

    [Export]
    double _delay = 0;

    public async Task ChangeScene()
    {
        await Task.Delay((int)(_delay * 1000));
        SceneManager.ChangeScene(_nextScene);
    }
}
