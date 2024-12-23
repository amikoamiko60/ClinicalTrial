namespace ClinicalTrial.BusinessLogic.Services.Interfaces
{
    public interface IEmbeddedResourceProvider
    {
        Stream GetManifestResourceStream(string resourceName);
    }
}
