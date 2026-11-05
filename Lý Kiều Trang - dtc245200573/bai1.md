# Bài 1 - Tổng quan hệ thống và MEP
Họ tên: Lý Kiều Trang
MSSV: DTC245200573
Lớp: CNTT K23C

## 1. Cài đặt
- Đã cài đặt môi trường .NET SDK và VS Code thành công.

## 2. MepDemo (Bước 7)
- **(1)** Ở MEP 3, mỗi bài nộp chỉ do **1 worker** xử lý (chia tải luân phiên).
- **(2)** Ở MEP 4, thông điệp được phát tới **tất cả các dịch vụ** (Publish - Subscribe).
- **(3) Mô tả ngắn:**
  - MEP 1 (Request-Response): Gửi yêu cầu và nhận phản hồi trực tiếp.
  - MEP 2 (One-Way): Gửi tin nhắn đi mà không cần chờ phản hồi.
  - MEP 3 (Queue-Based): Đưa tin nhắn vào hàng chờ để các worker lấy ra xử lý.
  - MEP 4 (Publish-Subscribe): Phát sự kiện cho tất cả các bên đăng ký cùng nhận.
  - MEP 5 (Streaming/Push): Truyền dòng dữ liệu liên tục theo thời gian thực.

## 3. BasicWebApi (Bước 8)
- Đã test thử nghiệm API thành công qua REST Client (`requests.http`) / PowerShell.
- **Giải thích:**
  1. `AddSingleton` phải gọi trước `builder.Build()` để đăng ký dependency injection trước khi ứng dụng khởi tạo.
  2. URL `/hello/{name}?language=en` lấy `name` từ route và `language` từ query string.
  3. POST thành công trả về mã `201 Created` kèm header `Location` chỉ định tài nguyên mới tạo.

## 4. MiniDistributedSystem (Bước 9)
- **Giải thích:**
  1. Client gọi endpoint `GET /api/assignments` (200 OK) và `POST /api/submissions` (201 Created).
  2. Client và API không dùng chung code, giao tiếp hoàn toàn qua giao thức HTTP và định dạng JSON.
  3. Địa chỉ truyền qua tham số `-- http://localhost:5000` giúp linh hoạt thay đổi máy chủ khi triển khai mà không cần biên dịch lại code.

## 5. Thí nghiệm lỗi (Bước 10)
| # | Thao tác | Thực tế & Giải thích |
|---|---|---|
| **a** | Thêm bài tập 3 | Client tự hiển thị bài 3 mà không cần sửa code do nhận mảng JSON động. |
| **b** | Tắt API | Lỗi `HttpRequestException` (Connection refused) do tiến trình Server bị ngắt. Gọi qua mạng phụ thuộc hạ tầng, khác gọi hàm nội bộ trong RAM. |
| **c** | StudentCode = "" | API trả `400 Bad Request`. `EnsureSuccessStatusCode()` ném ngoại lệ khi HTTP code không thuộc dải 2xx. Máy chủ luôn phải tự validate dữ liệu đầu vào. |
| **d** | Chạy cổng 5050 | Sửa địa chỉ khi chạy client: `dotnet run --project src/Submission.Client -- http://localhost:5050`. |

## 6. Dự án EduSubmit tự làm (Bước 11-12)
- Khởi tạo thành công Solution `EduSubmit-DTC245200573` chứa 2 project `Submission.Api` và `Submission.Client`.
- Luồng giao tiếp: Client gửi request HTTP GET/POST tới API, API xử lý và phản hồi dữ liệu JSON.

## 7. Khó khăn gặp phải và cách giải quyết
- **Khó khăn:** Nhầm lẫn thư mục khi thực thi lệnh `dotnet run`.
- **Giải quyết:** Sử dụng đúng lệnh `cd` chuyển vào đúng thư mục dự án trước khi thực thi.