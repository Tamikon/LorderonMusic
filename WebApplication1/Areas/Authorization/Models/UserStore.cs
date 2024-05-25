﻿using DatabaseService;
using DatabaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Areas.Authorization.Models
{
    public class UserStore
    {
        private readonly IUserRepository _userRepository;

        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddOrUpdateUser(User user)
        {
            var existingUser = await _userRepository.GetUser(user.DiscordId);
            if (existingUser == null)
            {
                if (string.IsNullOrEmpty(user.AvatarUrl))
                {
                    user.AvatarUrl = "https://cdn.discordapp.com/embed/avatars/0.png"; 
                }
                await _userRepository.AddUser(user);
            }
            else
            {
                existingUser.Username = user.Username;
                existingUser.AvatarUrl = string.IsNullOrEmpty(user.AvatarUrl) ? "https://cdn.discordapp.com/embed/avatars/0.png" : user.AvatarUrl;
                existingUser.Guilds = user.Guilds;
                await _userRepository.UpdateUser(existingUser);
            }
        }


        public async Task<User> GetUser(string discordId)
        {
            return await _userRepository.GetUser(discordId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
    }
}
