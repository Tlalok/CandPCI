using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CandPCI_6
{
    public class LsbMethod
    {
        private byte[] data;
        private int countLsBits;

        public byte[] Data
        {
            get { return data; }
        }

        private int writePosition = 0;
        private int readPosition = 0;

        public LsbMethod(byte[] data, int countLsBits)
        {
            //this.data = data;
            this.data = new byte[data.Length];

            Array.Copy(data, this.data, data.Length);
            if (countLsBits > 2 || countLsBits < 1)
                throw  new ArgumentException();
            this.countLsBits = countLsBits;
        }

        public void WriteBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                this[writePosition++] = bytes[i];
            }
        }

        public void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value).Reverse().ToArray();
            WriteBytes(bytes);
        }

        public void WriteLongInt(long value)
        {
            var bytes = BitConverter.GetBytes(value).Reverse().ToArray();
            WriteBytes(bytes);
        }

        public void WriteString(string message)
        {
            var bytes = Encoding.Unicode.GetBytes(message);
            WriteBytes(bytes);
        }

        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = this[readPosition++];
            }
            return bytes;
        }

        public long ReadLongInt()
        {
            return BitConverter.ToInt64(ReadBytes(8).Reverse().ToArray(), 0);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4).Reverse().ToArray(), 0);
        }

        public string ReadMessage(int countBytes)
        {
            var bytes = ReadBytes(countBytes);
            return Encoding.Unicode.GetString(bytes);
        }

        public byte this[int index]
        {
            get
            {
                var countBytesToRead = 8 / countLsBits;
                var startIndex = index * countBytesToRead;
                if (countBytesToRead + startIndex > data.Length)
                    throw new IndexOutOfRangeException();
                var result = 0;
                var mask = 1 | countLsBits; // работает только с моим набором занчений
                for (int i = 0; i < countBytesToRead; i++)
                {
                    result <<= countLsBits;
                    result |= data[startIndex + i] & mask;
                }
                return (byte) result;
            }

            set
            {
                var countBytesToRead = 8 / countLsBits;
                var startIndex = index * countBytesToRead;
                if (countBytesToRead + startIndex > data.Length)
                    throw new IndexOutOfRangeException();

                var toWrite = value;
                var mask = 128 | (countLsBits == 2 ? 64 : 0); // работает только с моим набором занчений
                var clearMask = 255 - (1 | countLsBits); // работает только с моим набором занчений
                var bitShift = 8 - countLsBits;
                for (int i = 0; i < countBytesToRead; i++)
                {
                    data[startIndex + i] &= (byte)clearMask;
                    data[startIndex + i] |= (byte)((toWrite & mask) >> bitShift);
                    toWrite <<= countLsBits;
                }
            }
        }
    }
}
