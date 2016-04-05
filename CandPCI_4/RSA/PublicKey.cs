using System;
using System.ComponentModel;
using System.Numerics;
using System.Xml.Serialization;

namespace CandPCI_4.RSA
{
    [Serializable]
    public class PublicKey
    {
        [XmlIgnore]
        public BigInteger r { get; set; }
        [XmlIgnore]
        public BigInteger e { get; set; }

        [XmlElement("r")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string rProxy
        {
            get
            {
                return r.ToString();
            }
            set
            {
                r = BigInteger.Parse(value);
            }
        }

        [XmlElement("e")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string eProxy
        {
            get
            {
                return e.ToString();
            }
            set
            {
                e = BigInteger.Parse(value);
            }
        }
    }
}
