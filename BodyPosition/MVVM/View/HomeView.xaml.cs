using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;

        JointType hipRight = JointType.HipRight;
        JointType spineBase = JointType.SpineBase;
        JointType spineMid = JointType.SpineMid;
        JointType hipLeft = JointType.HipLeft;

        JointType kneeRight = JointType.KneeRight;
        JointType ankleRight = JointType.AnkleRight;
        JointType footRight = JointType.FootRight;

        JointType kneeLeft = JointType.KneeLeft;
        JointType ankleLeft = JointType.AnkleLeft;
        JointType footLeft = JointType.FootLeft;

        JointType spineShoulder = JointType.SpineShoulder;
        JointType shoulderRight = JointType.ShoulderRight;
        JointType shoulderLeft = JointType.ShoulderLeft;

        PersonModel person = new PersonModel();

        VitruviusRecorder recorder = new VitruviusRecorder();

        readonly string FOLDER_PATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "video");

        public HomeView()
        {
            InitializeComponent();

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
                        viewer.DrawBody(body);

                        //spacePelvisFront.Update(body.Joints[_start1], body.Joints[_center1], body.Joints[_end1], 50);
                        //spaceShoulderRight.Update(body.Joints[_start2], body.Joints[_center2], body.Joints[_end2], 50);
                        //spaceShoulderLeft.Update(body.Joints[_start3], body.Joints[_center3], body.Joints[_end3], 50);

                        angleSidePelvis.Update(body.Joints[spineMid], body.Joints[hipLeft], body.Joints[kneeLeft], 50);
                        angleKnee.Update(body.Joints[hipLeft], body.Joints[kneeLeft], body.Joints[ankleLeft], 50);
                        angleAnkle.Update(body.Joints[kneeLeft], body.Joints[ankleLeft], body.Joints[footLeft], 50);

                        //angle.pelvis = spacePelvisFront.Angle;
                        //angle.knee = spaceShoulderRight.Angle;
                        //angle.ankle = spaceShoulderLeft.Angle;

                        //if (record)
                        //{
                        //    SqliteDataAccess.SaveAngle(angle);
                        //}

                        //tblPelvisFront.Text = ((int)spacePelvisFront.Angle).ToString(); //space
                        //tblShoulderRight.Text = ((int)spaceShoulderRight.Angle).ToString();  //space
                        //tblShoulderLeft.Text = ((int)spaceShoulderLeft.Angle).ToString();  //space

                        tblPelvisSideAngle.Text = ((int)angleSidePelvis.Angle).ToString();
                        tblKneeAngle.Text = ((int)angleKnee.Angle).ToString();
                        tblAnkleAngle.Text = ((int)angleAnkle.Angle).ToString();
                    }
                }
            }

        }

        void UserReporter_BodyEntered(object sender, PlayersControllerEventArgs e)
        {
        }

        void UserReporter_BodyLeft(object sender, PlayersControllerEventArgs e)
        {
            viewer.Clear();

            //angle1.Clear();
            //angle2.Clear();
            //angle3.Clear();
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
            if (recorder.IsRecording)
            {
                recorder.Stop();
            }
            else
            {
                recorder.Clear();

                recorder.Visualization = Visualization.Color;
                recorder.Folder = FOLDER_PATH;

            }
        }

        private void AddTestUser(object sender, RoutedEventArgs e)
        {
            //SqliteDataAccess.CreateTestTableByPersonID(person.id);

            // step follow
            // 1. show window popup
            // 2. map data from popup to PersonModel
            // 3. create new table ---> AngleTable_person{id}_test{id}
        }

        private void DeleteTestUser(object sender, RoutedEventArgs e)
        {

        }

        private void OpenUserData(object sender, RoutedEventArgs e)
        {

        }

        private void FrontDetect(object sender, RoutedEventArgs e)
        {

        }

        private void SideDetect(object sender, RoutedEventArgs e)
        {

        }
    }
}
