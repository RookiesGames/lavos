using Lavos.Core;
using Lavos.Services.SocialPlatform;
using System;
using System.Threading.Tasks;

namespace Lavos.Social.Auth;

sealed class SocialAuth : ISocialAuth
{
    LazyRef<ISocialPlatformService> _service = new();

    public async Task<bool> SignIn()
    {
        _service.Ref.SignIn();
        var attempts = 3;
        while (attempts > 0)
        {
            if (IsSignedIn()) break;
            //
            --attempts;
            await Task.Delay(1000);
        }
        return IsSignedIn();
    }

    public async Task<bool> SignOut()
    {
        _service.Ref.SignOut();
        var attempts = 3;
        while (attempts > 0)
        {
            if (!IsSignedIn()) break;
            //
            --attempts;
            await Task.Delay(1000);
        }
        return !IsSignedIn();
    }

    public bool IsSignedIn() => _service.Ref.IsSignedIn();
}