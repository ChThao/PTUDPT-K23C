# Bài 1 — Tổng quan hệ thống và các mô hình trao đổi thông điệp (MEP)

- **Họ và tên:** Nguyễn Thanh Sơn
- **MSSV:** DTC245200486
- **Lớp:** CNTT K23C
- **Nội dung hoàn thành:** Đầy đủ từ Mục 1 đến Mục 13 (bao gồm bài mở rộng Bước 12 để đạt điểm tối đa 10/10).

---

## Cấu trúc thư mục Bài 1:
```text
Bai1/
├── BasicWebApi/            # Dự án luyện tập Minimal API (Mục 8.1 - 8.6)
├── MiniDistributedSystem/  # Hệ thống phân tán EduSubmit (Mục 9 - 12)
│   ├── src/Submission.Api/     # Máy chủ API (kèm endpoint mở rộng Bước 12)
│   └── src/Submission.Client/  # Máy khách Client (kèm xác minh mở rộng)
├── MepDemo/                # Chương trình mô phỏng 5 mô hình MEP (Mục 7)
└── docs/
    ├── bai1.md             # Báo cáo thực hành chi tiết toàn bộ các bước
    └── images/             # Toàn bộ ảnh chụp minh chứng thực nghiệm
```

## Chạy dự án:
```bash
# 1. Chạy BasicWebApi (cổng 5081)
dotnet run --project BasicWebApi/src/BasicWebApi --urls http://localhost:5081

# 2. Chạy MiniDistributedSystem - Máy chủ (cổng 5000)
dotnet run --project MiniDistributedSystem/src/Submission.Api --urls http://localhost:5000

# 3. Chạy MiniDistributedSystem - Máy khách
dotnet run --project MiniDistributedSystem/src/Submission.Client -- http://localhost:5000
```
