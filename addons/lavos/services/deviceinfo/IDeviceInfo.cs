using Lavos.Dependency;

public interface IDeviceInfo : IService
{
    string Platform { get; }
    string OSVersion { get; }
    string DeviceName { get; }
    string VersionName { get; }
    string VersionCode { get; }
}