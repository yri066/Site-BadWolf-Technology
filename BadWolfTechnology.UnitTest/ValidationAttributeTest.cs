using BadWolfTechnology.Data.DataAnnotations;
using BadWolfTechnology.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using Moq;

namespace BadWolfTechnology.UnitTest
{
    public class ValidationAttributeTest
    {
        [Fact]
        public void MinimumAgeAttribute_OverMinimumAge_Success()
        {
            // Arrange
            var todayDate = new DateTime(2024, 05, 08);
            var birthDate = new DateTime(2000, 11, 23);
            var minimumAge = 16;

            var fakeDate = new Mock<IDateTime>();
            fakeDate.Setup(mockDate => mockDate.Today).Returns(todayDate);
            var fakeServiceProvider = new Mock<IServiceProvider>();
            fakeServiceProvider.Setup(mock => mock.GetService(typeof(IDateTime))).Returns(fakeDate.Object);
            var attribute = new MinimumAgeAttribute(minimumAge);
            var context = new ValidationContext(birthDate, fakeServiceProvider.Object, null);

            // Act
            var result = attribute.GetValidationResult(birthDate, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void MinimumAgeAttribute_UnderMinimumAge_ErrorResult()
        {
            // Arrange
            var todayDate = new DateTime(2024, 05, 08);
            var birthDate = new DateTime(2008, 11, 23);
            var minimumAge = 16;
            string ExpectedErrorMessage = $"Возраст должен быть не менее {minimumAge} лет.";

            var fakeDate = new Mock<IDateTime>();
            fakeDate.Setup(mockDate => mockDate.Today).Returns(todayDate);
            var fakeServiceProvider = new Mock<IServiceProvider>();
            fakeServiceProvider.Setup(mock => mock.GetService(typeof(IDateTime))).Returns(fakeDate.Object);
            var attribute = new MinimumAgeAttribute(minimumAge);
            var context = new ValidationContext(birthDate, fakeServiceProvider.Object, null);

            // Act
            var result = attribute.GetValidationResult(birthDate, context);

            // Assert
            var validationResult = Assert.IsType<ValidationResult>(result);
            Assert.Equal(ExpectedErrorMessage, validationResult.ErrorMessage);
        }
    }
}
