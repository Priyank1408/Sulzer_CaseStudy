namespace CaseStudy3.Interface
{
    public interface IDataTransformationService
    {
        Task<Stream> TransformDataAsync(Stream dataStream);
    }
}