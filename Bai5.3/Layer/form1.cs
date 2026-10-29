using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HotelManagement.DAL;

namespace HotelManagement
{
    public partial class Form1 : Form
    {
        HotelDAL dal = new HotelDAL();

        TabControl tabs = new TabControl();

        DataGridView dgvPhong = new DataGridView();
        DataGridView dgvPhieu = new DataGridView();
        DataGridView dgvDichVu = new DataGridView();

        ComboBox cboLoai = new ComboBox();
        ComboBox cboTrangThai = new ComboBox();
        ComboBox cboPhongNhan = new ComboBox();
        ComboBox cboPhieuDV = new ComboBox();
        ComboBox cboDichVu = new ComboBox();
        ComboBox cboPhieuTra = new ComboBox();
        ComboBox cboPhongDon = new ComboBox();

        TextBox txtHoTen = new TextBox();
        TextBox txtCCCD = new TextBox();
        TextBox txtSDT = new TextBox();
        NumericUpDown numSoLuong = new NumericUpDown();

        public Form1()
        {
            InitializeComponent();

            Controls.Clear();
            TaoGiaoDien();
            TaiTatCaDuLieu();
        }

        Label TaoLabel(string text, int x, int y, int width = 120)
        {
            Label lb = new Label();
            lb.Text = text;
            lb.Location = new Point(x, y);
            lb.Size = new Size(width, 28);
            lb.TextAlign = ContentAlignment.MiddleLeft;
            return lb;
        }

        TextBox TaoTextBox(int x, int y, int width = 200)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(width, 28);
            return txt;
        }

