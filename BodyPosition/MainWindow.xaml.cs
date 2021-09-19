using BodyPosition.Core;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using BodyPosition.MVVM.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace BodyPosition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly string PATH_DATABASE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase");
        public readonly string PATH_BACKUP_DATABASE = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase");

        public readonly string PATH_ANGLE_FOLDER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle");
        public readonly string PATH_ANGLE_BACKUP_FOLDER = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\Angle");

        public readonly string PATH_USER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json");
        public readonly string PATH_TEST = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json");

        public readonly string PATH_USER_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\UserJson.json");
        public readonly string PATH_TEST_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\TestJson.json");

        private Database user;
        private Database test;
        private Database userBackup;
        private Database testBackup;

        string json = "{}";

        public MainWindow()
        {
            InitializeComponent();


            try
            {
                // create JsonDatabase folder
                if (!Directory.Exists(PATH_DATABASE))
                {
                    Directory.CreateDirectory(PATH_DATABASE);
                }

                // create BackupDatabase folder
                if (!Directory.Exists(PATH_BACKUP_DATABASE))
                {
                    Directory.CreateDirectory(PATH_BACKUP_DATABASE);
                }

                // create angle folder
                if (!Directory.Exists(PATH_ANGLE_FOLDER))
                {
                    Directory.CreateDirectory(PATH_ANGLE_FOLDER);
                }

                if (!Directory.Exists(PATH_ANGLE_BACKUP_FOLDER))
                {
                    Directory.CreateDirectory(PATH_ANGLE_BACKUP_FOLDER);
                }

                // create json file
                if (!File.Exists(PATH_USER))
                {
                    using (StreamWriter sw = File.CreateText(PATH_USER))
                    {
                        sw.WriteLine("{}");
                        sw.Close();

                    }
                }

                if (!File.Exists(PATH_TEST))
                {
                    using (StreamWriter sw = File.CreateText(PATH_TEST))
                    {
                        sw.WriteLine("{}");
                        sw.Close();
                    }
                }

                if (!File.Exists(PATH_USER_BACKUP))
                {
                    using (StreamWriter sw = File.CreateText(PATH_USER_BACKUP))
                    {
                        sw.WriteLine("{}");
                        sw.Close();

                    }
                }

                if (!File.Exists(PATH_TEST_BACKUP))
                {
                    using (StreamWriter sw = File.CreateText(PATH_TEST_BACKUP))
                    {
                        sw.WriteLine("{}");
                        sw.Close();

                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new UserSelectionView());
        }
    }
}
