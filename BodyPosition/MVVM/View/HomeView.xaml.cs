using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Win32;

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

        JointType hipRight = JointType.HipRight;
        JointType spineBase = JointType.SpineBase;
        JointType spineMid = JointType.SpineMid;
        JointType hipLeft = JointType.HipLeft;

        JointType kneeLeft = JointType.KneeLeft;
        JointType ankleLeft = JointType.AnkleLeft;
        JointType footLeft = JointType.FootLeft;

        private bool readyToRecord = true;
        private bool save = false;

        readonly string FOLDER_PATH;
        readonly string folder_text;

        private bool frontDraw = true;

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

        List<AngleModel> angleList = new List<AngleModel>();
        List<Body> trackedBodies = new List<Body>();

        SolidColorBrush blueColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0xff, 0x00, 0x66));
        SolidColorBrush redColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x21, 0x96, 0xf3));


        #endregion

        #region main
        public HomeView(UserModel userModelHome, TestModel testModelHome)
        {
            InitializeComponent();

            UserModelHome = userModelHome;
            TestModelHome = testModelHome;

            userUID.Text = UserModelHome.ID.ToString();
            userName.Text = UserModelHome.FirstName + " " + UserModelHome.LastName;
            testName.Text = TestModelHome.TestName;

            folder_text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory +TestModelHome.TestName + ".txt");
            FOLDER_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory +TestModelHome.TestName + "video");

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                    FrameSourceTypes.Depth |
                    FrameSourceTypes.Infrared |
                    FrameSourceTypes.Body);
                if (!save)
                {
                    _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                }


                _playersController = new PlayersController();
                _playersController.BodyEntered += UserReporter_BodyEntered;
                _playersController.BodyLeft += UserReporter_BodyLeft;
                _playersController.Start();


            }
        }
        #endregion

        #region method
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

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            AngleModel angle = new AngleModel();

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
                        viewer.DrawBody(body, frontDraw);

                        double durationLeft = body.DurationLeft();
                        double durationRight = body.DurationRight();

                        //Point shoulderR = new Point(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.ShoulderRight].Position.Y);
                        //Point shoulderL = new Point(body.Joints[JointType.ShoulderLeft].Position.X, body.Joints[JointType.ShoulderLeft].Position.Y);
                        //Point spine = new Point(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.SpineMid].Position.Y);

                        //if (frontDraw)
                        //{
                            anglePelvisFront.Update(body.Joints[spineMid], body.Joints[spineBase], body.Joints[hipRight], 50);
                            //spaceShoulderRight.Update(spine, shoulderR, shoulderR, 50);
                            //spaceShoulderLeft.Update(spine, shoulderL, shoulderL, 50);

                            angleSidePelvis.Clear();
                            angleKnee.Clear();
                            angleAnkle.Clear();
                        //}
                        //else
                        //{
                            anglePelvisFront.Clear();

                            angleSidePelvis.Update(body.Joints[spineMid], body.Joints[hipLeft], body.Joints[kneeLeft], 50);
                            angleKnee.Update(body.Joints[hipLeft], body.Joints[kneeLeft], body.Joints[ankleLeft], 50);
                            angleKnee.SweepDirection = SweepDirection.Counterclockwise;

                            angleAnkle.Update(body.Joints[kneeLeft], body.Joints[ankleLeft], body.Joints[footLeft], 50);
                        //}

                        DateTime currentTime = DateTime.Now;

                        angle.Time = currentTime.ToString("hh:mm ss");
                        angle.FrontPelvis = anglePelvisFront.Angle;
                        angle.RightShoulder = durationRight;
                        angle.LeftShoulder = durationLeft;
                        angle.Pelvis = angleSidePelvis.Angle;
                        angle.Knee = angleKnee.Angle;
                        angle.Ankle = angleAnkle.Angle;

                        //if (!record)
                        //{
                        //    statusText.Text = "recording...";
                        //    trackedBodies.Add(body);
                        //    angleList.Add(angle);
                        //}

                        tblPelvisFront.Text = ((int)anglePelvisFront.Angle).ToString();
                        tblShoulderRight.Text = durationRight.ToString("0.00");
                        tblShoulderLeft.Text = durationLeft.ToString("0.00");

                        tblPelvisSideAngle.Text = ((int)angleSidePelvis.Angle).ToString();
                        tblKneeAngle.Text = ((int)angleKnee.Angle).ToString();
                        tblAnkleAngle.Text = ((int)angleAnkle.Angle).ToString();
                    }
                }
            }
        }

        void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
        }

        void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
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

        private void recordVideoAndAngle(object sender, RoutedEventArgs e)
        {
            //if (record)
            //{
            //    recordButton.Background = blueColor;
            //    recordButton.Content = "Record";
            //    record = false;
            //}
            //else
            //{
            //    recordButton.Background = redColor;
            //    recordButton.Content = "Recording";
            //    record = true;
            //}       
            if (readyToRecord)
            {
                if (MessageBox.Show("Recording using the Kinect Studio API generates HUGE files (~140 MB for 1 second). Are you sure, you want to proceed?",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var sfd = new SaveFileDialog();
                    sfd.Filter = "Kinect Eventstream|*.xef|All files|*.*";
                    sfd.Title = "Save the recording";

                    if (sfd.ShowDialog() == true)
                    {
                        KinectManager.Instance.StartRecording(sfd.FileName);
                        readyToRecord = false;
                    }

                    recordButton.Background = redColor;
                    recordButton.Content = "Recording...";
                }
            }
            else
            {
                KinectManager.Instance.StopRecording();
                readyToRecord = true;

                recordButton.Background = blueColor;
                recordButton.Content = "Record";
            }
        }

        private void FrontDetect(object sender, RoutedEventArgs e)
        {
            frontDraw = true;
            frontButton.Background = blueColor;
            sideButton.Background = redColor;
        }

        private void SideDetect(object sender, RoutedEventArgs e)
        {
            frontDraw = false;
            sideButton.Background = blueColor;
            frontButton.Background = redColor;
        }
        
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void saveResultToDatabase(object sender, RoutedEventArgs e)
        {
            
            statusText.Text = "Saving...";
            saveButton.Background = redColor;
            saveButton.Content = "Saving..";

            //save angle
            save = true;
            if (angleList.Count < 1) { }
            else
            {
                for (int i = 0; i < angleList.Count; i++)
                {
                    SqliteDataAccess.AddAngle(angleList[i], TestModelHome);
                }
            }

            //save body
            if (trackedBodies.Count() < 1) { }
            else
            {
                string kinectBodyDataString = JsonConvert.SerializeObject(trackedBodies);
                FileStream fParameter = new FileStream(folder_text, FileMode.Create, FileAccess.Write);
                StreamWriter m_WriterParameter = new StreamWriter(fParameter);
                m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
                m_WriterParameter.Write(kinectBodyDataString);
                m_WriterParameter.Flush();
                m_WriterParameter.Close();
            }

            statusText.Text = "Complete!";
            saveButton.Content = "Save";
            saveButton.Background = blueColor;

            save = false;

            //clear list
            angleList.Clear();
            trackedBodies.Clear();

        }

        #endregion
    }
}
