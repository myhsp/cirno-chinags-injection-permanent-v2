namespace CirnoFramework.Interface.Commands
{
    public interface IPackageInfo
    {
        string PackageName { get; set; }
        string PackageVersion { get; set; }

        string GetPackageInfo();
    }
}
