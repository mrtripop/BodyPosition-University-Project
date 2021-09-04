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

        private Dictionary<string, Dictionary<string, AngleModel>> _angleReadFile = new Dictionary<string, Dictionary<string, AngleModel>>();

        SolidColorBrush redColor = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
        SolidColorBrush blueColor = new SolidColorBrush(Color.FromArgb(0xff, 0x21, 0x96, 0xf3));

        private double durationLeft;
        private double durationRight;

        private int counter = 0;
        private string frontDraw = "front";

        private bool recordingState = false;
        private bool bodyEntered = false;
        private bool change = false;

        KinectManager km = KinectManager.Instance;
        
        #endregion

        #region Initialize
        public HomeView(UserModel userModelHome, TestModel testModelHome)
        {
            InitializeComponent();

            UserModelHome = userModelHome;
            TestModelHome = testModelHome;

            _angleReadFile = ReadAngle();

            userUID.Text = UserModelHome.Id.ToString();
            userName.Text = UserModelHome.FirstName + " " + UserModelHome.LastName;
            testName.Text = TestModelHome.TestName;

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

        //record angle at first index
        //edit joint
        //accurate angle

        //ui

        #region Method
        private void CalculateAngle(Body body)
        {
            // Edit joint
            Point footPoint = viewer.GetPoint(body.Joints[JointType.FootLeft].Position);
            Point anklePoint = viewer.GetPoint(body.Joints[JointType.AnkleLeft].Position);
            Point kneePoint = viewer.GetPoint(body.Joints[JointType.KneeLeft].Position);
            anklePoint.Y = footPoint.Y;

            durationLeft = body.DurationLeft();
            durationRight = body.DurationRight();

            Point shoulderR = new Point(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.ShoulderRight].Position.Y);
            Point shoulderL = new Point(body.Joints[JointType.ShoulderLeft].Position.X, body.Joints[JointType.ShoulderLeft].Position.Y);
            Point spine = new Point(body.Joints[JointType.ShoulderRight].Position.X, body.Joints[JointType.SpineMid].Position.Y);

            if (frontDraw == "front")
            {
                //show
                anglePelvisFront.Update(body.Joints[spineMid], body.Joints[spineBase], body.Joints[hipRight], 50);
                spaceShoulderRight.Update(spine, shoulderR);
                spaceShoulderLeft.Update(spine, shoulderL);

                // dont show
                //angleSidePelvis.Clear();
                //angleKnee.Clear();
                //angleAnkle.Clear();

                //tblPelvisSideAngle.Text = "";
                //tblKneeAngle.Text = "";
                //tblAnkleAngle.Text = "";

                //display
                tblPelvisFront.Text = ((int)anglePelvisFront.Angle).ToString();
                tblShoulderRight.Text = durationRight.ToString("0.00");
                tblShoulderLeft.Text = durationLeft.ToString("0.00");

                // show
                angleSidePelvis.Update(body.Joints[spineMid], body.Joints[hipLeft], body.Joints[kneeLeft], 50);
                angleKnee.Update(body.Joints[hipLeft], body.Joints[kneeLeft], body.Joints[ankleLeft], 50);
                angleKnee.SweepDirection = SweepDirection.Counterclockwise;
                angleAnkle.Update(kneePoint, anklePoint, footPoint, 50);

                // display
                tblPelvisSideAngle.Text = ((int)angleSidePelvis.Angle).ToString();
                tblKneeAngle.Text = ((int)angleKnee.Angle).ToString();
                tblAnkleAngle.Text = ((int)angleAnkle.Angle).ToString();

            }
            else if (frontDraw == "side")
            {
                // dont show
                anglePelvisFront.Clear();
                spaceShoulderRight.Clear();
                spaceShoulderLeft.Clear();

                tblPelvisFront.Text = "";
                tblShoulderRight.Text = "";
                tblShoulderLeft.Text = "";

                // show
                angleSidePelvis.Update(body.Joints[spineMid], body.Joints[hipLeft], body.Joints[kneeLeft], 50);
                angleKnee.Update(body.Joints[hipLeft], body.Joints[kneeLeft], body.Joints[ankleLeft], 50);
                angleKnee.SweepDirection = SweepDirection.Counterclockwise;
                angleAnkle.Update(kneePoint, anklePoint, footPoint, 50);

                // display
                tblPelvisSideAngle.Text = ((int)angleSidePelvis.Angle).ToString();
                tblKneeAngle.Text = ((int)angleKnee.Angle).ToString();
                tblAnkleAngle.Text = ((int)angleAnkle.Angle).ToString();

            }

            RecordJoint();
        }
        private void RecordJoint()
        {
            if (recordingState)
            {
                if (frontDraw == "front")
                {
                    if (bodyEntered)
                    {
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

                        _angleReadFile[UserModelHome.Id.ToString()][TestModelHome.Id.ToString()].Angle.Add(counter.ToString(), newAngle);
                    }
                    else
                    {
                        Angle newAngle = new Angle()
                        {
                            Id = counter,
                            FrontPelvis = 0,
                            RightShoulder = 0,
                            LeftShoulder = 0,
                            Pelvis = 0,
                            Knee = 0,
                            Ankle = 0
                        };

                        _angleReadFile[UserModelHome.Id.ToString()][TestModelHome.Id.ToString()].Angle.Add(counter.ToString(), newAngle);
                    }
                }
                else if (frontDraw == "side")
                {
                    Angle newAngle = new Angle()
                    {
                        Id = counter,
                        FrontPelvis = 0,
                        RightShoulder = 0,
                        LeftShoulder = 0,
                        Pelvis = angleSidePelvis.Angle,
                        Knee = angleKnee.Angle,
                        Ankle = angleAnkle.Angle
                    };

                    _angleReadFile[UserModelHome.Id.ToString()][TestModelHome.Id.ToString()].Angle.Add(counter.ToString(), newAngle);

                }
            }
        }
        private void Save()
        {
            if (_angleReadFile.Count > 0)
            {
                WriteAngle(_angleReadFile);
                MessageBox.Show("บันทึกข้อมูลสำเร็จ");
            }
            _angleReadFile.Clear();
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
                    //นับไปเรื่อยๆทั้งที่ยังไม่
                    if (recordingState)
                    { 
                         counter += 1; 
                    }

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
                        viewer.DrawBody(body, change, frontDraw);
                        CalculateAngle(body);
                        change = false;
                    }
                }
            }
        }
        private void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
            bodyEntered = true;
        }
        private void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
            //do something
            bodyEntered = false;

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
            Console.WriteLine("state change");
            //if (km.saveState == KStudioRecordingState.Recording)
            //{
            //    recordingState = true;
            //}
            //else if (km.saveState == KStudioRecordingState.Done)
            //{
            //    recordingState = false;
            //}
        }
        #endregion

        #region Button Event Method
        private void Record(object sender, RoutedEventArgs e)
        {
            if (!recordingState)
            {
                if (MessageBox.Show("Recording using the Kinect Studio API generates HUGE files (~140 MB for 1 second). Are you sure, you want to proceed?",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var sfd = new SaveFileDialog();
                    sfd.Filter = "Kinect Eventstream|*.xef|All files|*.*";
                    sfd.Title = "Save the recording";
                    sfd.FileName = TestModelHome.TestName;

                    if (sfd.ShowDialog() == true)
                    {
                        km.StartRecording(sfd.FileName);
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
                km.RecordingStateChanged += RecordingState;

                Save();

                recordButton.Background = blueColor;
                recordButton.Content = "Record";
                recordingState = false;
            }
        }
        private void FrontDetect(object sender, RoutedEventArgs e)
        {
            frontDraw = "front";
            change = true;
            frontButton.Background = redColor;
            sideButton.Background = blueColor;
        }
        private void SideDetect(object sender, RoutedEventArgs e)
        {
            frontDraw = "side";
            change = true;
            sideButton.Background = redColor;
            frontButton.Background = blueColor;
        }     
        private void OpenDatabaseView(object sender, RoutedEventArgs e)
        {
            DatabaseView bdv = new DatabaseView(UserModelHome, TestModelHome);
            bdv.ShowDialog();
        }
        private void OpenKinect(object sender, RoutedEventArgs e)
        {

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

        #region Json Method
        private Dictionary<string, Dictionary<string, AngleModel>> ReadAngle()
        {
            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json")));
            var angleModel = AngleModel.FromJson(json.ToString());
            return angleModel;
        }

        private void WriteAngle(Dictionary<string, Dictionary<string, AngleModel>> angle)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json"), angle.ToJson());
        }
        #endregion
    }
}
