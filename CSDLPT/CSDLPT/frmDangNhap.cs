﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSDLPT
{
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.V_DSPM' table. You can move, or remove it, as needed.
            this.v_DSPMTableAdapter.Fill(this.dS.V_DSPM);
            // TODO: This line of code loads data into the 'dS.V_DSPM' table. You can move, or remove it, as needed.
            this.v_DSPMTableAdapter.Fill(this.dS.V_DSPM);
            // TODO: This line of code loads data into the 'tRACNGHIEMDataSet.V_DS_PHANMANH' table. You can move, or remove it, as needed.
            string chuoiKetNoi = "Data Source=SUONG;Initial Catalog=" + Program.database + ";Integrated Security=True"; //Ket noi ve site chu k can password, k can tai khoan
            Program.conn.ConnectionString = chuoiKetNoi;
            //co kha nang ket noi k thanh conh. phai co try catch
            //Program.conn.Open(); //mo ket noi .Kiem tra ben ExecSqlDataTable roi

            //Goi cai view tra ve DataTable
            DataTable dt = new DataTable();
            dt = Program.ExecSqlDataTable("SELECT * FROM V_DSPM"); //dau cai chuoi lenh nay vao cai ham nay, ham nay thuc thi lenh, roi tra ve DataTable
            Program.bds_dspm.DataSource = dt;
            //lien ket giua bindingSource với combobox
            cmbCoSo.DataSource = dt;
            cmbCoSo.DisplayMember = "TENCS"; //ten cot muon hien len
            cmbCoSo.ValueMember = "TENSERVER"; //gia tri muon hien len

            cmbCoSo.SelectedIndex = 0;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if(txtTaiKhoan.Text.Trim() == "" || txtMatKhau.Text.Trim() == "")
            {
                MessageBox.Show("Tài khoản và Mật khẩu không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            Program.mCoso = cmbCoSo.SelectedIndex;
            string strLenh = "";
            if (rdbSinhVien.Checked)
            {
                Program.username = txtTaiKhoan.Text;
                Program.mlogin = "sv";
                Program.password = txtMatKhau.Text;
                if (Program.KetNoi() == 0) return;
                Program.mloginDN = "sv";
                Program.passwordDN = Program.password;

                strLenh = "EXEC dbo.SP_DangNhapSinhVien '" + Program.mloginDN + "', '" + Program.username + "'";
            }
            else if (rdbGiangVien.Checked)
            {
                Program.mlogin = txtTaiKhoan.Text;
                Program.password = txtMatKhau.Text;
                if (Program.KetNoi() == 0) return;
                Program.mloginDN = Program.mlogin;
                Program.passwordDN = Program.password;
                strLenh = "EXEC dbo.SP_DangNhapGiangVien '" + Program.mlogin + "'";
            }

            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;
            Program.myReader.Read();

            if (rdbSinhVien.Checked)
            {
                if (Program.myReader.GetString(1).Equals("NULL"))
                {
                    MessageBox.Show("Đăng nhập thất bại\n Bạn xem lại tài khoản và mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            else if(rdbGiangVien.Checked)
            {
                if(Program.myReader.GetString(2).Equals("Sinhvien"))
                {
                    MessageBox.Show("Đăng nhập thất bại\n Bạn xem lại tài khoản và mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }

            Program.username = Program.myReader.GetString(0); // Lay username
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Tài khoản bạn nhập không có quyền truy cập dữ liệu\n Bạn xem lại tài khoản và mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            
            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.conn.Close();

            if (Program.mGroup.Equals("Sinhvien"))
            {
                string strLenh1 = "EXEC SP_ThongTinSV '" + Program.username + "'";
                Program.myReader = Program.ExecSqlDataReader(strLenh1);
                if (Program.myReader == null) return;
                Program.myReader.Read();
                Program.mMaLop = Program.myReader.GetString(0);
                Program.mLop = Program.myReader.GetString(1);
                Program.myReader.Close();
                Program.conn.Close();

                frmThi sv = new frmThi();
                sv.ShowDialog();
                //Program.frmDN.Hide();
            }
            else
            {
                frmGiaoDienChinh form = new frmGiaoDienChinh();
                form.ShowDialog();
                //Program.frmDN.Hide();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes) {
                this.Close();
            }
        }

        private void cmbCoSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.servername = cmbCoSo.SelectedValue.ToString();
            }
            catch(Exception) { };
        }
    }
}
