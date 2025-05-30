using Microsoft.VisualStudio.TestTools.UnitTesting;
using PourfectApp.Core.Models;

namespace PourfectApp.Tests.Models
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void HashPassword_ShouldReturnConsistentHash()
        {
            // Arrange
            string password = "testPassword123";

            // Act
            string hash1 = User.HashPassword(password);
            string hash2 = User.HashPassword(password);

            // Assert
            Assert.AreEqual(hash1, hash2, "Same password should produce same hash");
        }

        [TestMethod]
        public void HashPassword_DifferentPasswords_ShouldReturnDifferentHashes()
        {
            // Arrange
            string password1 = "password123";
            string password2 = "password456";

            // Act
            string hash1 = User.HashPassword(password1);
            string hash2 = User.HashPassword(password2);

            // Assert
            Assert.AreNotEqual(hash1, hash2, "Different passwords should produce different hashes");
        }

        [TestMethod]
        public void VerifyPassword_CorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            string password = "correctPassword";
            var user = new User
            {
                Username = "testuser",
                PasswordHash = User.HashPassword(password)
            };

            // Act
            bool result = user.VerifyPassword(password);

            // Assert
            Assert.IsTrue(result, "Correct password should verify successfully");
        }

        [TestMethod]
        public void VerifyPassword_IncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            string correctPassword = "correctPassword";
            string wrongPassword = "wrongPassword";
            var user = new User
            {
                Username = "testuser",
                PasswordHash = User.HashPassword(correctPassword)
            };

            // Act
            bool result = user.VerifyPassword(wrongPassword);

            // Assert
            Assert.IsFalse(result, "Incorrect password should not verify");
        }
    }
}