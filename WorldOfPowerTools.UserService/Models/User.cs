namespace WorldOfPowerTools.UserService.Models
{
    public class User
    {
        public static readonly int MinLoginLength = 5;
        public static readonly int MaxLoginLength = 50;

        public Guid? Id { get; protected set; }
        public string Login { get; protected set; }
        public string PasswordHash { get; protected set; }

#nullable disable
        protected User() { }

        public User(string login, string passwordHash)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentNullException(nameof(login));
            if (login.Length < MinLoginLength || login.Length > MaxLoginLength) throw new ArgumentOutOfRangeException(nameof(login));
            if (string.IsNullOrEmpty(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));

            Login = login;
            PasswordHash = passwordHash;
        }
    }
}
