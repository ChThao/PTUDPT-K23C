// EduSubmit – lát cắt 1: Submission.Client (vai trò MÁY KHÁCH)
// Chạy: dotnet run --project src/Submission.Client -- http://localhost:5000

using System.Net.Http.Json;

// Địa chỉ API lấy từ tham số dòng lệnh → không gắn cứng vào một môi trường
var apiBaseUrl = args.Length > 0 ? args[0] : "http://localhost:5000";

using var http = new HttpClient {
    BaseAddress = new Uri(apiBaseUrl)
};

// Yêu cầu 1: GET /api/assignments
var assignments = await http.GetFromJsonAsync<List<Assignment>>("/api/assignments");
Console.WriteLine("Danh sach bai tap:");

foreach (var assignment in assignments ?? new List<Assignment>()) {
    Console.WriteLine($"- {assignment.Id}: {assignment.Title}");
}

// Yêu cầu 2: POST /api/submissions
var request = new CreateSubmissionRequest(
    AssignmentId: 1,
    StudentCode: "DTC245200486",
    RepositoryUrl: "https://github.com/thumayduong/PTUDPT-K23C");

var response = await http.PostAsJsonAsync("/api/submissions", request);
response.EnsureSuccessStatusCode();   // luôn kiểm tra mã trạng thái trước khi đọc kết quả

var result = await response.Content.ReadFromJsonAsync<SubmissionResult>();
Console.WriteLine($"Da nop bai. Ma luot nop: {result?.Id}");
Console.WriteLine($"Trang thai: {result?.Status}");

// Bước 12 (Bài mở rộng): Đọc lại lượt nộp vừa tạo
if (result != null) {
    var checkResponse = await http.GetAsync($"/api/submissions/{result.Id}");
    checkResponse.EnsureSuccessStatusCode();
    var verified = await checkResponse.Content.ReadFromJsonAsync<SubmissionResult>();
    Console.WriteLine($"[Mo rong] Doc lai thanh cong luot nop: {verified?.Id} ({verified?.Status})");
}

// Hợp đồng dữ liệu phía Client
record Assignment(int Id, string Title, string Description);

record CreateSubmissionRequest(
    int AssignmentId,
    string StudentCode,
    string RepositoryUrl);

record SubmissionResult(
    Guid Id,
    int AssignmentId,
    string StudentCode,
    string Status,
    DateTimeOffset ReceivedAt);
