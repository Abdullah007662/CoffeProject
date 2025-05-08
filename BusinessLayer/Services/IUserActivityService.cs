using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IUserActivityService
    {
        Task LogActivityAsync(string userId, string activityType, string entityName,
            int? entityId, string description, string oldValues = null!, string newValues = null!, string ipAddress = null!);
        Task<List<UserActivity>> GetUserActivitiesAsync(string userId);
        Task<List<UserActivity>> GetAllActivitiesAsync();
        Task<List<UserActivity>> GetActivitiesByTypeAsync(string activityType);
        Task<List<UserActivity>> GetActivitiesByEntityAsync(string entityName);
    }
}
