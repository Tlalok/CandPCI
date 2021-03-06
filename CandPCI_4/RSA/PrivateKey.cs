﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CandPCI_4.RSA
{
    [Serializable]
    public class PrivateKey
    {
        [XmlIgnore]
        public BigInteger r { get; set; }
        [XmlIgnore]
        public BigInteger d { get; set; }

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

        [XmlElement("d")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string dProxy
        {
            get
            {
                return d.ToString();
            }
            set
            {
                d = BigInteger.Parse(value);
            }
        }
    }
}
