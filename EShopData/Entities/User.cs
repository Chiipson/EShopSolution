using EShopData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserInfo UserInfo { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart? Cart { get; set; }
    }
}
