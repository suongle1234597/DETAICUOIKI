﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSDLPT
{
    public partial class frmMonHoc : Form
    {
        String status = "";

        public frmMonHoc()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
        }

        private void mONHOCBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsMonHoc.EndEdit(); //ket thuc chinh sua tren nay
            this.tableAdapterManager.UpdateAll(this.dS);
        }

        private void frmMonHoc_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.MONHOC' table. You can move, or remove it, as needed.
            this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
            this.mONHOCTableAdapter.Fill(this.dS.MONHOC);  //lenh tai ve
            // TODO: This line of code loads data into the 'dS.BODE' table. You can move, or remove it, as needed.
            this.bODETableAdapter.Connection.ConnectionString = Program.connstr;
            this.bODETableAdapter.Fill(this.dS.BODE);
            // TODO: This line of code loads data into the 'dS.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
            this.gIAOVIEN_DANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIAOVIEN_DANGKYTableAdapter.Fill(this.dS.GIAOVIEN_DANGKY);
            
            cmbCoSo.DataSource = Program.bds_dspm;
            cmbCoSo.DisplayMember = "TENCS";
            cmbCoSo.ValueMember = "TENSERVER";
            cmbCoSo.SelectedIndex = Program.mCoso;

            if (Program.mGroup == "Truong")
            {
                panelControl1.Enabled = false;
                gcMonHoc.Enabled = false;
                gcMonHoc.UseDisabledStatePainter = false; //khong phan biet mau
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnPhucHoi.Enabled = false;
                btnRefresh.Enabled = false;
                btnThoat.Enabled = true;
                btnGhi.Enabled = false;
                cmbCoSo.Enabled = true;
            }
            else
            {
                //gcMonHoc.Enabled = false;
                //gcMonHoc.UseDisabledStatePainter = false; //khong phan biet mau
                btnGhi.Enabled = false;
                cmbCoSo.Enabled = false;
            }
            panelControl1.Enabled = false;
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            panelControl1.Enabled = true;

            gcMonHoc.Enabled = false;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnPhucHoi.Enabled = true;
            btnRefresh.Enabled = false;
            btnThoat.Enabled = false;
            btnGhi.Enabled = true;
            txtMaMH.Enabled = true;

            this.bdsMonHoc.AddNew(); //Them mot muc moi vao danh sach
            txtMaMH.Focus(); //dieu khien con tro toi o textbox
            status = "Them";
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            status = "Sua";
            panelControl1.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThoat.Enabled = false;
            btnGhi.Enabled = true;
            btnPhucHoi.Enabled = true;
            btnRefresh.Enabled = false;
            gcMonHoc.Enabled = false;
            txtMaMH.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (status.Equals("Them"))
            {
                if (txtMaMH.Text.Trim() == "")
                {
                    MessageBox.Show("Mã Môn học không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMaMH.Focus();
                    return;
                }
                else if (txtMaMH.Text.Length > 5)
                {
                    MessageBox.Show("Mã Môn học chỉ được 5 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMaMH.Focus();
                    return;
                }

                string strLenh = "EXEC SP_TimKiemMH '" + txtMaMH.Text + "'";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                Program.myReader.Read();
                int kq = Int32.Parse(Program.myReader.GetInt32(0).ToString());
                Program.myReader.Close();
                Program.conn.Close();

                if (kq == 1)
                {
                    MessageBox.Show("Mã Môn học đã tồn tại. Mời nhập mã môn học khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMaMH.Focus();
                    return;
                }
            }

            if (txtTenMH.Text.Trim() == "")
            {
                MessageBox.Show("Tên Môn học không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTenMH.Focus();
                return;
            }
            else if (txtTenMH.Text.Length > 40)
            {
                MessageBox.Show("Tên Môn học chỉ được 40 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTenMH.Focus();
                return;
            }

            string strLenh1 = "EXEC SP_TimKiemTenMH N'" + txtTenMH.Text + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh1);
            Program.myReader.Read();
            int kq1 = Int32.Parse(Program.myReader.GetInt32(0).ToString());
            Program.myReader.Close();
            Program.conn.Close();

            if (kq1 == 1)
            {
                MessageBox.Show("Tên Môn học không được trùng. Mời nhập tên môn học khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTenMH.Focus();
                return;
            }

            try
            {
                bdsMonHoc.EndEdit(); //ket thuc qua trinh hieu chinh
                bdsMonHoc.ResetCurrentItem(); // lay du lieu hien tai day ve co so du lieu
                this.mONHOCTableAdapter.Update(this.dS.MONHOC);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi môn học", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            panelControl1.Enabled = false;
            btnGhi.Enabled = false;
            btnPhucHoi.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnRefresh.Enabled = true;
            btnThoat.Enabled = true;
            gcMonHoc.Enabled = true;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bdsBoDe.Count > 0)
            {
                MessageBox.Show("Môn học đã có bộ đề, không thể xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (bdsGVDK.Count > 0)
            {
                MessageBox.Show("Môn học có đăng kí thi nên không được xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (txtMaMH.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng chọn Môn học cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (bdsMonHoc.Count > 0)
            {
                if (MessageBox.Show("Bạn có thật sự muốn xóa Môn học này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                {
                    try
                    {
                        bdsMonHoc.RemoveCurrent();
                        this.mONHOCTableAdapter.Update(this.dS.MONHOC);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa Môn học " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }

            if (bdsMonHoc.Count == 0)
            {
                btnXoa.Enabled = false;
            }

            gcMonHoc.Enabled = true;
            panelControl1.Enabled = false;
            btnGhi.Enabled = false;
            btnPhucHoi.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnRefresh.Enabled = true;
            btnThoat.Enabled = true;
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsMonHoc.CancelEdit(); //huy chinh sua tren hang
            gcMonHoc.Enabled = true;
            panelControl1.Enabled = false;
            btnGhi.Enabled = false;
            btnPhucHoi.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnRefresh.Enabled = true;
            btnThoat.Enabled = true;
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dS.EnforceConstraints = false; //cac quy tac khong duoc thi hanh
            this.mONHOCTableAdapter.Fill(this.dS.MONHOC);
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát Form Môn học không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void cmbCoSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCoSo.SelectedValue.ToString() == "System.Data.DataRowView") return;
                Program.servername = cmbCoSo.SelectedValue.ToString();
            }
            catch (Exception) { };
            if (cmbCoSo.SelectedIndex != Program.mCoso)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }

            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Lỗi kết nối về cơ sở mới", "", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.mONHOCTableAdapter.Fill(this.dS.MONHOC);
                }
                catch (Exception ex) { }
            }
        }
    }
}
