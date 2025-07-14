using AuthApi.Modals;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace AuthApi.Services;

/// <summary>
/// Service for handling user operations with MongoDB.
/// </summary>
public class UserService
{
    private readonly IMongoCollection<User> _users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="settings">MongoDB configuration settings.</param>
    public UserService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _users = database.GetCollection<User>("Users");
    }

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user object if found; otherwise, null.</returns>
    public async Task<User> GetByUsername(string username) =>
        await _users.Find(u => u.Username == username).FirstOrDefaultAsync();

    /// <summary>
    /// Retrieves a user by Email.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user object if found; otherwise, null.</returns>
    public async Task<User> GetByEmail(string username) =>
        await _users.Find(u => u.Email == username).FirstOrDefaultAsync();

    /// <summary>
    /// Retrieves a user by Phoneno.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user object if found; otherwise, null.</returns>
    public async Task<User> GetByPhone(string username) =>
        await _users.Find(u => u.PhoneNo != null && u.PhoneNo.Number == username).FirstOrDefaultAsync();

    /// <summary>
    /// Creates a new user with a hashed password.
    /// </summary>
    /// <param name="username">The username for the new user.</param>
    /// <param name="password">The plain-text password to be hashed.</param>
    public async Task CreateUser(string username, string password)
    {
        //var passwordHash = HashPassword(password);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { Username = username, PasswordHash = passwordHash };
        await _users.InsertOneAsync(user);
    }

    /// <summary>
    /// Verifies a plain-text password against its hash.
    /// </summary>
    /// <param name="password">The plain-text password input.</param>
    /// <param name="hash">The stored hashed password.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public bool VerifyPassword(string password, string hash) =>
        //Verify(password, hash);
        BCrypt.Net.BCrypt.Verify(password, hash);
    // SHA-512 password hashing with salt
    public string HashPassword(string password)
    {
        // Generate a random salt
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        var salt = Convert.ToBase64String(saltBytes);
        var passwordBytes = Encoding.UTF8.GetBytes(password + salt);

        using (var sha512 = SHA512.Create())
        {
            var hashBytes = sha512.ComputeHash(passwordBytes);
            var hash = Convert.ToBase64String(hashBytes);

            // Combine salt and hash
            return $"{salt}:{hash}";
        }
    }

    public bool Verify(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2)
            return false;

        var salt = parts[0];
        var hash = parts[1];

        var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
        using (var sha512 = SHA512.Create())
        {
            var hashBytes = sha512.ComputeHash(passwordBytes);
            var computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == hash;
        }
    }
}