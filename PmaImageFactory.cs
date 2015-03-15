using System.IO;
using ImageProcessor;

namespace PMAConverter {
    /// <summary> Extended image factory. </summary>
    public class PmaImageFactory : ImageFactory {
        /// <summary> Initializes a new instance of the <see cref="PmaImageFactory"/> class. </summary>
        /// <param name="preserveExifData">if set to <c>true</c>, will preserve EXIF data.</param>
        public PmaImageFactory(bool preserveExifData = false)
            : base(preserveExifData) {
        }

        /// <summary> Changes the image to and from premultiplied alpha format. </summary>
        /// <param name="changeDirection"> Conversion direction. </param>
        /// <returns> The current instance of the <see cref="PmaImageFactory"/> class. </returns>
        public PmaImageFactory ChangePremultipliedAlpha(ColorChange changeDirection) {
            if (ShouldProcess) {
                var alpha = new ChangePremultipliedAlphaProcessor { DynamicParameter = changeDirection };
                CurrentImageFormat.ApplyProcessor(alpha.ProcessImage, this);
            }

            return this;
        }

        /// <summary> Loads the image to process. Always call this method first. </summary>
        /// <param name="stream"> The <see cref="Stream"/> containing the image information. </param>
        /// <returns> The current instance of the <see cref="PmaImageFactory"/> class. </returns>
        public new PmaImageFactory Load(Stream stream) {
            base.Load(stream);
            return this;
        }
        /// <summary> Loads the image to process. Always call this method first. </summary>
        /// <param name="imagePath">The absolute path to the image to load.</param>
        /// <returns> The current instance of the <see cref="PmaImageFactory"/> class. </returns>
        public new PmaImageFactory Load(string imagePath) {
            base.Load(imagePath);
            return this;
        }
    }
}
