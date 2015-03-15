using System;
using System.Drawing;

namespace PMAConverter {
    /// <summary> Color manipulation tools. </summary>
    public static class ColorExtensions {
        /// <summary> Translate a non-premultipled alpha <see cref="Color"/> to a <see cref="Color"/>
        /// that contains premultiplied alpha. </summary>
        /// <remarks>Obviously, there is a loss of precision, so multiple conversions to and from PMA
        /// degrade color quality.</remarks>
        /// <param name="r">Red component value.</param>
        /// <param name="g">Green component value.</param>
        /// <param name="b">Blue component value.</param>
        /// <param name="a">Alpha component value.</param>
        /// <returns>A <see cref="Color"/> which contains premultiplied alpha data.</returns>
        public static Color ToPremultiplied(int a, int r, int g, int b) {
            return Color.FromArgb(
                a,
                (byte) (r * a / 255),
                (byte) (g * a / 255),
                (byte) (b * a / 255));
        }

        /// <summary> Translate a non-premultipled alpha <see cref="Color"/> to a <see cref="Color"/>
        /// that contains premultiplied alpha. </summary>
        /// <remarks>Obviously, there is a loss of precision, so multiple conversions to and from PMA 
        /// degrade color quality.</remarks>
        /// <param name="color">Color to translate.</param>
        /// <returns>A <see cref="Color"/> which contains premultiplied alpha data.</returns>
        public static Color ToPremultiplied(this Color color) {
            return ToPremultiplied(color.A, color.R, color.G, color.B);
        }

        /// <summary> Translate a premultipled alpha <see cref="Color"/> to a <see cref="Color"/>
        /// that contains non-premultiplied alpha. </summary>
        /// <remarks>Obviously, there is a loss of precision, so multiple conversions to and from PMA 
        /// degrade color quality.</remarks>
        /// <param name="r">Red component value.</param>
        /// <param name="g">Green component value.</param>
        /// <param name="b">Blue component value.</param>
        /// <param name="a">Alpha component value.</param>
        /// <returns>A <see cref="Color"/> which contains non-premultiplied alpha data.</returns>
        public static Color ToNonPremultiplied(int a, int r, int g, int b) {
            return Color.FromArgb(
                a,
                (byte) Math.Ceiling(r * 255f / a),
                (byte) Math.Ceiling(g * 255f / a),
                (byte) Math.Ceiling(b * 255f / a));
        }

        /// <summary> Translate a premultipled alpha <see cref="Color"/> to a <see cref="Color"/>
        /// that contains non-premultiplied alpha. </summary>
        /// <remarks>Obviously, there is a loss of precision, so multiple conversions to and from PMA 
        /// degrade color quality.</remarks>
        /// <param name="color">Color to translate.</param>
        /// <returns>A <see cref="Color"/> which contains non-premultiplied alpha data.</returns>
        public static Color ToNonPremultiplied(this Color color) {
            return ToNonPremultiplied(color.A, color.R, color.G, color.B);
        }
    }
}
