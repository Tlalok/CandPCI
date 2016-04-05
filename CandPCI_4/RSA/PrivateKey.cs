using System;
using System.ComponentModel;
using System.Numerics;
using System.Xml.Serialization;

namespace CandPCI_4.RSA
{
    [Serializable]
    public class PrivateKey
    {
        [XmlIgnore]
        public BigInteger p { get; set; }
        [XmlIgnore]
        public BigInteger q { get; set; }

        [XmlElement("p")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string pProxy
        {
            get
            {
                return p.ToString();
            }
            set
            {
                p = BigInteger.Parse(value);
            }
        }

        [XmlElement("q")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string qProxy
        {
            get
            {
                return q.ToString();
            }
            set
            {
                q = BigInteger.Parse(value);
            }
        }
    }
}
