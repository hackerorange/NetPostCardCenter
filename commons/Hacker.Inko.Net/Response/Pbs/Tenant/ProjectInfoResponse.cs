using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.Pbs.Tenant
{
    public class ProjectInfoResponse
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("workspaceId")] public string WorkspaceId { get; set; }
        [JsonProperty("province")] public string Province { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("district")] public string District { get; set; }
        [JsonProperty("orgId")] public string OrgId { get; set; }
        [JsonProperty("tenantId")] public string TenantId { get; set; }
    }
}