using DiemDanh;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace DiemDanh.Entity
{
    class SinhVien
    {
        private Image<Bgr, Byte> _Img;
        private string _MaSV;
        private string _HoTen;
        private string _Lop;
        private DateTime _NgaySinh;
        private bool _GioiTinh;
       
        
        public SinhVien()
        {

        }

        public SinhVien(string MaSV, string HoTen, string Lop, DateTime NgaySinh, bool GioiTinh)
        {
            _MaSV = MaSV;
            _HoTen = HoTen;
            _Lop = Lop;
            _NgaySinh = NgaySinh;
            _GioiTinh = GioiTinh;
        }
        //ham thuoc tinh
        public string MaSV
        {
            get { return _MaSV;  }
            set {  _MaSV = value;  }
        }

        public string HoTen
        {
            set { _HoTen = value; }
            get { return _HoTen; }
        }
        public string Lop
        {
            set { _Lop = value; }
            get { return _Lop; }
        }
        public DateTime NgaySinh
        { 
            set { _NgaySinh = value; }
            get { return _NgaySinh; }
        }
        public bool GioiTinh
        {
            set { _GioiTinh = value; }
            get { return _GioiTinh; }
        }

        public Image<Bgr, Byte> IMG
        {
            get { return _Img;  }
            set {  _Img = value;}
        }

   /*     public SinhVien ()
        {
            _Img = null;
            _MaSV = "";
          /*  _HoTen = "";
            _Lop = "";
            _NgaySinh = DateTime.Now;
            _GioiTinh = false;*/
       // }*/
      

       
    }
}
