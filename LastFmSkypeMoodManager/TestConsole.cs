using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastFmSkypeMoodManager.LastFm;

namespace LastFmSkypeMoodManager {
    class TestConsole {
        static void Main(string[] args) {
            // This used to run the service as a console (development phase only)
            var lastFm = new LastFmData();
            lastFm.GetData();

            Console.WriteLine("Press Enter to terminate ...");
            Console.ReadLine();
        }
    }
}
