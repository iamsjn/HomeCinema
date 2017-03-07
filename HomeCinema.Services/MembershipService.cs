﻿using HomeCinema.Data;
using HomeCinema.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.Services
{
    public class MembershipService : IMembershipService
    {
        #region Variables
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<UserRole> _userRoleRepository;
        private readonly IEntityBaseRepository<Role> _roleRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        public MembershipService(IEntityBaseRepository<User> userRepository, IEntityBaseRepository<UserRole> userRoleRepository,
            IEntityBaseRepository<Role> roleRepository, IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }

        private void addUserToRole(User user, int roleId)
        {
            var role = _roleRepository.GetSingle(roleId);
            if (role == null)
                throw new ApplicationException("Role doesn't exist.");
            var userRole = new UserRole()
            {
                RoleId = role.ID,
                UserId = user.ID
            };
            _userRoleRepository.Add(userRole);
        }
        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password,
           user.Salt), user.HashedPassword);
        }

        private bool isUserValid(User user, string password)
        {
            if (isPasswordValid(user, password))
            {
                return !user.IsLocked;
            }
            return false;
        }
        {
            var existingUser = _userRepository.GetSingleByUsername(username);
            if (existingUser != null)
            {
                throw new Exception("Username is already in use");
            }
            var passwordSalt = _encryptionService.CreateSalt();
            var user = new User()
            {
                Username = username,
                Salt = passwordSalt,
                Email = email,
                IsLocked = false,
                HashedPassword = _encryptionService.EncryptPassword(password, passwordSalt),
                DateCreated = DateTime.Now
            };
            _userRepository.Add(user);
            _unitOfWork.Commit();

            if (roles != null || roles.Length > 0)
            {
                foreach (var role in roles)
                {
                    addUserToRole(user, role);
                }
            }
            _unitOfWork.Commit();
            return user;
        }

        public User GetUser(int userId)
        {
            return _userRepository.GetSingle(userId);
        }
        public List<Role> GetUserRoles(string username)
        {
            List<Role> _result = new List<Role>();
            var existingUser = _userRepository.GetSingleByUsername(username);
            if (existingUser != null)
            {
                foreach (var userRole in existingUser.UserRoles)
                {
                    _result.Add(userRole.Role);
                }
            }
            return _result.Distinct().ToList();
        }
    }
}