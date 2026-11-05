# Báo Cáo Thực Hành Bài 1: Tổng Quan Hệ Thống và Các Mô Hình Trao Đổi Thông Điệp (MEP)

- **Họ và tên:** Nguyễn Thanh Sơn  
- **Mã sinh viên:** DTC245200486  
- **Lớp:** CNTT K23C  
- **Môn học:** Phát triển ứng dụng phân tán  
- **Môi trường thực hành:** macOS (Apple Silicon), .NET 10.0 SDK, Visual Studio Code  

---

## 1. Cài đặt môi trường (Bước 1 - Bước 4)

- Kiểm tra phiên bản SDK: `.NET 10.0.203`
- Kiểm tra Git: `git version 2.39.5`
- Dự án mẫu `HelloPtudpt`: Biên dịch và chạy thành công, terminal xuất ra dòng chữ: `Hello, World!`

---

## 2. MepDemo — Khảo sát 5 mô hình trao đổi thông điệp (Bước 7)

### Trả lời câu hỏi:
1. **Ở MEP 3 (Queue-based), mỗi bài nộp sub-x được mấy worker xử lý?**
   - Mỗi bài nộp chỉ được **đúng 1 worker duy nhất** xử lý.
   - *Giải thích:* Đây là cơ chế hàng đợi cạnh tranh (competing consumers). Thông điệp nằm trong queue, worker nào rảnh trước sẽ lấy ra xử lý, giúp cân bằng tải và loại bỏ trùng lặp công việc.

2. **Ở MEP 4 (Publish-Subscribe), một sự kiện được gửi tới mấy dịch vụ?**
   - Một sự kiện nộp bài được gửi tới **toàn bộ 3 dịch vụ** đăng ký nhận tin (Notification Service, Audit Service, Analytics Service).
   - *Giải thích:* Nhà phát hành (Publisher) chỉ phát thông điệp lên kênh chung (Topic). Mọi bên đăng ký (Subscribers) đều nhận được bản sao độc lập của sự kiện đó.

3. **Mô tả ngắn gọn (1 câu) cho mỗi mô hình MEP:**
   - **MEP 1 (Request - Response):** Trao đổi thông điệp hai chiều, bên yêu cầu gửi request và chờ bên nhận trả về phản hồi tương ứng.
   - **MEP 2 (One-Way / Fire & Forget):** Trao đổi một chiều, client gửi thông điệp đi và giải phóng tài nguyên ngay mà không chờ bất kỳ kết quả phản hồi nào.
   - **MEP 3 (Queue-based):** Sử dụng hàng đợi trung gian để đệm thông điệp, cho phép nhiều worker cùng chia nhau xử lý tải bất đồng bộ.
   - **MEP 4 (Publish - Subscribe):** Mô hình phân phối một-nhiều, sự kiện từ bên phát được đồng loạt chuyển đến tất cả các bên đăng ký quan tâm.
   - **MEP 5 (Streaming):** Dòng dữ liệu liên tục được truyền tuần tự theo thời gian thực qua một kết nối mở kéo dài giữa hai đầu cuối.

---

## 3. BasicWebApi — Luyện kỹ thuật Minimal API (Bước 8)

### 8.1. Build và chạy API trên macOS
Biên dịch dự án `BasicWebApi` và khởi chạy lắng nghe tại địa chỉ `http://localhost:5081`:

![Build thành công và BasicWebApi chạy tại localhost:5081](docs/images/basicwebapi-build-run.png)

---

### 8.3. Thử nghiệm REST Client (Kiểm thử 200, 201, 400)

1. **Kiểm tra API đang chạy — `200 OK`:**
   - Gửi request: `GET http://localhost:5081/`
   - Nhận phản hồi: `200 OK`, JSON `{"application":"BasicWebApi","message":"ASP.NET Core Web API is running."}`.
   ![GET / trả về 200 OK](docs/images/basicwebapi-200-ok.png)

