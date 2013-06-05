using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NLog;
using SKYPE4COMLib;

namespace LastFmSkypeMoodManager.LastFm {
    internal class LastFmData {
        private const string lastFmUrl = "http://ws.audioscrobbler.com/2.0/user/{0}/recenttracks";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string _lastFmUserName;
        private recenttracks _lastTracks;

        protected string LastFmUserName {
            get {
                if (_lastFmUserName == null) {
                    _lastFmUserName = ConfigurationManager.AppSettings["LastFmUsername"];
                    if (_lastFmUserName == null) throw new ConfigurationErrorsException("Missing value for key 'LastFmUsername'");
                }

                return _lastFmUserName;
            }
        }

        internal void GetData() {
            var recent = new recenttracks();
            var serialize = new XmlSerializer(typeof(recenttracks));
            var request = WebRequest.Create(String.Format(lastFmUrl, LastFmUserName));
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream()) {
                recent = (recenttracks)serialize.Deserialize(stream);
            }

            // See if the data has changed
            if (_lastTracks == recent) {
                _lastTracks = recent;
                return;
            }

            // Parse
            ParseData(recent);

            // Set cached value
            _lastTracks = recent;
        }

        private void ParseData(recenttracks recent) {
            var latestTrack = recent.tracks[0];
            OutputMoodMessage(latestTrack);
            if (logger.IsDebugEnabled) {
                // If we have "cached" data just write the most recent track
                if (_lastTracks != null) {
                    var track = recent.tracks[0];
                    LogDebug(String.Format(@"Played ""{0}"" by {1} from the album ""{2}"" on {3}", track.name, track.artist, track.album, track.date));
                    return;
                }

                // Log 'em all
                foreach (var track in recent.tracks) {
                    LogDebug(String.Format(@"Played ""{0}"" by {1} from the album ""{2}"" on {3}", track.name, track.artist, track.album, track.date));
                }
            }
        }

        private void OutputMoodMessage(track track) {
            var skype = new Skype();
            if (!skype.Client.IsRunning) return;

            // Get format message
            var msg = ConfigurationManager.AppSettings["MoodMessage"];
            if (msg == null) msg = "Now Playing: {0}";

            // Apply match replacements
            msg = Regex.Replace(msg, "%name%", track.name ?? String.Empty);
            msg = Regex.Replace(msg, "%artist%", track.artist ?? String.Empty);
            msg = Regex.Replace(msg, "%album%", track.album ?? String.Empty);
            msg = Regex.Replace(msg, "%date%", track.date ?? String.Empty);
            msg = Regex.Replace(msg, "%url%", track.url ?? String.Empty);

            // Set mood message
            skype.CurrentUserProfile.RichMoodText = msg;
        }

        internal void LogInfo(string message) {
            logger.Log(LogLevel.Info, message);
        }

        internal void LogDebug(string message) {
            logger.Log(LogLevel.Debug, message);
        }

    }
}
