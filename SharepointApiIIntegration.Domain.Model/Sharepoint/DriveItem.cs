using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharepointApiIIntegration.Domain.Model.Sharepoint;

    public class ContentType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class CreatedBy
    {
        public User user { get; set; }
    }

    public class DriveItem
    {
        [JsonProperty("@microsoft.graph.downloadUrl")]
        [JsonPropertyName("@microsoft.graph.downloadUrl")]
        public string MicrosoftGraphDownloadUrl { get; set; }
        public DateTime createdDateTime { get; set; }
        public string eTag { get; set; }
        public string id { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
        public string cTag { get; set; }
        public int size { get; set; }
        public CreatedBy createdBy { get; set; }
        public LastModifiedBy lastModifiedBy { get; set; }
        public ParentReference parentReference { get; set; }
        public File file { get; set; }
        public FileSystemInfo fileSystemInfo { get; set; }
    }

    public class File
    {
        public string mimeType { get; set; }
        public Hashes hashes { get; set; }
    }

    public class FileSystemInfo
    {
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
    }

    public class Hashes
    {
        public string quickXorHash { get; set; }
    }

    public class LastModifiedBy
    {
        public User user { get; set; }
    }

    public class ParentReference
    {
        public string id { get; set; }
        public string siteId { get; set; }
        public string driveType { get; set; }
        public string driveId { get; set; }
        public string path { get; set; }
    }

    public class Root
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        public List<Value> value { get; set; }
    }

    public class User
    {
        public string email { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
    }

    public class Value
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
        public Fields fields { get; set; } 
        [JsonProperty("driveItem@odata.context")]
        public string DriveItemOdataContext { get; set; }
        public DriveItem driveItem { get; set; }
    }

    public class Fields
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }
        public string FileLeafRef { get; set; }
        public string Empreendimento { get; set; }
        public string id { get; set; }
        public string ContentType { get; set; }
        public DateTime Created { get; set; }
        public string AuthorLookupId { get; set; }
        public DateTime Modified { get; set; }
        public string EditorLookupId { get; set; }
        public string _CheckinComment { get; set; }
        public string LinkFilenameNoMenu { get; set; }
        public string LinkFilename { get; set; }
        public string DocIcon { get; set; }
        public string FileSizeDisplay { get; set; }
        public string ItemChildCount { get; set; }
        public string FolderChildCount { get; set; }
        public string _ComplianceFlags { get; set; }
        public string _ComplianceTag { get; set; }
        public string _ComplianceTagWrittenTime { get; set; }
        public string _ComplianceTagUserId { get; set; }
        public string _CommentCount { get; set; }
        public string _LikeCount { get; set; }
        public string _DisplayName { get; set; }
        public string Edit { get; set; }
        public string _UIVersionString { get; set; }
        public string ParentVersionStringLookupId { get; set; }
        public string ParentLeafNameLookupId { get; set; }
    }
