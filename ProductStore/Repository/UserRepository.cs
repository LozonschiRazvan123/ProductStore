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
            User? existingUser = _context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (existingUser == null)
            {
                var userDTO = new User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    Role = user.Role,
                    Email = user.Email,
                    VerificationToken = user.VerificationToken,
                    PasswordHash = user.PasswordHash,
                    PasswordResetToken = user.PasswordResetToken,
                    PasswordSalt = user.PasswordSalt,
                    ResetTokenExpires = user.ResetTokenExpires,
                    VerifiedAt = user.VerifiedAt,
                    ImageProfile = user.ImageProfile
                };
                _context.Add(userDTO);
                return Save();
            }
            return false;
        }
        public bool Save() 
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Delete(UserDTO user)
        {
            var userDTO = _context.Users.Find(user.Id);
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
                ImageProfile = user.ImageProfile
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return _context.Users.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
                ImageProfile = user.ImageProfile
            }).ToList();
        }

        public bool Update(UserDTO user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser != null)
            {

                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;
                existingUser.ImageProfile = user.ImageProfile;
                _context.Update(existingUser);
                return Save();
            }

            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public async Task<User> GetUserByToken(string token)
        {
            return _context.Users.Where(u => u.VerificationToken == token).FirstOrDefault();
        }

        public async Task<User> GetUserByTokenVerification(string token)
        {
            return _context.Users.Where(u => u.PasswordResetToken == token).FirstOrDefault();
        }
    }
}
