﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcBuggetoEx.Areas.Admin.Models
{
    public class AddUserRoleDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}