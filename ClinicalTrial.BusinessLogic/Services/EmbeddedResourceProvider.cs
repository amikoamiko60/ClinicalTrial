using ClinicalTrial.BusinessLogic.Services.Interfaces;

namespace ClinicalTrial.BusinessLogic.Services
{
    public sealed class EmbeddedResourceProvider : IEmbeddedResourceProvider
    {
        public Stream GetManifestResourceStream(string resourceName)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.FullName.Contains("ClinicalTrial.Api"));
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
