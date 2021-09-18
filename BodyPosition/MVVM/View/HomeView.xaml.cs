using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Win32;
using BodyPosition.MVVM.Model.UserModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.AngleModel;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using Microsoft.Kinect.Tools;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using BodyPosition.Core;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        #region parameter

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;

        private UserModel myUserModelHome;
        public UserModel UserModelHome
        {
            get { return myUserModelHome; }
            set { myUserModelHome = value; }
        }

        private TestModel myTestModelHome;
        public TestModel TestModelHome
        {
            get { return myTestModelHome; }
            set { myTestModelHome = value; }
        }

        AngleModel _angleReadFile = new AngleModel();

        SolidColorBrush redColor = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
        SolidColorBrush blueColor = new SolidColorBrush(Color.FromArgb(0xff, 0x21, 0x96, 0xf3));

        private double durationLeft;
        private double durationRight;
        private int counter = 0;
        private bool recordingState = false;

        KinectManager km = KinectManager.Instance;
        private Stopwatch sw;

        private Database dbAngleManager;

        #endregion

        #region Initialize
        public HomeView(UserModel userModelHome, TestModel testModelHome, string path_angle_select)
        {
            InitializeComponent();

            dbAngleManager = new Database(path_angle_select);

            UserModelHome = userModelHome;
            TestModelHome = testModelHome;

            userUID.Text = UserModelHome.Id.ToString();
            userName.Text = UserModelHome.FirstName + " " + UserModelHome.LastName;
            testName.Text = TestModelHome.TestName;

            _angleReadFile = dbAngleManager.ReadAngle();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                    FrameSourceTypes.Depth |
                    FrameSourceTypes.Infrared |
                    FrameSourceTypes.Body);

                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _playersController = new PlayersController();
                _playersController.BodyEntered += UserReporter_BodyEntered;
                _playersController.BodyLeft += UserReporter_BodyLeft;
                _playersController.Start();

            }
        }
        #endregion

        #region Method
        private void CalculateAngle(Body body)
        {

            // New body management. ex. position, joint, ----> Vector3D
            Point footPoint = viewer.GetPoint(body.Joints[JointType.FootLeft].Position);
            Point anklePoint = viewer.GetPoint(body.Joints[JointType.AnkleLeft].Position);
            Point kneePoint = viewer.GetPoint(body.Joints[JointType.KneeLeft].Position);
            anklePoint.Y = footPoint.Y;

            Point shoulderR = new Point(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.ShoulderRight].Position.Y);
            Point shoulderL = new Point(body.Joints[JointType.ShoulderLeft].Position.X, body.Joints[JointType.ShoulderLeft].Position.Y);
            Point spine = new Point(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.SpineMid].Position.Y);



            //shoulder
            Vector3D spineShoulder3d = body.Joints[JointType.SpineShoulder].Position.ToVector3();
            Vector3D shoulderLeft3d = body.Joints[JointType.ShoulderLeft].Position.ToVector3();
            Vector3D shoulderRight3d = body.Joints[JointType.ShoulderRight].Position.ToVector3();

            //mid
            Vector3D spineMid3d = body.Joints[JointType.SpineMid].Position.ToVector3();

            //base
            Vector3D spineBase3d = body.Joints[JointType.SpineBase].Position.ToVector3();

            //left
            Vector3D hipLeft3d = body.Joints[JointType.HipLeft].Position.ToVector3();
            Vector3D kneeLeft3d = body.Joints[JointType.KneeLeft].Position.ToVector3();
            Vector3D ankleLeft3d = body.Joints[JointType.AnkleLeft].Position.ToVector3();
            Vector3D footLeft3d = body.Joints[JointType.FootLeft].Position.ToVector3();

            //right
            Vector3D hipRight3d = body.Joints[JointType.HipRight].Position.ToVector3();
            Vector3D kneeRight3d = body.Joints[JointType.KneeRight].Position.ToVector3();
            Vector3D ankleRight3d = body.Joints[JointType.AnkleRight].Position.ToVector3();
            Vector3D footRight3d = body.Joints[JointType.FootRight].Position.ToVector3();

            //edit ankle position
            Vector3D editAnkleLeftFootRef = new Vector3D(ankleLeft3d.X, footLeft3d.Y, ankleLeft3d.Z);
            Vector3D editAnkleRightFootRef = new Vector3D(ankleRight3d.X, footRight3d.Y, ankleRight3d.Z);

            //psudo vector3d spineMid refference
            Vector3D shoulderLeftBaseRef = new Vector3D(shoulderLeft3d.X, spineMid3d.Y, spineMid3d.Z);
            Vector3D shoulderRightBaseRef = new Vector3D(shoulderRight3d.X, spineMid3d.Y, spineMid3d.Z);

            //psudo spine base refference
            Vector3D psudoKneeLeftMapSpineBaseX = new Vector3D(spineBase3d.X, kneeLeft3d.Y, kneeLeft3d.Z);

            //calculate duration between shoulder and spineMid
            durationLeft = body.DurationLeft();
            durationRight = body.DurationRight();

            //body display
            //show front ---> position of this topic must correct!
            anglePelvisFront.Update(spineMid3d, spineBase3d, hipRight3d, 50);
            spaceShoulderRight.Update(spine, shoulderR);
            spaceShoulderLeft.Update(spine, shoulderL);

            // show left ---> position of this topic must correct!
            angleSidePelvis.Update(spineMid3d, spineBase3d, psudoKneeLeftMapSpineBaseX, 50);


            angleKnee.Update(hipLeft3d, kneeLeft3d, editAnkleLeftFootRef, 50);
            angleAnkle.Update(kneeLeft3d, editAnkleLeftFootRef, footLeft3d, 50);

            //show right ---> position of this topic must correct!

            //text display
            tblPelvisFront.Text = ((int)anglePelvisFront.Angle).ToString();
            tblShoulderRight.Text = durationRight.ToString("0.00");
            tblShoulderLeft.Text = durationLeft.ToString("0.00");
            tblPelvisSideAngle.Text = ((int)angleSidePelvis.Angle).ToString();
            tblKneeAngle.Text = ((int)angleKnee.Angle).ToString();
            tblAnkleAngle.Text = ((int)angleAnkle.Angle).ToString();


            RecordJoint();
        }
        private void RecordJoint()
        {
            if (recordingState)
            {
                counter++;

                Angle newAngle = new Angle()
                {
                    Id = counter,
                    FrontPelvis = anglePelvisFront.Angle,
                    RightShoulder = durationRight,
                    LeftShoulder = durationLeft,
                    Pelvis = angleSidePelvis.Angle,
                    Knee = angleKnee.Angle,
                    Ankle = angleAnkle.Angle
                };

                _angleReadFile.Angle.Add(counter.ToString(), newAngle);
            }
        }
        private void Save(AngleModel angleList)
        {
            dbAngleManager.WriteJson(angleList);
            MessageBox.Show("บันทึกข้อมูลสำเร็จ");
            angleList.Angle.Clear();
        }
        #endregion

        #region Event Method

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();
                    _playersController.Update(bodies);

                    Body body = bodies.Closest();

                    if (body != null)
                    {
                        //วาดกระดูก
                        viewer.DrawBody(body);
                        //คำนวณองศา
                        CalculateAngle(body);
                    }
                }
            }
        }
        private void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
        }
        private void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
            //do something

            viewer.Clear();

            anglePelvisFront.Clear();
            spaceShoulderRight.Clear();
            spaceShoulderLeft.Clear();
            angleSidePelvis.Clear();
            angleKnee.Clear();
            angleAnkle.Clear();

            tblPelvisFront.Text = "";
            tblShoulderRight.Text = "";
            tblShoulderLeft.Text = "";
            tblPelvisSideAngle.Text = "";
            tblKneeAngle.Text = "";
            tblAnkleAngle.Text = "";
        }
        private void RecordingState(object sender, EventArgs e)
        {
            Console.WriteLine("recording state: Done");
            recordingState = false;
        }
        #endregion

        #region Button Event Method
        private void Record(object sender, RoutedEventArgs e)
        {
            if (_angleReadFile.Angle.Count > 0 && recordingState == false)
            {
                MessageBox.Show("การทดสอบนี้มีข้อมูลถูกบันทึกไว้อยู่แล้ว");
                return;
            }

            if (!recordingState)
            {
                if (MessageBox.Show("การบันทึกนั้นใช้ Kinect Studio API เพื่อสร้างไฟล์ขนาดใหญ่(~140 MB for 1 second). กรุณาตรวจสอบพื้นที่การจัดเก็บให้เพียงพอ",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var sfd = new SaveFileDialog();
                    sfd.Filter = "Kinect Eventstream|*.xef|All files|*.*";
                    sfd.Title = "Save the recording";
                    sfd.FileName = TestModelHome.TestName;

                    if (sfd.ShowDialog() == true)
                    {
                        km.StartRecording(sfd.FileName);
                        sw = Stopwatch.StartNew();
                        km.RecordingStateChanged += RecordingState;

                        recordButton.Background = redColor;
                        recordButton.Content = "Recording...";
                        recordingState = true;
                    }
                    else
                    {
                        recordButton.Background = blueColor;
                        recordButton.Content = "Record";
                    }
                }
            }
            else
            {
                km.StopRecording();
                sw.Stop();
                sw.Restart();
                km.RecordingStateChanged += RecordingState;

                Save(_angleReadFile);

                recordButton.Background = blueColor;
                recordButton.Content = "Record";
                recordingState = false;
            }
        }
        private void OpenDatabaseView(object sender, RoutedEventArgs e)
        {
            //read angle
            AngleModel sdbv = new AngleModel();
            sdbv = dbAngleManager.ReadAngle();

            DatabaseView bdv = new DatabaseView(UserModelHome, TestModelHome, sdbv, dbAngleManager.path);
            bdv.Show();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_playersController != null)
            {
                _playersController.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (_playersController != null)
            {
                _playersController.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
        #endregion

    }
}
