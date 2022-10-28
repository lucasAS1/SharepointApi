using System.Text.Json.Serialization;

namespace SharepointApiIIntegration.Domain.Model.Sharepoint
{
    public class ListItemsResponse
    {
        [JsonPropertyName("d")]
        public RespostaListItemResponse Response { get; set; }
    }

    public class RespostaListItemResponse
    {
        [JsonPropertyName("results")]
        public List<ListItem> ListItems { get; set; }
    }

    public class FileNameAsPath
    {
        [JsonPropertyName("__metadata")]
        public Metadata __metadata { get; set; }

        public string DecodedUrl { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("uri")]
        public string uri { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }
    }

    public class ListItem
    {
        [JsonPropertyName("__metadata")]
        public Metadata __metadata { get; set; }

        public string FileName { get; set; }
        public FileNameAsPath FileNameAsPath { get; set; }
        public ServerRelativePath ServerRelativePath { get; set; }

        [JsonPropertyName("AttachmentFiles")]
        public AttachmentFiles AttachmentFiles { get; set; }

        public string ServerRelativeUrl { get; set; }
    }

    public class ServerRelativePath
    {
        public Metadata __metadata { get; set; }
        public string DecodedUrl { get; set; }
    }

    public class AttachmentFiles
    {
        public List<ListItem> results { get; set; }
    }
}