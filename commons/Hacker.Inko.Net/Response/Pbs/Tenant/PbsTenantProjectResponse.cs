using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.Pbs.Tenant
{
    public class PbsTenantProjectResponse
    {
        [JsonProperty("tenant")] public TenantInfo Tenant;
        [JsonProperty("projects")] public List<ProjectInfo> ProjectList;
    }

    public class TenantInfo
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("current")] public string Current { get; set; }
    }

    public class ProjectInfo
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("orgId")] public string OrgId { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
}