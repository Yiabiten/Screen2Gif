using System;
using System.Runtime.Serialization;

namespace ScreenToGif.ImageUtil.Decoder
{
    [Serializable]
    public class GifDecoderException : Exception
    {
        public GifDecoderException() { }
        public GifDecoderException(string message) : base(message) { }
        public GifDecoderException(string message, Exception inner) : base(message, inner) { }
        public GifDecoderException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
