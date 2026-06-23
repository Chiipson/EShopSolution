using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Entities
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public User User { get; set; }
    }
}
