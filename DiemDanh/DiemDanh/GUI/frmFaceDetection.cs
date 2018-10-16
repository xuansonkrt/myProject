using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO;
using System.Diagnostics;
using DiemDanh.Entity;
using System.Drawing.Imaging;
using DirectShowLib;
using System.Globalization;

namespace DiemDanh
{
    public partial class frmFaceDetection : Form
    {
        private Capture capture;         // camera input
        private bool captureInProcess = false;
        private bool picProcess = false;
        private HaarCascade haar; // detector faces
        private Image<Gray, Byte> objFace;
       // private SinhVien sv;
      //  private string fileImg;
        Image<Bgr, Byte> ImageFrame;
        List<SinhVien> listSV = new List<SinhVien>();
        List<Image<Gray, byte>> listImg = new List<Image<Gray, byte>>();
        Image<Bgr, Byte>[] EXFace;
        int FaceNum = 0;
       // int key = 0;
        int stt = 1;
        //  string filePath;
        // Image<Bgr, Byte> imgSV;
        DsDevice[] multiCam;
        public frmFaceDetection(ref List<SinhVien> _listSV, ref List<Image<Gray, byte>> _listImg)
        {
            InitializeComponent();
            this.listSV = _listSV;
            this.listImg = _listImg;
            multiCam = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if(multiCam.Length!=0)
            {
                for (int i = 0; i < multiCam.Length; i++)
                {
                    string camName = (i + 1) + ":" + multiCam[i].Name;
                    cbCamIndex.Items.Add(camName);
                }
            }

        }
        
        public void ProcessFrame(object sender, EventArgs arg)
        {
            if(!picProcess)
            {
                ImageFrame = capture.QueryFrame();
                imgCamera.Image = ImageFrame;
                DetectFaces_cam();
            }
           
        }
       
