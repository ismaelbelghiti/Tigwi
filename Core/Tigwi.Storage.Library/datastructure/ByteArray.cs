using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Tigwi.Storage.Library
{
    [ProtoContract]
    public class ByteArray
    {
        [ProtoMember(1)]
        Byte[] bytes;

        public ByteArray()
        {
        }

        public ByteArray(Byte[] bytes)
        {
            this.bytes = bytes;
        }

        public Byte[] Bytes
        {
            get
            {
                if (bytes == null)
                    bytes = new Byte[0];
                return bytes;
            }
            set
            {
                bytes = value;
            }
        }
    }
}
