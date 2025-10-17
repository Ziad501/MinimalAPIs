using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinimalAPIs.OpenApiSpecs
{
    public static class EnrollmentsSpecs
    {
        public static OpenApiOperation List(OpenApiOperation op)
        {
            op.Summary = "List all enrollments (paged)";
            op.Description = "Paged list. Defaults: pageNumber=1, pageSize=10. Max pageSize=100.";
            op.Parameters ??= new List<OpenApiParameter>();

            if (op.Responses.TryGetValue(StatusCodes.Status200OK.ToString(), out var resp) &&
                resp.Content.TryGetValue("application/json", out var media))
            {
                media.Example = new OpenApiObject
                {
                    ["items"] = new OpenApiArray {
                    new OpenApiObject {
                        ["id"]=new OpenApiInteger(9001),
                        ["courseId"]=new OpenApiInteger(101),
                        ["studentId"]=new OpenApiInteger(501)
                    }
                },
                    ["pageNumber"] = new OpenApiInteger(1),
                    ["pageSize"] = new OpenApiInteger(10),
                    ["totalCount"] = new OpenApiInteger(300),
                    ["totalPages"] = new OpenApiInteger(30),
                    ["hasPrevious"] = new OpenApiBoolean(false),
                    ["hasNext"] = new OpenApiBoolean(true)
                };
            }
            return op;
        }

        public static OpenApiOperation GetById(OpenApiOperation op)
        { op.Summary = "Get an enrollment by ID"; op.Description = "Returns 404 if not found."; return op; }

        public static OpenApiOperation Create(OpenApiOperation op)
        { op.Summary = "Create an enrollment"; op.Description = "Returns 201 with Location header."; return op; }

        public static OpenApiOperation Update(OpenApiOperation op)
        { op.Summary = "Update an enrollment"; op.Description = "Returns 204 on success."; return op; }

        public static OpenApiOperation Delete(OpenApiOperation op)
        { op.Summary = "Delete an enrollment"; op.Description = "Removes an enrollment by ID."; return op; }
    }
}