using Godot;
using Lavos.Utils.Automation;
using Lavos.Utils.Extensions;
using System.Threading.Tasks;

namespace Lavos.UI
{
    public sealed class FadePanel : Sprite
    {
        public enum PanelState
        {
            FadingIn,
            FadedIn,
            FadingOut,
            FadedOut,
        }

        [Export] bool Faded = false;
        [Export] float FadeInDuration = 1;
        [Export] float FadeOutDuration = 1;

        readonly StateMachine _stateMachine = new StateMachine();
        readonly State _idleState = new State();
        readonly State _fadeInState = new State();
        readonly State _fadeOutState = new State();

        float _timer = 0;

        PanelState _currentState;
        public PanelState State => _currentState;


        public override void _Ready()
        {
            this.SetAlpha(Faded ? 1f : 0f);
            SetupStates();
            _stateMachine.ChangeState(_idleState);
            _currentState = Faded ? PanelState.FadedOut : PanelState.FadedIn;
        }

        void SetupStates()
        {
            _fadeInState.Enter = () =>
            {
                _timer = 0;
                _currentState = PanelState.FadingIn;
                this.SetAlpha(1);
            };
            _fadeInState.Process = (dt) =>
            {
                _timer += dt;
                var alpha = Mathf.Lerp(1, 0, _timer / FadeInDuration);
                this.SetAlpha(alpha);
                //
                if (alpha <= 0)
                {
                    _stateMachine.ChangeState(_idleState);
                }
            };
            _fadeInState.Exit = () =>
            {
                _currentState = PanelState.FadedIn;
            };
            //
            _fadeOutState.Enter = () =>
            {
                _timer = 0;
                _currentState = PanelState.FadingOut;
                this.SetAlpha(0);
            };
            _fadeOutState.Process = (dt) =>
            {
                _timer += dt;
                var alpha = Mathf.Lerp(0, 1, _timer / FadeOutDuration);
                this.SetAlpha(alpha);
                //
                if (alpha >= 1)
                {
                    _stateMachine.ChangeState(_idleState);
                }
            };
            _fadeOutState.Exit = () =>
            {
                _currentState = PanelState.FadedOut;
            };
        }

        public override void _Process(float delta)
        {
            _stateMachine.Process(delta);
        }

        public Task FadeIn()
        {
            _stateMachine.ChangeState(_fadeInState);
            return Task.Run(async () =>
            {
                System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(60);
                while (State != PanelState.FadedIn)
                {
                    await Task.Delay(timeSpan);
                }
                return Task.CompletedTask;
            });
        }

        public Task FadeOut()
        {
            _stateMachine.ChangeState(_fadeOutState);
            return Task.Run(async () =>
            {
                System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(60);
                while (State != PanelState.FadedOut)
                {
                    await Task.Delay(timeSpan);
                }
                return Task.CompletedTask;
            });
        }
    }
}