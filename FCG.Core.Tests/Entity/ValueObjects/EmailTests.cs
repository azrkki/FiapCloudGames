using FCG.Core.Entity.ValueObjects;
using NUnit.Framework;
using System;

namespace FCG.Core.Tests.Entity.ValueObjects
{
    [TestFixture]
    public class EmailTests
    {
        [Test]
        public void Create_ValidEmail_ReturnsEmailObject()
        {
            // Arrange
            var validEmail = "test@example.com";

            // Act
            var email = Email.Create(validEmail);

            // Assert
            Assert.IsNotNull(email);
            Assert.AreEqual(validEmail, email.Value);
        }

        [Test]
        public void Create_InvalidEmail_ThrowsArgumentException()
        {
            // Arrange
            var invalidEmail = "invalid-email";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
        }

        [Test]
        public void Create_EmptyEmail_ThrowsArgumentNullException()
        {
            // Arrange
            var emptyEmail = "";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Email.Create(emptyEmail));
        }

        [Test]
        public void Create_NullEmail_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Email.Create(null));
        }
    }
}