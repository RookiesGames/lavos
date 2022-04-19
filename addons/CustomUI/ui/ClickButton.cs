using Godot;
using Lavos.Audio;
using Lavos.Scene;
using Lavos.Utils.Lazy;
using System;

namespace Lavos.UI
{
    public class ClickButton : Button
    {
        public enum Type
        {
            None,
            Forward,
            Back,
        };

        public event Action ButtonPressed = null;

        [Export] Type ButtonType = Type.None;

        public static AudioStreamOGGVorbis AcceptSound = null;
        public static AudioStreamOGGVorbis CancelSound = null;

        static LazyBuilder<SoundManager> SoundManagerRef = new LazyBuilder<SoundManager>(
            () => NodeTree.GetPinnedNode<SoundManager>()
        );


        public override void _EnterTree()
        {
            base._EnterTree();
            ButtonPressed = null;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            ButtonPressed = null;
        }

        public override void _Pressed()
        {
            base._Pressed();
            Press();
        }

        public void Press()
        {
            PlayClick();
            ButtonPressed?.Invoke();
        }

        void PlayClick()
        {
            switch (ButtonType)
            {
                case Type.Forward: SoundManagerRef.Get.PlayStream(AcceptSound); break;
                case Type.Back: SoundManagerRef.Get.PlayStream(CancelSound); break;
                default: break;
            }
        }
    }
}