2. **Tạo lời chào hợp lệ — `201 Created`:**
   - Gửi request: `POST http://localhost:5081/greetings` kèm body `{"name":"Binh"}`.
   - Nhận phản hồi: `201 Created`, header `Location: /greetings/1`, body chứa đối tượng gồm `id: 1`, `name: "Binh"`, `createdAt`.
   ![POST /greetings hợp lệ trả về 201 Created](docs/images/basicwebapi-201-created.png)

3. **Kiểm tra dữ liệu rỗng — `400 Bad Request`:**
   - Gửi request: `POST http://localhost:5081/greetings` kèm body `{"name":""}`.
   - Nhận phản hồi: `400 Bad Request`, body `{"message":"Name must not be empty."}`.
   ![POST /greetings tên rỗng trả về 400 Bad Request](docs/images/basicwebapi-400-bad-request.png)

---

### 8.4. Trả lời câu hỏi đọc hiểu mã nguồn

**Câu 1: Hai dòng `builder.Services.AddSingleton<GreetingService>()` và `var app = builder.Build();`, dòng nào phải đứng trước? Vì sao?**
- **Trả lời:** Dòng `builder.Services.AddSingleton<GreetingService>()` bắt buộc phải đứng trước `var app = builder.Build()`.
- **Giải thích:** ASP.NET Core áp dụng quy trình 2 pha tách biệt: Pha 1 đăng ký dịch vụ vào container DI (`IServiceCollection`), Pha 2 gọi `Build()` để biên dịch thành đối tượng ứng dụng `WebApplication` và hoàn tất tạo bộ phân giải dịch vụ `IServiceProvider`. Sau khi gọi `Build()`, tập hợp dịch vụ bị đóng băng (read-only). Nếu gọi đăng ký sau `Build()`, dịch vụ không tồn tại trong container DI, dẫn đến lỗi ngoại lệ khi endpoint POST cố gắng tiêm `GreetingService`. Từ khóa `Singleton` đảm bảo cả ứng dụng chỉ dùng chung một instance duy nhất.

**Câu 2: Trong `/hello/{name}?language=en`, tham số `name` lấy từ đâu, `language` lấy từ đâu?**
- **Trả lời:**
  - `name`: Được trích xuất từ **Route Data** (khớp với vị trí `{name}` trên đường dẫn URL).
  - `language`: Được trích xuất từ **Query String** (phần tham số nằm sau dấu `?`).
- **Giải thích:** Minimal API tự động ánh xạ thông minh (model binding): các tham số khớp với tên biến trong chuỗi định tuyến URL được ưu tiên lấy từ route; các tham số nguyên thủy còn lại được tự động đọc từ chuỗi truy vấn (query string).

**Câu 3: Vì sao POST hợp lệ trả `Results.Created` (201) chứ không phải `Results.Ok` (200)?**
- **Trả lời:**
  - Theo chuẩn ngữ nghĩa HTTP (RFC 9110): `200 OK` chỉ cho biết yêu cầu được xử lý thành công nói chung, còn `201 Created` xác nhận cụ thể rằng một tài nguyên mới đã được tạo thành công trên server.
  - Khi dùng `Results.Created($"/greetings/{greeting.Id}", greeting)`:
    1. Server trả mã trạng thái `201 Created`.
    2. Tự động sinh header phản hồi chuẩn `Location: /greetings/1`, thông báo cho client biết URI của tài nguyên vừa tạo.
    3. Đóng gói đối tượng tài nguyên vừa tạo trong body của response.

---

### 8.5. Tự làm — Bổ sung ngôn ngữ tiếng Nhật (`language=ja`)

