using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Voltly.Api.Swagger
{
    /// <summary> Inserts title/description for each tag (controller) in the Swagger document. </summary>
    public class TagDescriptionsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument doc, DocumentFilterContext context)
        {
            doc.Tags = new List<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name        = "Auth",
                    Description = "Authentication and authorization endpoints, including login and registration."
                },
                new OpenApiTag
                {
                    Name        = "Users",
                    Description = "Endpoints for managing user profiles, including updating personal information and admin functionalities."
                }
            };
        }
    }
}