using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiemDanh.GUI;
using DiemDanh.Entity;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
namespace DiemDanh
{
    public partial class frmMain : Form
    {
        //List<DiemDanh.Entity.SinhVien> listSV;
        List<SinhVien> listSV= new List<SinhVien>();
        List<Image<Gray, byte>> listImg = new List<Image<Gray, byte>>();
        public frmMain()
        {
            InitializeComponent();
            lsvDanhSach.Dispose();
        }


        
        private void điểmDanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDiemDanh frm = new frmDiemDanh(ref listSV, ref listImg);
            frm.ShowDialog();
        }

        private void mởToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFaceDetection frm = new frmFaceDetection();
            frm.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmFaceDetection frm = new frmFaceDetection(ref listSV, ref listImg);
            frm.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            openFile.ShowDialog();
            string file = openFile.FileName;
            
        }

        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUI.frmDmSinhVien frm = new frmDmSinhVien();
            frm.ShowDialog();
        }

        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
