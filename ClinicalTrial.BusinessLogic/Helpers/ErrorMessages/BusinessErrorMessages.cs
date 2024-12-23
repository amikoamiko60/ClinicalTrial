namespace ClinicalTrial.BusinessLogic.Helpers.ErrorMessages
{
    public static class BusinessErrorMessages
    {
        public const string SchemaFileNotFound = "Schema file '{0}' not found as an embedded resource.";

        public const string InvalidFileType = "Invalid file type. Only .json files are allowed.";

        public const string FileShouldNotBeNull = "File should not be null";

        public const string FileSizeExceeds = "File size exceeds the 1MB limit.";

        public const string JsonValidationFailed = "JSON validation failed: {0}";

        public const string FailedToLoadJson = "Failed to load or parse the JSON schema.";

        public const string InvalidJsonFormat = "Invalid JSON format. Unable to parse JSON.";
    }
}
