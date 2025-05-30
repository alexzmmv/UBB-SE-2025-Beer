using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;

namespace DrinkDb_Auth.ServiceProxy
{
    public class UserServiceProxy : IUserService
    {
        private const string ApiRoute = "api/users";
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public UserServiceProxy(string baseApiUrl)
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(baseApiUrl);
        }

        public async Task ChangeRoleToUser(Guid userId, Role roleToAdd)
        {
            string userUrl = $"{ApiRoute}/byId/{userId}/addRole";
            HttpResponseMessage response = await this.httpClient.PatchAsJsonAsync(userUrl, roleToAdd);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> CreateUser(User user)
        {
            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"{ApiRoute}/add", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<List<User>> GetAllUsers()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(ApiRoute);
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<List<User>> GetBannedUsersWhoHaveSubmittedAppeals()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/banned/appealed");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<RoleType> GetRoleTypeForUser(Guid userId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/byId/{userId}/role");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RoleType>(jsonOptions);
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/byId/{userId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>(jsonOptions);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/byUserName/{username}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>(jsonOptions);
        }

        public async Task<List<User>> GetUsersByRoleType(RoleType roleType)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/byRole/{roleType}");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<List<User>> GetUsersWhoHaveSubmittedAppeals()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/appealed");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<bool> UpdateUser(User user)
        {
            HttpResponseMessage response = await this.httpClient.PatchAsJsonAsync($"{ApiRoute}/{user.UserId}/updateUser", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<bool> ValidateAction(Guid userId, string resource, string action)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/validateAction?userID={userId}&resource={resource}&action={action}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<List<User>> GetActiveUsersByRoleType(RoleType roleType)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/active/byRole/{roleType}");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<List<User>> GetBannedUsers()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/banned");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<List<User>> GetAdminUsers()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/admins");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<List<User>> GetManagers()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/managers");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<List<User>> GetRegularUsers()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/regular");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }

        public async Task<RoleType?> GetHighestRoleTypeForUser(Guid id)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/byId/{id}/role");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RoleType>(jsonOptions);
        }

        public async Task UpdateUserRole(Guid userId, RoleType roleType)
        {
            string url = $"{ApiRoute}/byId/{userId}/addRole";
            Role role = new Role { RoleType = roleType };
            HttpResponseMessage response = await this.httpClient.PatchAsJsonAsync(url, role);
            return;
        }

        public async Task<string?> GetUserFullNameById(Guid userId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/byId/{userId}/fullName");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<string>(jsonOptions);
        }

        public async Task UpdateUserAppleaed(User user, bool newValue)
        {
                HttpResponseMessage response = await this.httpClient.PatchAsJsonAsync($"{ApiRoute}/byId/{user.UserId}/appealed", newValue);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return;
                }
                response.EnsureSuccessStatusCode();
        }

        public async Task<List<User>> GetUsersWithHiddenReviews()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{ApiRoute}/hidden-reviews");
            response.EnsureSuccessStatusCode();
            List<User>? users = await response.Content.ReadFromJsonAsync<List<User>>(jsonOptions);
            return users ?? new List<User>();
        }
    }
}