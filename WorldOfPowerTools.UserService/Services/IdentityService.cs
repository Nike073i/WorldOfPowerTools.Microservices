using System.Security.Cryptography;
using System.Text;
using WorldOfPowerTools.UserService.Data;
using WorldOfPowerTools.UserService.Exceptions;
using WorldOfPowerTools.UserService.Models;

namespace WorldOfPowerTools.UserService.Services
{
    public class IdentityService
    {
        private static readonly string UserExistErrorMessage = "Пользователь с таким логином уже существует";

        private readonly DbUserRepository _userRepository;

        public IdentityService(DbUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Authorization(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentNullException(nameof(login));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var userByLogin = await _userRepository.GetByLoginAsync(login);
            if (userByLogin == null) return null;
            var user = PasswordHash(password).Equals(userByLogin.PasswordHash, StringComparison.OrdinalIgnoreCase)
                ? userByLogin : null;
            return user;
        }
        public async Task<User> Registration(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentNullException(nameof(login));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var userByLogin = await _userRepository.GetByLoginAsync(login);
            if (userByLogin != null) throw new UserExistsException(UserExistErrorMessage);
            var newUser = new User(login, PasswordHash(password));
            return await _userRepository.SaveAsync(newUser);
        }

        public string PasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
