﻿using Microsoft.AspNetCore.Identity;

namespace EBest.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now; 



    }
}
