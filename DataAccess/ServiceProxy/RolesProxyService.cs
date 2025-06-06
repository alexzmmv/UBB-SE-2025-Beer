using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;

namespace DataAccess.ServiceProxy
{
    public class RolesProxyService : IRolesService
    {
        private const string API_ROUTE = "api/roles";
        private readonly HttpClient httpClient;

        public RolesProxyService(string baseApiUrl)
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(baseApiUrl);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(API_ROUTE);
            response.EnsureSuccessStatusCode();
            List<Role> roles = await response.Content.ReadFromJsonAsync<List<Role>>() ?? new List<Role>();
            return roles;
        }

        public async Task<Role?> GetNextRoleInHierarchyAsync(RoleType currentRoleType)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(API_ROUTE);
            response.EnsureSuccessStatusCode();
            List<Role> roles = await response.Content.ReadFromJsonAsync<List<Role>>() ?? new List<Role>();

            if (currentRoleType.Equals(RoleType.Admin))
            {
                return roles.Find(role => role.RoleType == currentRoleType);
            }

            return roles.Find(role => role.RoleType > currentRoleType);
        }
    }
}