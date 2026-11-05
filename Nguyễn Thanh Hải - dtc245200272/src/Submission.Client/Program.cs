using System.Net.Http.Json;

if (args.Length == 0)
{
    Console.WriteLine("Vui long truyen URL cua API (VD: http://localhost:5000).");
    return;
}

var apiUrl = args[0];
using var client = new HttpClient { BaseAddress = new Uri(apiUrl) };

try
{
    var assignments = await client.GetFromJsonAsync<Assignment[]>("/api/assignments");
    Console.WriteLine("Danh sach bai tap:");
    foreach (var a in assignments!)
    {
        Console.WriteLine($"- {a.Id}: {a.Title}");
    }

    var request = new { StudentCode = "Nguyen Thanh Hai", AssignmentId = 1 };
    var response = await client.PostAsJsonAsync("/api/submissions", request);
    
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<SubmissionResult>();
    Console.WriteLine($"Da nop bai. Ma luot nop: {result!.SubmissionId}");
    Console.WriteLine($"Trang thai: {result.Status}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Loi ket noi hoac loi tu server: {ex.Message}");
}

class Assignment
{
    public int Id { get; set; }
    public string Title { get; set; }
}

class SubmissionResult
{
    public string SubmissionId { get; set; }
    public string Status { get; set; }
}
