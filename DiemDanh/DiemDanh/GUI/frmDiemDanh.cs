using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using System.IO;
using System.Diagnostics;
using DiemDanh.Entity;
using DirectShowLib;
namespace DiemDanh.GUI
{
    public partial class frmDiemDanh : Form
    {

        Capture capture;  // camera
        Image<Bgr, Byte> ImageFrame;
        Image<Gray, Byte> objFace;
        List<SinhVien> listSV;//= new List<SinhVien>() ;
        List<Image<Gray, byte>> listImg = new List<Image<Gray, byte>>();
        List<string> listID = new List<string>();
        HaarCascade haar;
        string ID;
        DsDevice[] multiCam;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5d, 0.5d);
        public void ProcessFrame(object sender, EventArgs arg)
        {
           
                ImageFrame = capture.QueryFrame();
                imgCamera.Image = ImageFrame;
                FaceRecognition();
        }
        public frmDiemDanh()
        {
            InitializeComponent();
            

        }
        
        public frmDiemDanh( ref List<SinhVien> _listSV, ref List<Image<Gray, byte>> _listImg)
        {
            InitializeComponent();
            this.listSV = _listSV;
            this.listImg = _listImg;
            showVangMat();
            multiCam = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (multiCam.Length != 0)
            {
                for (int i = 0; i < multiCam.Length; i++)
                {
                    string camName = (i + 1) + ":" + multiCam[i].Name;
                    cbCamIndex.Items.Add(camName);
                }
            }
        }


        public void FaceRecognition()
        {
            foreach (SinhVien sv in listSV)
            {
                ID = sv.ID;
                listID.Add(ID);
            } 
            if (ImageFrame != null)
            {
                 Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                 var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                 Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                 new Size(25, 25))[0];


                foreach(var f in faces)
                {


                    imgTrain.Image = ImageFrame.Copy(f.rect).Convert<Bgr, Byte>().Resize(148,
                                                        161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); // hien thi 1 khuon mat len imgTrain => nhap thong tin
                     objFace= ImageFrame.Copy(f.rect).Convert<Gray, Byte>().Resize(148,
                                                        161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    ImageFrame.Draw(f.rect, new Bgr(Color.Red), 3); // danh dau khuon mat phat hien
                    if(listSV.Count!= 0)
                    {
                        MCvTermCriteria term = new MCvTermCriteria(listSV.Count, 0.001);
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(listImg.ToArray(),listID.ToArray(),ref term);
                        string sv;
                        sv = recognizer.Recognize(objFace);
                        showInfo(sv);
                        ImageFrame.Draw(sv, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                        
                    }

                }
     
                 
/*
                //Get the current frame form capture device
                // currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Convert it to Grayscale
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();

                //Face Detector
                MCvAvgComp[][] facesDetected = grayFrame.DetectHaarCascade(
                                          haar,  1.2,  10,
                                          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                          new Size(20, 20));
                foreach (MCvAvgComp f in facesDetected[0])
                {

                    objFace = grayFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //draw the face detected in the 0th (gray) channel with blue color
                    grayFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                    if (listImg.ToArray().Length != 0)
                    {
                        //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Eigen face recognizer
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           listImg.ToArray(),
                           listSV.ToArray(),
                           3000,
                           ref termCrit);
                        grayFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                    }
                }
                

    */
            }
        }
        public void showInfo(string _ID)
        {
            if(_ID.Equals(""))
            {
                txtMaSV.Text = "";
                txtHoTen.Text = "";
                txtLop.Text = "";
                txtNgaySinh.Text = "";
                radNam.Checked = false;
                radNu.Checked = false;
            } else
                foreach (SinhVien sv in listSV)
                {
                    if(sv.ID.Equals(_ID))
                    {
                        txtMaSV.Text = sv.ID;
                        txtHoTen.Text = sv.HoTen;
                        txtLop.Text = sv.Lop;
                        txtNgaySinh.Text = sv.NgaySinh.ToString();
                        if (sv.GioiTinh==true)
                            radNam.Checked = true;
                        else
                            radNu.Checked = true;
                        break;
                    }
                }
        }
        public void showVangMat()
        {
            int stt = 1;
            foreach(SinhVien sv in listSV)
            {
                ListViewItem item = new ListViewItem(stt.ToString());
                ListViewItem.ListViewSubItem sub_item = new ListViewItem.ListViewSubItem(item, sv.ID);
                ListViewItem.ListViewSubItem sub_item_1 = new ListViewItem.ListViewSubItem(item, sv.HoTen);
                ListViewItem.ListViewSubItem sub_item_2 = new ListViewItem.ListViewSubItem(item, sv.Lop);
                
                ListViewItem.ListViewSubItem sub_item_3 = new ListViewItem.ListViewSubItem(item, sv.NgaySinh.ToString());
                ListViewItem.ListViewSubItem sub_item_4 = new ListViewItem.ListViewSubItem(item, (sv.GioiTinh == true ? "Nam" : "Nữ"));
                
                item.SubItems.Add(sub_item);
                item.SubItems.Add(sub_item_1);
                item.SubItems.Add(sub_item_2);
                item.SubItems.Add(sub_item_3);
                item.SubItems.Add(sub_item_4);
                // item.SubItems.Add(sub_item_5);

                lsvVangMat.Items.Add(item);
                stt++;
                lsvVangMat.Refresh();
            }
        }

        private void frmDiemDanh_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_default.xml");
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {

                capture = null;
            }

            try
            {
                capture = new Capture(cbCamIndex.SelectedIndex);
                Application.Idle += ProcessFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //if (capture == null)
            //{
            //    try
            //    {
            //        capture = new Capture();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}


            //if (capture != null)
            //{
            //    Application.Idle += ProcessFrame;
            //}
        }

        private void frmDiemDanh_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            ImageFrame = null;
        }
    }
}
