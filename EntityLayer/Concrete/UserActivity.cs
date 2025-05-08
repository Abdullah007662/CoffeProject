using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class UserActivity
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? ActivityType { get; set; } // Login, Logout, Create, Update, Delete
        public string? EntityName { get; set; } // Hangi entity üzerinde işlem yapıldı
        public int? EntityId { get; set; } // İşlem yapılan kaydın ID'si
        public string? Description { get; set; } // İşlem detayı
        public string? OldValues { get; set; } // JSON formatında eski değerler
        public string? NewValues { get; set; } // JSON formatında yeni değerler
        public string? IpAddress { get; set; }
        public DateTime ActivityDate { get; set; } = DateTime.Now;
    }
}
