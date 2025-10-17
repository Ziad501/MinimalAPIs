using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinimalAPIs.OpenApiSpecs
{
    public static class StudentsSpecs
    {
        public static OpenApiOperation List(OpenApiOperation op)
        {
            op.Summary = "List all students (paged)";
            op.Description = "Paged list. Defaults: pageNumber=1, pageSize=10. Max pageSize=100.";
            op.Parameters ??= new List<OpenApiParameter>();

            if (op.Responses.TryGetValue(StatusCodes.Status200OK.ToString(), out var resp) &&
                resp.Content.TryGetValue("application/json", out var media))
            {
                media.Example = new OpenApiObject
                {
                    ["items"] = new OpenApiArray {
                    new OpenApiObject {
                        ["id"]=new OpenApiInteger(501),
                        ["firstName"]=new OpenApiString("Sara"),
                        ["lastName"]=new OpenApiString("Ali"),
                        ["dateOfBirth"]=new OpenApiString("2001-05-17"),
                        ["idNumber"]=new OpenApiString("A123456789"),
                        ["picture"]=new OpenApiString("https://example.com/sara.jpg")
                    }
                },
                    ["pageNumber"] = new OpenApiInteger(1),
                    ["pageSize"] = new OpenApiInteger(10),
                    ["totalCount"] = new OpenApiInteger(250),
                    ["totalPages"] = new OpenApiInteger(25),
                    ["hasPrevious"] = new OpenApiBoolean(false),
                    ["hasNext"] = new OpenApiBoolean(true)
                };
            }
            return op;
        }

        public static OpenApiOperation GetById(OpenApiOperation op)
        { op.Summary = "Get a student by ID"; op.Description = "Returns 404 if not found."; return op; }

        public static OpenApiOperation Create(OpenApiOperation op)
        { op.Summary = "Create a new student"; op.Description = "Returns 201 with Location header."; return op; }

        public static OpenApiOperation Update(OpenApiOperation op)
        { op.Summary = "Update a student"; op.Description = "Returns 204 on success."; return op; }

        public static OpenApiOperation Delete(OpenApiOperation op)
        { op.Summary = "Delete a student"; op.Description = "Removes a student by ID."; return op; }
    }
}