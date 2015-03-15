using System;
using System.Collections.Generic;
using System.Drawing;
using ImageProcessor;
using ImageProcessor.Common.Exceptions;
using ImageProcessor.Imaging;
using ImageProcessor.Processors;

namespace PMAConverter {
    /// <summary> Describes convertion direction. </summary>
    public enum ColorChange {
        /// <summary> Convert non-premultiplied alpha to premultiplied alpha. </summary>
        ToPma,

        /// <summary> Convert premultiplied alpha to non-premultiplied alpha. </summary>
        FromPma
    }

    /// <summary> Encapsulates methods to change the image to and from premultiplied alpha format. </summary>
    public class ChangePremultipliedAlphaProcessor : IGraphicsProcessor {
        /// <summary> Initializes a new instance of the <see cref="ChangePremultipliedAlphaProcessor"/> class. </summary>
        public ChangePremultipliedAlphaProcessor() {
            Settings = new Dictionary<string, string>();
        }
        /// <summary> Gets or sets the dynamic parameter. </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary> Gets or sets any additional settings required by the processor. </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary> Processes the image. </summary>
        /// <param name="factory"> 
        /// The current instance of the <see cref="T:ImageProcessor.ImageFactory"/> class containing
        /// the image to process. </param>
        /// <returns>
        /// The processed image from the current instance of the <see cref="T:ImageProcessor.ImageFactory"/> class.
        /// </returns>
        public Image ProcessImage(ImageFactory factory) {
            Bitmap newImage = null;
            var image = factory.Image;
            try {
                ColorChange convert = DynamicParameter;
                newImage = new Bitmap(image);
                newImage = ChangeAlpha(newImage, convert);

                image.Dispose();
                image = newImage;
            } catch (Exception ex) {
                if (newImage != null) { newImage.Dispose(); }

                throw new ImageProcessingException("Error while processing image with " + GetType().Name + ":" + 
                    Environment.NewLine + ex.Message , ex);
            }

            return image;
        }

        private static Bitmap ChangeAlpha(Image source, ColorChange conversion) {
            using (var bitmap = new FastBitmap(source)) {
                for (int y = 0; y < source.Height; y++) {
                    for (int x = 0; x < source.Width; x++) {
                        // ReSharper disable AccessToDisposedClosure
                        Color color = bitmap.GetPixel(x, y);
                        switch (conversion) {
                            case ColorChange.ToPma:
                                color = color.ToPremultiplied();
                                break;
                            case ColorChange.FromPma:
                                color = color.ToNonPremultiplied();
                                break;
                        }

                        bitmap.SetPixel(x, y, color);
                        // ReSharper restore AccessToDisposedClosure
                    }
                }
            }

            return (Bitmap) source;
        }
    }
}
