using ClinicalTrial.BusinessLogic.Services;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using ClinicalTrial.DataAccess;
using ClinicalTrial.DataAccess.Repositories;
using ClinicalTrial.DataContracts;
using ClinicalTrial.DataContracts.Requests;
using ClinicalTrial.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ClinicalTrial.Tests.Tests
{
    public class ClinicalTrialServiceTests
    {
        private Mock<IClinicalTrialRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IJsonValidationService> _mockJsonValidationService;
        private Mock<IFileValidationService> _mockFileValidationService;
        private Mock<IFileReaderService> _mockFileReaderService;

        private IClinicalTrialService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IClinicalTrialRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockJsonValidationService = new Mock<IJsonValidationService>();
            _mockFileValidationService = new Mock<IFileValidationService>();
            _mockFileReaderService = new Mock<IFileReaderService>();

            _service = new ClinicalTrialService(
                _mockRepository.Object,
                _mockUnitOfWork.Object,
                _mockJsonValidationService.Object,
                _mockFileValidationService.Object,
                _mockFileReaderService.Object);
        }

        [Test]
        public async Task UploadTrialAsync_ShouldSetDefaultEndDateAndCalculateDuration_WhenStatusIsOngoing()
        {
            var mockFile = new Mock<IFormFile>();
            var mockJson = @"{ ""Status"": ""Ongoing"", ""StartDate"": ""2024-12-01"" }";

            var trialData = new UploadTrialRequest
            {
                Status = nameof(ClinicalTrialStatus.Ongoing),
                StartDate = new DateTime(2024, 12, 01),
                EndDate = null
            };

            _mockFileValidationService.Setup(a => a.ValidateFile(It.IsAny<IFormFile>()));
            _mockFileReaderService.Setup(a => a.ReadFileAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockJson);
            _mockJsonValidationService.Setup(a => a.ValidateAndDeserialize(It.IsAny<string>()))
                .Returns(trialData);

            await _service.UploadTrialAsync(mockFile.Object, CancellationToken.None);

            _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
            _mockJsonValidationService.Verify(j => j.ValidateAndDeserialize(mockJson), Times.Once);

            Assert.That(trialData.EndDate, Is.EqualTo(new DateTime(2025, 01, 01)));
            Assert.That(trialData.Duration, Is.EqualTo(31));

            _mockRepository.Verify(r => r.AddTrialAsync(trialData, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UploadTrialAsync_ShouldCalculateDuration_WhenEndDateIsProvided()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var mockJson = @"{ ""Status"": ""Completed"", ""StartDate"": ""2025-01-01"", ""EndDate"": ""2025-01-31"" }";

            var trialData = new UploadTrialRequest
            {
                Status = nameof(ClinicalTrialStatus.Completed),
                StartDate = new DateTime(2025, 01, 01),
                EndDate = new DateTime(2025, 01, 31)
            };

            _mockFileValidationService.Setup(a => a.ValidateFile(It.IsAny<IFormFile>()));
            _mockFileReaderService.Setup(a => a.ReadFileAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockJson);
            _mockJsonValidationService.Setup(a => a.ValidateAndDeserialize(It.IsAny<string>()))
                .Returns(trialData);

            await _service.UploadTrialAsync(mockFile.Object, CancellationToken.None);

            Assert.That(trialData.Duration, Is.EqualTo(30));
            _mockRepository.Verify(a => a.AddTrialAsync(trialData, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetTrialAsync_ShouldReturnCorrectTrial_WhenIdIsValid()
        {
            // Arrange
            int trialId = 1;
            var expectedTrial = new GetTrialResponse { Id = trialId, TrialId = "Trial 1" };

            _mockRepository
                .Setup(a => a.GetTrialAsync(trialId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTrial);

            var result = await _service.GetTrialAsync(trialId, CancellationToken.None);

            Assert.That(result, Is.EqualTo(expectedTrial));
            _mockRepository.Verify(a => a.GetTrialAsync(trialId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetTrialsAsync_ShouldReturnListOfTrials_WhenRequestIsValid()
        {
            var request = new GetTrialsRequest { Status = "Ongoing" };
            var expectedTrials = new List<GetTrialResponse>
            {
                new GetTrialResponse { Id = 1, TrialId = "Trial 1" },
                new GetTrialResponse { Id = 2, TrialId = "Trial 2" }
            };

            _mockRepository
                .Setup(a => a.GetTrialsAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTrials);

            var result = await _service.GetTrialsAsync(request, CancellationToken.None);

            Assert.That(result, Is.EqualTo(expectedTrials));
            _mockRepository.Verify(a => a.GetTrialsAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
