using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib
{
    public class Themes
    {
        public static SKColor[] BASE_THEME = new SKColor[]
        {
            SKColors.ForestGreen,
            SKColors.MediumVioletRed,
            SKColors.Goldenrod,
            SKColors.CornflowerBlue,
            SKColors.SaddleBrown,
            SKColors.DarkOrange,
        };

        public static SKColor[] RAINBOW_THEME = new SKColor[] {
            new SKColor(0xFFBD2014),
            new SKColor(0xFFD6711B),
            new SKColor(0xFFE8BF19),
            new SKColor(0xFF72E023),
            new SKColor(0xFF3279B6),
            new SKColor(0xFF4A2CD5),
        };

        public static SKColor[] OLD_BLUE_COLOR_THEME = new SKColor[] {
            new SKColor(0xFF3A2194),
            new SKColor(0xFF5471AB),
            new SKColor(0xFF12686B),
            new SKColor(0xFF70B3AD),
            new SKColor(0xFFA3CEE0),
            new SKColor(0xFF031157),
        };

        public static SKColor[] BLUE_COLOR_THEME = new SKColor[] {
            new SKColor(0xFFA1B255),
            new SKColor(0xFF0B2945),
            new SKColor(0xFF11406B),
            new SKColor(0xFF1F6DB2),
            new SKColor(0xFF2C9CFF),
            new SKColor(0xFF9CCAFF),
        };

        public static SKColor[] NATURAL_COLOR_THEME = new SKColor[] {
            new SKColor(0xFF0D0908),
            new SKColor(0xFF30211F),
            new SKColor(0xFF5C403C),
            new SKColor(0xFF966760),
            new SKColor(0xFFEBA396),
            new SKColor(0xFFFFE6E5),
        };
    }
}
