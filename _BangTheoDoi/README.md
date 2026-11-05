# Bảng theo dõi nộp bài

- `TheoDoi_BaiN.xlsx`: tình trạng nộp (Đúng hạn / Muộn / Quá hạn / Chưa nộp) và điểm từng tiêu chí của mỗi sinh viên.
  GitHub tự tạo lại file này 30 phút một lần và mỗi khi có thay đổi trên nhánh main (`.github/workflows/bang-theo-doi.yml`).
  Bài đã gửi Pull Request nhưng chưa được gộp cũng được tính, có ghi chú "CHỜ CÔ GỘP".
- `DiemGV_BaiN.csv`: **giảng viên nhập điểm tại đây** (bấm vào file → biểu tượng bút chì → sửa → Commit changes).
  Ô để trống = dùng điểm máy gợi ý. Cột `Chay_duoc_4` máy không tự chấm được, giảng viên nhập sau khi chạy thử.
  Bảng Excel sẽ tự cập nhật theo điểm đã nhập sau khoảng 1 phút.
- `TheoDoi_BaiN.csv`: bản sao dạng chữ của bảng, chỉ dùng để so sánh thay đổi.

Điểm máy gợi ý chỉ là tham khảo; điểm chính thức là điểm giảng viên nhập.