        Button TaoButton(string text, int x, int y, int width = 140)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(width, 34);
            return btn;
        }

        DataGridView TaoBang()
        {
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            return dgv;
        }

        void TaoGiaoDien()
        {
            Text = "QUẢN LÝ KHÁCH SẠN";
            Size = new Size(1050, 700);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "QUẢN LÝ KHÁCH SẠN";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.Dock = DockStyle.Top;
            title.Height = 60;
            title.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(title);

            tabs.Dock = DockStyle.Fill;
            Controls.Add(tabs);
            tabs.TabPages.Add(tab);
        }

        void TaoTabDichVu()
        {
            TabPage tab = new TabPage("Sử dụng dịch vụ");

            Panel top = new Panel();
            top.Dock = DockStyle.Top;
            top.Height = 150;

            cboPhieuDV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhieuDV.Location = new Point(150, 15);
            cboPhieuDV.Width = 300;

            cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDichVu.Location = new Point(150, 55);
            cboDichVu.Width = 300;

            numSoLuong.Location = new Point(150, 95);
            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 1000;
            numSoLuong.Value = 1;

            Button btnThem = TaoButton("Thêm dịch vụ", 500, 55, 150);
            btnThem.Click += (s, e) => ThemDichVu();

            top.Controls.Add(TaoLabel("Phiếu đang thuê:", 15, 15, 130));
            top.Controls.Add(cboPhieuDV);
            top.Controls.Add(TaoLabel("Dịch vụ:", 15, 55));
            top.Controls.Add(cboDichVu);
            top.Controls.Add(TaoLabel("Số lượng:", 15, 95));
            top.Controls.Add(numSoLuong);
            top.Controls.Add(btnThem);

            dgvDichVu = TaoBang();

            tab.Controls.Add(dgvDichVu);
            tab.Controls.Add(top);
            tabs.TabPages.Add(tab);
        }

        void TaoTabTraPhong()
        {
            TabPage tab = new TabPage("Trả phòng");

            Panel top = new Panel();
            top.Dock = DockStyle.Top;
            top.Height = 150;

            cboPhieuTra.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhieuTra.Location = new Point(150, 15);
            cboPhieuTra.Width = 350;

            Button btnTra = TaoButton("Thanh toán và trả phòng",
                520, 12, 200);
            btnTra.Click += (s, e) => TraPhong();

            cboPhongDon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhongDon.Location = new Point(150, 65);
            cboPhongDon.Width = 200;

            Button btnDon = TaoButton("Xác nhận đã dọn xong",
                370, 62, 200);
            btnDon.Click += (s, e) => DonPhong();

            top.Controls.Add(TaoLabel("Phiếu đang thuê:", 15, 15, 130));
            top.Controls.Add(cboPhieuTra);
            top.Controls.Add(btnTra);
            top.Controls.Add(TaoLabel("Phòng cần dọn:", 15, 65, 130));
            top.Controls.Add(cboPhongDon);
            top.Controls.Add(btnDon);

            dgvPhieu = TaoBang();

            tab.Controls.Add(dgvPhieu);
            tab.Controls.Add(top);
            tabs.TabPages.Add(tab);
        }

        void TaiTatCaDuLieu()
        {
            try
            {
                DataTable dt = dal.LayDuLieu("SELECT * FROM Phong");
                dgvPhong.DataSource = dt;
                LocPhong();

                DataTable dtDV =
                    dal.LayDuLieu("SELECT * FROM DichVu");
                    dgvDichVu.DataSource = dtDV;

                TaiDanhSachPhongTrong();
                TaiDanhSachPhieu();
                TaiDanhSachDichVu();
                TaiDanhSachPhongDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        void LocPhong()
        {
            if (dgvPhong.DataSource == null)
                return;

            DataTable dt = dgvPhong.DataSource as DataTable;

            if (dt == null)
                return;

            DataView dv = dt.DefaultView;
            string dieuKien = "";

            if (cboLoai.SelectedItem != null &&
                cboLoai.Text != "Tất cả")
            {
                dieuKien = "LoaiPhong = '" +
                    cboLoai.Text.Replace("'", "''") + "'";
            }

            if (cboTrangThai.SelectedItem != null &&
                cboTrangThai.Text != "Tất cả")
            {
                if (dieuKien != "")
                    dieuKien += " AND ";

                dieuKien += "TrangThai = '" +
                    cboTrangThai.Text.Replace("'", "''") + "'";
            }

            dv.RowFilter = dieuKien;
        }

        void TaiDanhSachPhongTrong()
        {
            DataTable dt = dal.LayDuLieu(
                "SELECT SoPhong FROM Phong WHERE TrangThai='Trống'");

            cboPhongNhan.DataSource = dt;
            cboPhongNhan.DisplayMember = "SoPhong";
            cboPhongNhan.ValueMember = "SoPhong";
        }

        void TaiDanhSachPhieu()
        {
            DataTable dt = dal.LayPhieuDangThue();

            cboPhieuDV.DataSource = dt.Copy();
            cboPhieuDV.DisplayMember = "MaPhieu";
            cboPhieuDV.ValueMember = "MaPhieu";

            cboPhieuTra.DataSource = dt.Copy();
            cboPhieuTra.DisplayMember = "MaPhieu";
            cboPhieuTra.ValueMember = "MaPhieu";

            dgvPhieu.DataSource = dt;
        }

        void TaiDanhSachDichVu()
        {
            DataTable dt = dal.LayDuLieu(
                "SELECT MaDV, TenDV, DonGia FROM DichVu");

            cboDichVu.DataSource = dt;
            cboDichVu.DisplayMember = "TenDV";
            cboDichVu.ValueMember = "MaDV";
        }

        void TaiDanhSachPhongDon()
        {
            DataTable dt = dal.LayDuLieu(
                "SELECT SoPhong FROM Phong " +
                "WHERE TrangThai='Đang dọn dẹp'");

            cboPhongDon.DataSource = dt;
            cboPhongDon.DisplayMember = "SoPhong";
            cboPhongDon.ValueMember = "SoPhong";
        }

        void NhanPhong()
        {
            if (cboPhongNhan.SelectedValue == null)
            {
                MessageBox.Show("Không có phòng trống.");
                return;
            }

            if (txtHoTen.Text.Trim() == "" ||
                txtCCCD.Text.Trim() == "")
            {
            MessageBox.Show("Vui lòng nhập họ tên và CCCD.");
                return;
            }

            try
            {
                string kq = dal.NhanPhong(
                    cboPhongNhan.SelectedValue.ToString(),
                    txtHoTen.Text.Trim(),
                    txtCCCD.Text.Trim(),
                    txtSDT.Text.Trim());

                MessageBox.Show(kq);

                txtHoTen.Clear();
                txtCCCD.Clear();
                txtSDT.Clear();

                TaiTatCaDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể nhận phòng: " + ex.Message);
            }
        }

        void ThemDichVu()
        {
            if (cboPhieuDV.SelectedValue == null ||
                cboDichVu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu thuê và dịch vụ.");
                return;
            }

            try
            {
                string kq = dal.ThemDichVu(
                    Convert.ToInt32(cboPhieuDV.SelectedValue),
                    Convert.ToInt32(cboDichVu.SelectedValue),
                    (int)numSoLuong.Value);

                MessageBox.Show(kq);

                TaiDanhSachPhieu();
                TaiTatCaDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm dịch vụ: " + ex.Message);
            }
        }

        void TraPhong()
        {
            if (cboPhieuTra.SelectedValue == null)
            {
                MessageBox.Show("Không có phiếu đang thuê.");
                return;
            }

            DialogResult hoi = MessageBox.Show(
                "Bạn xác nhận trả phòng và thanh toán?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (hoi != DialogResult.Yes)
                return;

            try
            {
                decimal tongTien = dal.TraPhong(
                    Convert.ToInt32(cboPhieuTra.SelectedValue));

                MessageBox.Show(
                    "Thanh toán thành công!\nTổng tiền: " +
                    tongTien.ToString("N0") + " VNĐ");

                TaiTatCaDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể trả phòng: " + ex.Message);
            }
        }

        void DonPhong()
        {
            if (cboPhongDon.SelectedValue == null)
            {
                MessageBox.Show("Không có phòng đang dọn dẹp.");
                return;
            }

            try
            {
                dal.DonXong(cboPhongDon.SelectedValue.ToString());

                MessageBox.Show("Đã cập nhật phòng về trạng thái Trống.");
                TaiTatCaDuLieu();
            }
            catch (Exception ex)
            {
            MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
