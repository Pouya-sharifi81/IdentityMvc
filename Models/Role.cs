﻿using Microsoft.AspNetCore.Identity;

namespace MvcBuggetoEx.Models
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }

    }
}
