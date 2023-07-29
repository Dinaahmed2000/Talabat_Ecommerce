using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.services
{
    public interface IResponseCacheService
    {
        Task cacheResponseAsync(string cacheKey,object response,TimeSpan timeToLive);
        Task<string> getCahedResponseAsync(string cacheKey);
    }
}
