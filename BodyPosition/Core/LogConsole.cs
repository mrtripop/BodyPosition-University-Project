﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition
{
    static class LogConsole
    {
        private static ObservableCollection<string> logs = new ObservableCollection<string>();
        public static ObservableCollection<string> Logs
        {
            get { return logs; }
            private set
            {
                logs = value;
            }
        }

        public static void WriteLine(string format, params object[] arg)
        {
            if (arg != null)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(format, arg);
                System.Windows.Application.Current.Dispatcher.Invoke(() => logs.Add(sb.ToString()));
            }
            else
            {
                logs.Add(format);
            }

            Debug.WriteLine(format, arg);
        }
    }
}