using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiemDanh.GUI
{
    public partial class frmDmSinhVien : Form
    {
        int stt2 = 1;
        public frmDmSinhVien()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void gbContainer_Enter(object sender, EventArgs e)
        {

        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close(); // đóng tạm thời
            this.Dispose(); // thu hồi toàn bộ tài nguyên
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

            //bước 1: kiểm tra dữ liệu
            if(txtMaSV.Text.Trim()=="")  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập mã sinh viên");
                ActiveControl = txtMaSV;
                return;
                
            }
           // int _id = 0;
           // _id = Convert.ToInt16(txtMaSV.Text);


            //Bước 2: tạo đối tượng
            Entity.SinhVien obj = new Entity.SinhVien();
            obj.MaSV = txtMaSV.Text.Trim();
            obj.HoTen = txtHoTen.Text.Trim();
            obj.Lop = txtLop.Text.Trim();
            //Bước 3: hiển thị

            ListViewItem item = new ListViewItem(stt2.ToString());
            ListViewItem.ListViewSubItem sub_item = new ListViewItem.ListViewSubItem(item, txtMaSV.Text);
            ListViewItem.ListViewSubItem sub_item_1 = new ListViewItem.ListViewSubItem(item, txtHoTen.Text);
            ListViewItem.ListViewSubItem sub_item_2 = new ListViewItem.ListViewSubItem(item, txtLop.Text);
            ListViewItem.ListViewSubItem sub_item_3 = new ListViewItem.ListViewSubItem(item, txtNgaySinh.Text);
            ListViewItem.ListViewSubItem sub_item_4 = new ListViewItem.ListViewSubItem(item, (rdbNam.Checked==true?"Nam":"Nu"));
            ListViewItem.ListViewSubItem sub_item_5 = new ListViewItem.ListViewSubItem(item, txtGhiChu.Text);

            /*  item.SubItems[0].Text = stt2.ToString();
              item.SubItems[1].Text = txtMaSV.Text.Trim();
              item.SubItems[2].Text = txtHoTen.Text.Trim();
              item.SubItems[3].Text = txtLop.Text.Trim(); */
            item.SubItems.Add(sub_item);
            item.SubItems.Add(sub_item_1);
            item.SubItems.Add(sub_item_2);
            item.SubItems.Add(sub_item_3);
            item.SubItems.Add(sub_item_4);
            item.SubItems.Add(sub_item_5);

            lsvSinhVien.Items.Add(item);
            stt2++;
            lsvSinhVien.Refresh();
        }
    }
}
