using System;
using System.Diagnostics.CodeAnalysis;

namespace AcceptanceTests.AppInterface.Data
{
    public class UserInfo : IEquatable<UserInfo>
    {
        public UserInfo(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or whitespace.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));
            }

            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }

        public UserInfo WithDifferentPassword(string password)
        {
            if (password == Password)
            {
                throw new ArgumentException("Trying to create the same user with a different password, but the password provided is the same.", nameof(password));
            }

            return new UserInfo(Username, password);
        }

        public override bool Equals(object? obj) => obj is UserInfo other && Equals(other);
        public bool Equals([AllowNull] UserInfo other) => other != null && other.Username == Username;
        public override int GetHashCode() => Username.GetHashCode();
    }
}
