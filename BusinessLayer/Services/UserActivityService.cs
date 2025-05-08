using DataAccessLayer.Repository;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserActivityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogActivityAsync(string userId, string activityType, string entityName,
            int? entityId, string description, string oldValues = null!, string newValues = null!, string ipAddress = null!)
        {
            var activity = new UserActivity
            {
                UserId = userId,
                ActivityType = activityType,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                OldValues = oldValues,
                NewValues = newValues,
                IpAddress = ipAddress
            };

            await _unitOfWork.UserActivities.AddAsync(activity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserActivity>> GetUserActivitiesAsync(string userId)
        {
            var activities = await _unitOfWork.UserActivities.FindAsync(a => a.UserId == userId);
            return activities.OrderByDescending(a => a.ActivityDate).ToList();
        }

        public async Task<List<UserActivity>> GetAllActivitiesAsync()
        {
            var activities = await _unitOfWork.UserActivities.GetAllAsync();
            return activities.OrderByDescending(a => a.ActivityDate).ToList();
        }

        public async Task<List<UserActivity>> GetActivitiesByTypeAsync(string activityType)
        {
            var activities = await _unitOfWork.UserActivities.FindAsync(a => a.ActivityType == activityType);
            return activities.OrderByDescending(a => a.ActivityDate).ToList();
        }

        public async Task<List<UserActivity>> GetActivitiesByEntityAsync(string entityName)
        {
            var activities = await _unitOfWork.UserActivities.FindAsync(a => a.EntityName == entityName);
            return activities.OrderByDescending(a => a.ActivityDate).ToList();
        }
    }
}
