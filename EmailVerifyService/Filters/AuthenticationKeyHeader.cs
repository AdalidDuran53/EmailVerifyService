using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EmailVerifyService.Filters
{
    public class AuthenticationKeyHeader : Attribute, IOperationFilter
    {
        // Add the AuthenticationKey header to all operations
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "AuthenticationKey",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Format = "uuid",
                    Example = "e3f9c7a2-4b6d-4a1e-9f3d-8c2b1a7d6f91"
                },
                Description = "API Key for authentication"
            });
        }
    }
}
