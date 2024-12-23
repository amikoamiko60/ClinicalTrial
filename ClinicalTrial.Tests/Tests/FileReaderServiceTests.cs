using System.Text;
using ClinicalTrial.BusinessLogic.Exceptions;
using ClinicalTrial.BusinessLogic.Helpers.ErrorMessages;
using ClinicalTrial.BusinessLogic.Services;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ClinicalTrial.Tests.Tests
{
    public class FileReaderServiceTests
    {
        private IFileReaderService _fileReaderService;

        [SetUp]
        public void Setup()
        {
            _fileReaderService = new FileReaderService();
        }

        [Test]
        public async Task ReadFileAsync_ValidFile_ReturnsFileContent()
        {
            var expectedContent = "This is a test JSON file content.";
            var fileMock = new Mock<IFormFile>();

            var fileContent = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));
            fileMock.Setup(a => a.OpenReadStream()).Returns(fileContent);

            var cancellationToken = CancellationToken.None;

            var result = await _fileReaderService.ReadFileAsync(fileMock.Object, cancellationToken);

            Assert.That(result, Is.EqualTo(expectedContent));
        }

        [Test]
        public void ReadFileAsync_NullFile_ThrowsArgumentNullException()
        {
            IFormFile file = null;

            Assert.ThrowsAsync<BusinessException>(async () => await _fileReaderService.ReadFileAsync(file, CancellationToken.None), BusinessErrorMessages.FileShouldNotBeNull);
        }
    }
}
