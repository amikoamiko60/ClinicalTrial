using ClinicalTrial.BusinessLogic.Exceptions;
using ClinicalTrial.BusinessLogic.Helpers.ErrorMessages;
using ClinicalTrial.BusinessLogic.Services;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ClinicalTrial.Tests.Tests
{
    public class FileValidationServiceTests
    {
        private IFileValidationService _fileValidationService;

        [SetUp]
        public void Setup()
        {
            _fileValidationService = new FileValidationService();
        }

        [Test]
        public void ValidateFile_ThrowsBusinessException_When_File_Is_Null()
        {
            IFormFile file = null;

            var ex = Assert.Throws<BusinessException>(() => _fileValidationService.ValidateFile(file));
            Assert.That(ex.Message, Is.EqualTo(BusinessErrorMessages.InvalidFileType));
        }

        [Test]
        public void ValidateFile_ThrowsBusinessException_When_File_Type_Is_Invalid()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(a => a.FileName).Returns("invalid.txt");

            var ex = Assert.Throws<BusinessException>(() => _fileValidationService.ValidateFile(fileMock.Object));
            Assert.That(ex.Message, Is.EqualTo(BusinessErrorMessages.InvalidFileType));
        }

        [Test]
        public void ValidateFile_ThrowsBusinessException_When_File_Size_Exceeds_Limit()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(a => a.FileName).Returns("valid.json");
            fileMock.Setup(a => a.Length).Returns(2 * 1024 * 1024);

            var ex = Assert.Throws<BusinessException>(() => _fileValidationService.ValidateFile(fileMock.Object));
            Assert.That(ex.Message, Is.EqualTo(BusinessErrorMessages.FileSizeExceeds));
        }

        [Test]
        public void ValidateFile_ValidFile_DoesNotThrow()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(a => a.FileName).Returns("valid.json");
            fileMock.Setup(a => a.Length).Returns(500 * 1024);

            Assert.DoesNotThrow(() => _fileValidationService.ValidateFile(fileMock.Object));
        }
    }
}
