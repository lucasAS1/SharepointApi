using System.Net.Mime;
using Newtonsoft.Json;

namespace SharepointApiIIntegration.Domain.Model.Sharepoint;

public class ItemValue
{
    [JsonProperty("@odata.etag")]
    public string OdataEtag { get; set; }
    public DateTime createdDateTime { get; set; }
    public string eTag { get; set; }
    public string id { get; set; }
    public DateTime lastModifiedDateTime { get; set; }
    public string webUrl { get; set; }
    public CreatedBy createdBy { get; set; }
    public LastModifiedBy lastModifiedBy { get; set; }
    public ParentReference parentReference { get; set; }
    public ContentType contentType { get; set; }
    
    public class User
    {
        public string email { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
    }
    public class ContentType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class CreatedBy
    {
        public User user { get; set; }
    }

    public class LastModifiedBy
    {
        public User user { get; set; }
    }

    public class ParentReference
    {
        public string id { get; set; }
        public string siteId { get; set; }
    }
}
