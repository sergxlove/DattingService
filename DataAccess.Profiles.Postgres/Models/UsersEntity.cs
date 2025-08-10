﻿using Newtonsoft.Json.Linq;

namespace DataAccess.Profiles.Postgres.Models
{
    public class UsersEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Target { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public JArray PhotoURL { get; set; } = new JArray();

        public bool IsActive { get; set; }

        public bool IsVerify { get; set; }
    }
}
