using Lavos.Dependency;
using System;
using System.Threading.Tasks;

namespace Lavos.Social.Auth;

public interface ISocialAuth : IService
{
    Task<bool> SignIn();
    Task<bool> SignOut();
    bool IsSignedIn();
}