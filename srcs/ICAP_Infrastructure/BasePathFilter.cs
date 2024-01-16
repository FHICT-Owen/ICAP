using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace ICAP_Infrastructure
{
    public class BasePathFilter(string basePath) : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = basePath } };
        }
    }
}