        public void DetectFaces_pic()
        {
            picProcess = true;
            btChup.Enabled = false;
            if (ImageFrame != null)
            {
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                new Size(25, 25))[0];   // mang chua cac khuon mat phat hien duoc
                                                        /*
                                                            haar: trained data
                                                            1.1: Scale Increase Rate (1.1,1.2,1.3,1.4) càng nhỏ: phát hiện được nhiều khuôn mặt -> chậm hơn. 
                                                            4:  Minimum Neighbors Threshold 0->4  giá trị cao phát hiện chặt chẽ hơn
                                                            CANNY_PRUNING:  (0)
                                                            new Size(25, 25): size of the smallest face to search for. mặc định 25x25
                                                         */

                
                if(faces.Length>0)
                {
                    MessageBox.Show("Số khuôn mặt được phát hiện: " + faces.Length);
    

                    EXFace = new Image<Bgr, Byte>[faces.Length];
                    int i = 0;
                    foreach (var face in faces)
                    {                     
                        EXFace[i] = ImageFrame.Copy(face.rect).Convert<Bgr,Byte>().Resize(148,
                               161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                        i++;
                        ImageFrame.Draw(face.rect, new Bgr(Color.Blue), 3); // danh dau khuon mat phat hien
                    }
                    imgCamera.Image = ImageFrame;
                    
                    imgTrain.Image = EXFace[FaceNum];
                    objFace= EXFace[FaceNum].Convert<Gray,Byte>();
                   // FaceNum++;
                    if ((faces.Length > 1))
                    {
                        bt_next.Enabled = true;
                        bt_previous.Enabled = false;
                    }
                    else
                    {
                        bt_next.Enabled = false;
                        bt_previous.Enabled = false;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Không có khuôn mặt!");
                }

                /*   ImageFrame.Draw(faces[0].rect, new Bgr(Color.Red), 3);
                   imgTrain.Image = ImageFrame.Copy(faces[0].rect).Convert<Bgr, Byte>().Resize(148,
                                                                 161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);*/

                txtMaSV.Focus();
            }
        }

        public void DetectFaces_cam()
        {
            bt_next.Enabled = false;
            bt_previous.Enabled = false;
            if (ImageFrame != null)
            {
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                new Size(25, 25))[0];   // mang chua cac khuon mat phat hien duoc
                                                        /*
                                                            haar: trained data
                                                            1.1: Scale Increase Rate (1.1,1.2,1.3,1.4) càng nhỏ: phát hiện được nhiều khuôn mặt -> chậm hơn. 
                                                            4:  Minimum Neighbors Threshold 0->4  giá trị cao phát hiện chặt chẽ hơn
                                                            CANNY_PRUNING:  (0)
                                                            new Size(25, 25): size of the smallest face to search for. mặc định 25x25
                                                         */

               
                if(faces.Length!=0)
                {
                    
                    btChup.Enabled = true;
                    if (captureInProcess == true)
                        btChup.Enabled = false;
                    //  if(face.rect!=null)
                    if ((captureInProcess == false))
                    {
                        imgTrain.Image = ImageFrame.Copy(faces[0].rect).Convert<Bgr, Byte>().Resize(148,
                                                        161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); // hien thi 1 khuon mat len imgTrain => nhap thong tin
                        objFace = ImageFrame.Copy(faces[0].rect).Convert<Gray, Byte>().Resize(148,
                                                                161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    }
                    ImageFrame.Draw(faces[0].rect, new Bgr(Color.Red), 3); // danh dau khuon mat phat hien

                    //nhap(objFace);
                }
                else
                {
                   if(captureInProcess==false)
                    {
                        imgTrain.Image = null;
                        btChup.Enabled = false;
                    }

                }
                
  

            }
        }
        public void nhap(Image<Bgr, Byte> img)
        {

        }
        public frmFaceDetection()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btXacNhan_Click(object sender, EventArgs e)
        {
            //bước 1: kiểm tra dữ liệu
            if (imgTrain==null)  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải chụp ảnh");
                return;
            }
            if (txtMaSV.Text.Trim() == "")  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập Mã sinh viên");
                ActiveControl = txtMaSV;
                return;
            }
            if (txtHoTen.Text.Trim() == "")  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập Họ tên");
                ActiveControl = txtHoTen;
                return;
            }
            if (txtHoTen.Text.Trim() == "")  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập Họ tên");
                ActiveControl = txtHoTen;
                return;
            }
            if (txtLop.Text.Trim() == "")  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập Lớp");
                ActiveControl = txtLop;
                return;
            }
            if (txtNgaySinh.Text.Trim() == "")  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập Ngày sinh");
                ActiveControl = txtNgaySinh;
                return;
            }
            if (radNam.Checked==false&&radNu.Checked==false)  // trim() cắt toàn bộ khoảng trắng
            {
                MessageBox.Show("Bạn phải nhập Giới tính");
                ActiveControl = radNam;
                return;
            }
           // int _id = 0;
            // _id = Convert.ToInt16(txtMaSV.Text);


            //Bước 2: tạo đối tượng
            SinhVien obj = new SinhVien();
            obj.ID = txtMaSV.Text.Trim();
            obj.HoTen = txtHoTen.Text.Trim();
            obj.Lop = txtLop.Text.Trim();
            obj.NgaySinh = Convert.ToDateTime(txtNgaySinh.Text);
            string format = "d";
           // obj.NgaySinh = Convert.ToDateTime(DateTime.ParseExact(txtNgaySinh.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture));
            obj.GioiTinh = radNam.Checked;
            //obj.IMG = objFace;
           
            
            //objFace = objFace.ThresholdBinary(new Bgr(50,50,50), new Bgr(255,255,255));

            //obj.image = objFace;
            //CvInvoke.cvShowImage("binary", objFace);
            listSV.Add(obj);
            listImg.Add(objFace);
            //Bước 3: hiển thị

            ListViewItem item = new ListViewItem(stt.ToString());
            ListViewItem.ListViewSubItem sub_item = new ListViewItem.ListViewSubItem(item, txtMaSV.Text);
            ListViewItem.ListViewSubItem sub_item_1 = new ListViewItem.ListViewSubItem(item, txtHoTen.Text);
            ListViewItem.ListViewSubItem sub_item_2 = new ListViewItem.ListViewSubItem(item, txtLop.Text);
            ListViewItem.ListViewSubItem sub_item_3 = new ListViewItem.ListViewSubItem(item, txtNgaySinh.Text);
            ListViewItem.ListViewSubItem sub_item_4 = new ListViewItem.ListViewSubItem(item, (radNam.Checked == true ? "Nam" : "Nữ"));
          
            item.SubItems.Add(sub_item);
            item.SubItems.Add(sub_item_1);
            item.SubItems.Add(sub_item_2);
            item.SubItems.Add(sub_item_3);
            item.SubItems.Add(sub_item_4);
        

            lsvInput.Items.Add(item);
            stt++;
            lsvInput.Refresh();


            captureInProcess = false;
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            txtLop.Text = "";
            txtNgaySinh.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
            btChup.Enabled = true;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            btChup.Enabled = true;
            picProcess = false;
            captureInProcess = false;
            if (capture != null)
            {

                capture = null;
            }
                
            try
            {
                capture = new Capture(cbCamIndex.SelectedIndex);
                Application.Idle += ProcessFrame;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           


          
        }

        private void frmFaceDetection_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_default.xml");
        }

        private void cbCamIndex_SelectedIndexChanged(object sender, EventArgs e) // chon camera
        {
           /* int CamNumber = 0;
            CamNumber = cbCamIndex.SelectedIndex ;
            if(capture==null)
            {
                try
                {
                    capture = new Capture(CamNumber);
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
           // btStart_Click(sender, e);
           // btStart.Enabled = true;
        }
        public void InputInforSV ( )
        {

        }

        private void btMoFile_Click(object sender, EventArgs e)
        {
            ofdMoFile.ShowDialog();
            string file = ofdMoFile.FileName;
            Image imgInput = Image.FromFile(ofdMoFile.FileName);
            ImageFrame = new Image<Bgr, byte>(new Bitmap(imgInput));
            imgCamera.Image = ImageFrame;
            DetectFaces_pic();
        }

        private void imgCamera_Click(object sender, EventArgs e)
        {

        }

        private void bt_next_Click(object sender, EventArgs e)
        {
          
            bt_previous.Enabled = true;
            if(FaceNum<EXFace.Length-1)
            {
                FaceNum++;
                imgTrain.Image = EXFace[FaceNum];
            } 
            if(FaceNum==EXFace.Length-1)
            {
                bt_next.Enabled = false;
            }

        }

        private void bt_Previous_Click(object sender, EventArgs e)
        {
           
            bt_next.Enabled = true;
            if(FaceNum>0)
            {
                FaceNum--;
                imgTrain.Image = EXFace[FaceNum];
            }
            else
            {
                bt_previous.Enabled = false;
            }
            if(FaceNum==0)
                bt_previous.Enabled = false;
        }

        private void btChup_Click(object sender, EventArgs e)
        {
            captureInProcess = true;
            txtMaSV.Focus();
            btChup.Enabled = false;
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            captureInProcess = false;
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            txtLop.Text = "";
            txtNgaySinh.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
            btChup.Enabled = true;
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            ImageFrame = null;
            this.Close();
        }

        private void frmFaceDetection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            ImageFrame = null;
            
        }
    }
}
