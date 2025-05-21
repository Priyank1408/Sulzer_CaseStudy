namespace CaseStudy1.Interface
{
    public interface IDataIngestionService
    {
        /// <summary>
        /// Ingests data stream for a specific tenant and data type
        /// </summary>
        /// <param name="dataStream">The stream containing the data to be ingested</param>
        /// <param name="tenantId">The identifier of the tenant</param>
        /// <param name="dataType">The type of data being ingested</param>
        /// <returns>The unique identifier of the ingested data</returns>
        Task<string> IngestDataAsync(Stream dataStream, string tenantId, string dataType);

        /// <summary>
        /// Retrieves previously ingested data
        /// </summary>
        /// <param name="id">The unique identifier of the data</param>
        /// <param name="tenantId">The identifier of the tenant</param>
        /// <returns>The data as a byte array</returns>
        Task<byte[]> RetrieveDataAsync(string id, string tenantId);
    }
}