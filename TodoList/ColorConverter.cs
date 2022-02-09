using System;
using System.Windows.Media;
namespace TodoList
{
    class ColorConverter
    {
        public static SolidColorBrush GetColorFromHexa(string hex)
        {
            string hexaColor = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hexaColor.Length == 8)
            {
                a = byte.Parse(hexaColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hexaColor.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hexaColor.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hexaColor.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }
    }
}
