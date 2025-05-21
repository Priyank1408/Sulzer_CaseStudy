using CaseStudy3.Interface;

namespace CaseStudy3.Services
{
    public class DataTransformationService : IDataTransformationService
    {
        public async Task<Stream> TransformDataAsync(Stream dataStream)
        {
            // Simulate data transformation
            await Task.FromResult(dataStream);
            return dataStream;
        }
    }
}