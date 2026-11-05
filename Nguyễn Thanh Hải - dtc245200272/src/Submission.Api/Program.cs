var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var assignments = new[]
{
    new { Id = 1, Title = "Bai tap 1" },
    new { Id = 2, Title = "Bai tap 2" },
    new { Id = 3, Title = "Bai tap 3" } 
};

var submissions = new List<SubmissionResult>();

app.MapGet("/api/assignments", () => Results.Ok(assignments));

app.MapPost("/api/submissions", (SubmissionRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.StudentCode))
    {
        return Results.BadRequest(new { Error = "StudentCode khong duoc de trong!" });
    }

    var result = new SubmissionResult
    {
        SubmissionId = Guid.NewGuid().ToString(),
        Status = "received"
    };
    submissions.Add(result);

    return Results.Created($"/api/submissions/{result.SubmissionId}", result);
});

app.Run();

class SubmissionRequest
{
    public string StudentCode { get; set; }
    public int AssignmentId { get; set; }
}

class SubmissionResult
{
    public string SubmissionId { get; set; }
    public string Status { get; set; }
}