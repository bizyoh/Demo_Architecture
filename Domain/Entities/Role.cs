﻿
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role: IdentityRole<int>
    {
       public string Description { get; set; }
    }
}
