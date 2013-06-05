using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LastFmSkypeMoodManager.LastFm {
    [Serializable]
    public class track {
        [XmlElement]
        public string artist { get; set; }
        [XmlElement]
        public string name { get; set; }
        [XmlElement]
        public string streamable { get; set; }
        [XmlElement]
        public string mbid { get; set; }
        [XmlElement]
        public string album { get; set; }
        [XmlElement]
        public string url { get; set; }
        [XmlElement]
        public string image { get; set; }
        [XmlAttribute]
        public string small { get; set; }
        [XmlAttribute]
        public string medium { get; set; }
        [XmlAttribute]
        public string large { get; set; }
        [XmlAttribute]
        public string extralarge { get; set; }
        [XmlElement]
        public string date { get; set; }
    }
}
