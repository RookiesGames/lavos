using Godot;
using Lavos.Audio;
using Lavos.Utils.Lazy;
using System;

namespace Lavos.UI;

public partial class ClickButton : Button
{
    public enum Type
    {
        None,
        Forward,
        Back,
    };

    public event Action OnButtonPressed;
    public event Action<bool> OnButtonToggled;

    [Export] Type ButtonType = Type.None;

    public static AudioStreamOggVorbis AcceptSound;
    public static AudioStreamOggVorbis CancelSound;

    static readonly LazyBuilder<SoundManager> SoundManagerLazy = new(
        () => NodeTree.GetPinnedNodeByType<SoundManager>()
    );

    public override void _ExitTree()
    {
        base._ExitTree();
        OnButtonPressed = null;
        OnButtonToggled = null;
    }

    public override void _Pressed()
    {
        base._Pressed();
        Press();
    }

    public void Press()
    {
        PlayClick();
        OnButtonPressed?.Invoke();
    }

    void PlayClick()
    {
        switch (ButtonType)
        {
            case Type.Forward:
                {
                    if (AcceptSound != null)
                    {
                        SoundManagerLazy.Instance.PlayStream(AcceptSound);
                    }
                    break;
                }
            case Type.Back:
                {
                    if (CancelSound != null)
                    {
                        SoundManagerLazy.Instance.PlayStream(CancelSound);
                    }
                    break;
                }
            default: break;
        }
    }

    public override void _Toggled(bool buttonPressed)
    {
        base._Toggled(buttonPressed);
        OnButtonToggled?.Invoke(buttonPressed);
    }
}