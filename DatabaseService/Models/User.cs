﻿using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models
{
    public class User
    {
        [Key]
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; } = "https://cdn.discordapp.com/embed/avatars/0.png";
        public DateTime FirstAuthorizationDate { get; set; }
        public List<Server> Servers { get; set; }
    }
}
