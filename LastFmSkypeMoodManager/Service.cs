using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using LastFmSkypeMoodManager.LastFm;
using NLog;

namespace LastFmSkypeMoodManager {
    public partial class Service : ServiceBase {

        private LastFmData lastFm;

        public Service() {
            InitializeComponent();
            lastFm = new LastFmData();
        }

        protected override void OnStart(string[] args) {
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        protected override void OnStop() {
            timer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            lastFm.GetData();
        }

    }
}
