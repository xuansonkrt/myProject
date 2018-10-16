using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiemDanh.GUI;
using DiemDanh.Entity;

namespace DiemDanh
{
    static class Program
    {

       // List<SinhVien> lsv = new List<SinhVien>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
           // List<DiemDanh.Entity.SinhVien> listSV;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            //Application.Run(new frmDmSinhVien());
            //Application.Run(new frmDmKhoaHocc());           
        }
    }
}
