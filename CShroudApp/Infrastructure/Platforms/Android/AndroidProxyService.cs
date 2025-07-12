using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Platforms.Android;

public class AndroidProxyService : IProxyManager
{
    public ProxyData? CachedProxy { get; set; }
    public ProxyData? GetProxyData()
    {
        //throw new NotImplementedException();
        return null;
    }

    public void SetupProxy(ProxyData proxy, bool savePreviousProxy = true)
    {
        //throw new NotImplementedException();
    }

    public void DisableProxy()
    {
        //throw new NotImplementedException();
    }
}