Cập nhật endpoint trong [Program.cs](file:///Users/nguyenthanhson/Downloads/PTUDPT-K23C/Nguyễn%20Thanh%20Sơn%20-%20dtc245200486/Bai1/BasicWebApi/src/BasicWebApi/Program.cs):
```csharp
app.MapGet("/hello/{name}", (string name, string? language) => {
    var message = string.Equals(language, "en", StringComparison.OrdinalIgnoreCase)
        ? $"Hello, {name}!"
        : string.Equals(language, "ja", StringComparison.OrdinalIgnoreCase)
            ? $"Konnichiwa, {name}!"
            : $"Xin chào, {name}!";

    return Results.Ok(new GreetingResponse(message));
});
```
Kiểm tra trên trình duyệt với `http://localhost:5081/hello/An?language=ja` trả về `{"message":"Konnichiwa, An!"}`:

![Kiểm tra language=ja trên Google Chrome macOS](docs/images/basicwebapi-8-5-tieng-nhat.png)

---

### 8.6. Debug và quan sát dữ liệu trong bộ nhớ

1. Đặt điểm dừng (breakpoint) tại dòng:
   `var greeting = service.Create(request.Name.Trim());`
2. Chạy debug trên VS Code, gửi request POST với tên `"Binh"`.
3. Quan sát biến tại breakpoint:
   - Biến `request.Name` mang giá trị `"Binh"`.
   - Bấm **F10** (Step Over) để thực thi lệnh tạo, đối tượng `greeting` được khởi tạo thành công với `Id = 1`, `Name = "Binh"`, `CreatedAt` mang mốc thời gian hiện tại.
   - Bấm **F5** để tiếp tục luồng thực thi và trả về `201 Created`.

![Debug BasicWebApi trên VS Code macOS](docs/images/basicwebapi-8-6-debug.png)

---

## 4. MiniDistributedSystem — Hai tiến trình giao tiếp qua mạng (Bước 9)

### Trả lời câu hỏi:
1. **Client gọi những endpoint nào của API? Mỗi lần gọi là một cặp request–response gồm method, URL, mã trạng thái gì?**
   - Lần gọi 1: `GET http://localhost:5000/api/assignments` &rarr; Mã trạng thái: `200 OK`.
   - Lần gọi 2: `POST http://localhost:5000/api/submissions` &rarr; Mã trạng thái: `201 Created` (kèm header `Location`).
   - Lần gọi 3 (mở rộng): `GET http://localhost:5000/api/submissions/{id}` &rarr; Mã trạng thái: `200 OK`.

2. **Client có tham chiếu (dùng chung) code của API không? Hai bên "thống nhất" với nhau bằng gì?**
   - **Không**, Client hoàn toàn độc lập, không thêm bất kỳ Project Reference nào tới API.
   - Hai bên thống nhất với nhau qua **Hợp đồng giao tiếp (Contract)** thông qua chuẩn giao thức HTTP và định dạng trao đổi dữ liệu JSON. Mỗi bên tự định nghĩa các record dữ liệu phù hợp với cấu trúc JSON đó.

3. **Vì sao địa chỉ API được truyền qua tham số `-- http://localhost:5000` mà không viết cứng trong code?**
   - Để đảm bảo tính lỏng lẻo (Loose Coupling) và khả năng cấu hình linh hoạt. Khi triển khai lên các môi trường khác nhau (Development, Staging, Production) hoặc khi đổi cổng mạng, chỉ cần đổi tham số cấu hình mà không phải chỉnh sửa mã nguồn và biên dịch lại ứng dụng.

---

## 5. Thí nghiệm lỗi — Zero-Trust & Khả năng chịu lỗi (Bước 10)

| # | Thao tác | Dự đoán (TRƯỚC khi chạy) | Kết quả thực tế | Đúng/Sai | Giải thích nguyên nhân |
|---|---|---|---|---|---|
| **a** | Thêm bài tập thứ 3 vào mảng `assignments` trên API, chạy lại API rồi chạy Client | Client tự động hiển thị bài tập thứ 3 mà không cần sửa code Client | Client in ra đầy đủ 3 bài tập: `1: Bai tap 1`, `2: Bai tap 2`, `3: Bai tap 3` | **Đúng** | Vì Client dùng `GetFromJsonAsync<List<Assignment>>()`, dữ liệu được phân tích động từ mảng JSON trả về của API tại thời điểm chạy. Hai bên độc lập mã nguồn. |
| **b** | Tắt API (Ctrl+C), sau đó chạy Client | Client không kết nối được và báo lỗi mạng | Ném ngoại lệ `HttpRequestException: Connection refused (localhost:5000)` | **Đúng** | Giao tiếp phân tán phụ thuộc vào mạng và tính sẵn sàng của tiến trình máy chủ. Gọi nội bộ cùng bộ nhớ thì code luôn chạy, còn gọi qua mạng luôn tiềm ẩn rủi ro máy chủ ngừng hoạt động hoặc mất kết nối. |
| **c** | Đổi `StudentCode: ""` trên Client rồi chạy lại | API từ chối với mã lỗi 400, Client dừng do `EnsureSuccessStatusCode` | API log `400 Bad Request`. Client ném lỗi `HttpRequestException: Response status code does not indicate success: 400 (Bad Request)` | **Đúng** | Lệnh `EnsureSuccessStatusCode()` tự động ném ngoại lệ nếu mã HTTP &ge; 400. Máy chủ bắt buộc phải tự xác thực lại dữ liệu (Zero-Trust Security) vì không thể kiểm soát hay tin tưởng client gửi gì lên. |
| **d** | Đổi cổng API sang 5050 (`--urls http://localhost:5050`), Client vẫn gọi 5000 | Client báo lỗi Connection refused tại cổng 5000 | Ném ngoại lệ `HttpRequestException: Connection refused` tại cổng 5000 | **Đúng** | Khắc phục mà không sửa code: Truyền lại tham số URL cho Client khi chạy: `dotnet run --project src/Submission.Client -- http://localhost:5050`. |

---

## 6. Dự án EduSubmit tự làm & Bài mở rộng (Bước 11 - Bước 12)

### 6.1. Kiến trúc hệ thống
- Hai tiến trình độc lập hoàn toàn: `Submission.Api` (Máy chủ) và `Submission.Client` (Máy khách).
- Không chia sẻ mã nguồn chung, giao tiếp hoàn toàn qua chuẩn RESTful HTTP.

### 6.2. Triển khai bài mở rộng (Bước 12 — Đạt điểm 10 tối đa)
1. **Lưu trữ bài nộp tại API:** Khởi tạo danh sách `var submissions = new List<SubmissionResult>();` để lưu trữ trạng thái các lượt nộp bài.
2. **Endpoint truy vấn theo mã định danh:**
   - Bổ sung `GET /api/submissions/{id:guid}`: Trả về `200 OK` nếu tìm thấy, hoặc `404 Not Found` nếu không tồn tại mã lượt nộp.
3. **Client xác minh:**
   - Sau khi gửi POST thành công, Client tự động gọi `GET /api/submissions/{result.Id}` để kiểm chứng lại thông tin đã lưu trên server.

```csharp
// Endpoint mở rộng trên API:
app.MapGet("/api/submissions/{id:guid}", (Guid id) => {
    var item = submissions.FirstOrDefault(s => s.Id == id);
    return item is not null
        ? Results.Ok(item)
        : Results.NotFound(new { message = $"Khong tim thay luot nop: {id}" });
});
```

### 6.3. Sơ đồ luồng trao đổi thông điệp (MEP)
```text
Client --[ HTTP GET  /api/assignments ]---------> Api : Request - Response (Lấy danh sách bài tập, trả về 200 OK)
Client --[ HTTP POST /api/submissions ]---------> Api : Request - Response (Gửi bài nộp, server lưu bộ nhớ và trả 201 Created + Header Location)
Client --[ HTTP GET  /api/submissions/{id} ]----> Api : Request - Response (Đọc lại thông tin lượt nộp, trả về 200 OK)
```

---

## 7. Khó khăn gặp phải và giải pháp

1. **Xung đột cổng mạng (Address already in use):**
   - *Nguyên nhân:* Tiến trình cũ vẫn đang chạy ngầm trên cổng `5081` hoặc `5000`.
   - *Cách giải quyết:* Dùng lệnh `lsof -i :5081` để xác định PID và gọi `kill -9 <PID>` giải phóng cổng trước khi chạy lại.
2. **Khác biệt nền tảng hệ điều hành:**
   - Khi chạy trên macOS, sử dụng đường dẫn zsh chuẩn Unix, cấu hình đường dẫn cài đặt `.NET 10.0 SDK` tại `~/.dotnet/dotnet`, đảm bảo các lệnh hoạt động chuẩn xác không phụ thuộc môi trường Windows.
