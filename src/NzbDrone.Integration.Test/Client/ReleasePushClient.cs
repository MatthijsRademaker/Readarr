using Lidarr.Api.V3.Indexers;
using RestSharp;

namespace NzbDrone.Integration.Test.Client
{
    public class ReleasePushClient : ClientBase<ReleaseResource>
    {
        public ReleasePushClient(IRestClient restClient, string apiKey)
            : base(restClient, apiKey, "release/push")
        {
        }
    }
}