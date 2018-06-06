using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MesAutoLoaderLib.Exceptions
{
    class FileFormatException : Exception, ISerializable
    {
        public FileFormatException()
        : base("FileFormatException") { }
        public FileFormatException(string message) 
        : base(message) { }
        public FileFormatException(string message, Exception inner) 
        : base(message, inner) { }
        protected FileFormatException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
    }
}
