using ClinicalTrial.BusinessLogic.Exceptions;
using ClinicalTrial.BusinessLogic.Helpers.ErrorMessages;
using ClinicalTrial.BusinessLogic.Services;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ClinicalTrial.Tests.Tests
{
    public class JsonValidationServiceTests
    {
        private IJsonValidationService _jsonValidationService;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IEmbeddedResourceProvider> _mbeddedResourceProviderMock;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(config => config["SchemaSettings:ClinicalTrialSchemaPath"])
                .Returns("ClinicalTrial.Api.Schemas.ClinicalTrialSchema.json");
            _mbeddedResourceProviderMock = new Mock<IEmbeddedResourceProvider>();
            _jsonValidationService = new JsonValidationService(_configurationMock.Object, _mbeddedResourceProviderMock.Object);
        }

        [Test]
        public void ValidateAndDeserialize_ValidJson_ReturnsObject()
        {
            var validJson = GetValidJson();

            var schemaJson = GetSchemaJson();

            var schemaStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(schemaJson));

            _configurationMock.Setup(c => c["SchemaSettings:ClinicalTrialSchemaPath"])
                .Returns("ClinicalTrial.Api.Schemas.ClinicalTrialSchema.json");

            _mbeddedResourceProviderMock.Setup(a => a.GetManifestResourceStream("ClinicalTrial.Api.Schemas.ClinicalTrialSchema.json"))
                .Returns(schemaStream);

            var result = _jsonValidationService.ValidateAndDeserialize(validJson);

            Assert.NotNull(result);
            Assert.That(result.TrialId, Is.EqualTo("TR898"));
            Assert.That(result.Title, Is.EqualTo("Test Trial"));
        }

        [Test]
        public void ValidateAndDeserialize_ThrowsBusinessException_When_Missing_File_Schema()
        {
            _mbeddedResourceProviderMock.Setup(a => a.GetManifestResourceStream("ClinicalTrialSchema.json")).Returns((Stream)null);
            var schemaPath = "ClinicalTrial.Api.Schemas.ClinicalTrialSchema.json";
            var errorMessage = string.Format(BusinessErrorMessages.SchemaFileNotFound, schemaPath);

            var validJson = GetValidJson();

            var ex = Assert.Throws<BusinessException>(() => _jsonValidationService.ValidateAndDeserialize(validJson));
            Assert.That(ex.Message, Is.EqualTo(errorMessage));
        }

        [Test]
        public void ValidateAndDeserialize_ThrowsBusinessException_When_Json_Is_Invalid()
        {
            var inValidJson = InvalidGetValidJson();

            var schemaJson = GetSchemaJson();

            var schemaStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(schemaJson));

            _configurationMock.Setup(c => c["SchemaSettings:ClinicalTrialSchemaPath"])
                .Returns("ClinicalTrial.Api.Schemas.ClinicalTrialSchema.json");

            _mbeddedResourceProviderMock.Setup(a => a.GetManifestResourceStream("ClinicalTrial.Api.Schemas.ClinicalTrialSchema.json"))
                .Returns(schemaStream);

            var ex = Assert.Throws<BusinessException>(() => _jsonValidationService.ValidateAndDeserialize(inValidJson));
            Assert.That(ex.Message, Is.EqualTo(BusinessErrorMessages.InvalidJsonFormat));
        }

        private string GetSchemaJson()
        {
            return @"{
            ""$schema"": ""http://json-schema.com/schema"",
            ""type"": ""object"",
            ""properties"": {
                ""trialId"": { ""type"": ""string"" },
                ""title"": { ""type"": ""string"" },
                ""startDate"": { ""type"": ""string"", ""format"": ""date"" },
                ""status"": {
                    ""type"": ""string"",
                    ""enum"": [""Not Started"", ""Ongoing"", ""Completed""]
                }
            },
            ""required"": [""trialId"", ""title"", ""startDate"", ""status""],
            ""additionalProperties"": false
            }";
        }

        private string InvalidGetValidJson()
        {
            return @"{
             """"trialId"""": """"TR898"""",
             """"title"""": """"Test Trial"""",
             """"startDate"""": """"2023-01-01""""
             // Missing ""status"" property, which is required by the schema
            }";
        }

        private string GetValidJson()
        {
            return @"{
            ""trialId"": ""TR898"",
            ""title"": ""Test Trial"",
            ""startDate"": ""2024-12-20"",
            ""status"": ""Ongoing""
            }";
        }
    }
}
