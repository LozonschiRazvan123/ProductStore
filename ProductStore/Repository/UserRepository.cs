using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) 
        {
            _context = context;
        }
        public bool Add(User user)
        {
            var userDTO = _context.Users.Select(u => u.Email != user.Email).Select(userCreateDTO => new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
                Email  = user.Email,
                VerificationToken = user.VerificationToken,
                PasswordHash = user.PasswordHash,
                PasswordResetToken = user.PasswordResetToken,
                PasswordSalt = user.PasswordSalt,
                ResetTokenExpires = user.ResetTokenExpires,
                VerifiedAt = user.VerifiedAt
            }).FirstOrDefault();
            _context.Add(userDTO);
            return Save();
        }
        public bool Save() 
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Delete(UserDTO user)
        {
            var userDTO = _context.Users.Select(userCreateDTO => new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role
            }).FirstOrDefault();
            _context.Remove(userDTO);
            return Save();
        }

        public bool ExistUser(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            return _context.Users.Where(u => u.Id == id).Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return _context.Users.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role
            }).ToList();
        }

        public bool Update(UserDTO user)
        {
            var userDTO = _context.Users.Select(userCreateDTO => new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role
            }).FirstOrDefault();
            _context.Update(userDTO);
            return Save();
        }
    }
}
