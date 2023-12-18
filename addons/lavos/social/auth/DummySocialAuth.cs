using System;
using System.Threading.Tasks;

namespace Lavos.Social.Auth;

sealed class DummySocialAuth : ISocialAuth
{
    public Task<bool> SignIn() => Task.FromResult(false);
    public Task<bool> SignOut() => Task.FromResult(false);
    public bool IsSignedIn() => false;
}