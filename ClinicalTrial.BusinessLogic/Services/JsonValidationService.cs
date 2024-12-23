using System.ComponentModel.DataAnnotations;
using ClinicalTrial.BusinessLogic.Exceptions;
using ClinicalTrial.BusinessLogic.Helpers.ErrorMessages;
using ClinicalTrial.BusinessLogic.Services.Interfaces;
using ClinicalTrial.DataContracts.Requests;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ClinicalTrial.BusinessLogic.Services
{
    public sealed class JsonValidationService
        (IConfiguration configuration,
         IEmbeddedResourceProvider embeddedResourceProvider) : IJsonValidationService
    {
        private readonly string _schemaPath = configuration["SchemaSettings:ClinicalTrialSchemaPath"];

        public UploadTrialRequest ValidateAndDeserialize(string jsonData)
        {
            using var stream = embeddedResourceProvider.GetManifestResourceStream(_schemaPath);

            if (stream == null)
            {
                throw new BusinessException(string.Format(BusinessErrorMessages.SchemaFileNotFound, _schemaPath));
            }

            string schemaJson;
            using (var reader = new StreamReader(stream))
            {
                schemaJson = reader.ReadToEnd();
            }

            JSchema schema;
            try
            {
                schema = JSchema.Parse(schemaJson);
            }
            catch (JsonException ex)
            {
                throw new BusinessException(BusinessErrorMessages.FailedToLoadJson, ex);
            }

            JObject jsonObject;
            try
            {
                jsonObject = JObject.Parse(jsonData);
            }
            catch (JsonReaderException ex)
            {
                throw new BusinessException(BusinessErrorMessages.InvalidJsonFormat, ex);
            }

            IList<string> validationErrors = new List<string>();
            bool isValid = jsonObject.IsValid(schema, out validationErrors);

            if (!isValid)
            {
                var errorMessage = string.Join("; ", validationErrors);
                throw new ValidationException(string.Format(BusinessErrorMessages.JsonValidationFailed, errorMessage));
            }

            return jsonObject.ToObject<UploadTrialRequest>();
        }
    }
}
