using ConsoleUILib.Exceptions;
using ConsoleUILib.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib
{
    /// <summary>
    /// The console canvas contains a bunch of useful methods for drawing to the console.
    /// </summary>
    public class ConsoleCanvas {
        private const string fillChar = "█";

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="w">W</param>
        /// <param name="h">H</param>
        /// <param name="clr">Color for background</param>
        public static void DrawRect(int x, int y, int w, int h, Color clr) {
            Console.ResetColor();
            SetBackgroundColor(clr);
            string lineFill = " ".Repeat(w);

            for (int yT = y; yT < y + h; yT++) {
                Console.SetCursorPosition(x, yT);
                Console.Write(lineFill);
            }
        }

        /// <summary>
        /// Draws a rect with a gradient.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="w">W</param>
        /// <param name="h">H</param>
        /// <param name="orientation">The orientation to use</param>
        /// <param name="start">The start color of the gradient</param>
        /// <param name="end">The end color</param>
        public static void DrawRectGradient(int x, int y, int w, int h, Orientation orientation, Color start, Color end) {
            if (orientation == Orientation.VERTICAL)
                DrawRectGradientVertically(x, y, w, h, start, end);
            else
                DrawRectGradientHorizontally(x, y, w, h, start, end);
        }

        private static void DrawRectGradientVertically(int x, int y, int w, int h, Color start, Color end) {
            if (h < 2) throw new ArgumentOutOfRangeException("h", "Height must be more than or equal to 2 for vertical gradients.");

            Console.ResetColor();
            string lineFill = " ".Repeat(w);
            List<Color> gradient = GetGradients(start, end, h).ToList();

            for (int yT = y; yT < y + h; yT++) {
                SetBackgroundColor(gradient[yT - y]);
                Console.SetCursorPosition(x, yT);
                Console.Write(lineFill);
            }
        }

        private static void DrawRectGradientHorizontally(int x, int y, int w, int h, Color start, Color end) { // TODO: Generate full line to improve performance as well
            if (w < 2) throw new ArgumentOutOfRangeException("w", "Width must be more than or equal to 2 for horizontal gradients.");

            Console.ResetColor();
            List<Color> gradient = GetGradients(start, end, w).ToList();


            string line = "";

            for (int xOff = 0; xOff < w; xOff++)
            {
                line += GetBackgroundColorString(gradient[xOff]) + " ";
            }

            for (int yT = y; yT < y + h; yT++) {
                Console.SetCursorPosition(x, yT);
                Console.Write(line);
            }
        }

        /// <summary>
        /// Draws a string without any boundary restriction
        /// </summary>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fore">Foreground Color</param>
        /// <param name="back">Background Color</param>
        /// <returns>OverflowState containing which way the string has overflown</returns>
        public static OverflowState DrawString(string str, int x, int y, Color fore, Color back) {
            return DrawString(str, x, y, 1000, 1000, fore, back);
        }

        /// <summary>
        /// Draws a string without any boundary restriction, but also skips all the checks and adjustments to the bounds.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fore">Foreground Color</param>
        /// <param name="back">Background Color</param>
        public static void DrawStringWithoutBoundsCheck(string str, int x, int y, Color fore, Color back)
        {
            Console.ResetColor();
            SetForegroundColor(fore);
            SetBackgroundColor(back);

            Console.SetCursorPosition(x, y);
            Console.Write(str);
        }

        /// <summary>
        /// Draws a string with boundary restriction
        /// </summary>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="fore">Foreground Color</param>
        /// <param name="back">Background Color</param>
        /// <returns>OverflowState containing which way the string has overflown</returns>
        public static OverflowState DrawString(string str, int x, int y, int w, int h, Color fore, Color back, int lineScrollText = 0) {
            List<string> tempLines = str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            List<string> lines = new();
            OverflowState state = OverflowState.NONE;

            Console.ResetColor();
            SetForegroundColor(fore);
            SetBackgroundColor(back);

            foreach (string line in tempLines) {
                if (line.Length > w) {
                    int wadd = 0;

                    while (wadd < line.Length - w) {
                        lines.Add(line.Substring(wadd, w));
                        wadd += w;
                        state = OverflowState.HORIZONTAL;

                    }
                } else {
                    lines.Add(line);
                }
            }

            lines = lines.Skip(lineScrollText).ToList();

            int off = 0;
            foreach (string line in lines) {
                Console.SetCursorPosition(x, y + off);
                Console.Write(line);

                off++;

                if (off >= h)
                {
                    if(lines.Count > h) state = state == OverflowState.HORIZONTAL ? OverflowState.BOTH : OverflowState.VERTICAL;
                    break;
                }
            }

            return state;
        }

        /// <summary>
        /// Sets the foreground color to any RGB24 color
        /// </summary>
        /// <param name="clr">Color</param>
        public static void SetForegroundColor(Color clr) {
            Console.Write("\x1b[38;2;" + clr.R + ";" + clr.G + ";" + clr.B + "m");
        }

        /// <summary>
        /// Sets the background color to any RGB24 color
        /// </summary>
        /// <param name="clr">Color</param>
        public static void SetBackgroundColor(Color clr) {
            Console.Write("\x1b[48;2;" + clr.R + ";" + clr.G + ";" + clr.B + "m");
        }

        /// <summary>
        /// Generates the string used to set the background color to any RGB24 color
        /// </summary>
        /// <param name="clr">Color</param>
        public static string GetBackgroundColorString(Color clr)
        {
            return "\x1b[48;2;" + clr.R + ";" + clr.G + ";" + clr.B + "m";
        }

        /// <summary>
        /// This gets all colors in a gradient
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="steps">How many color points you want</param>
        /// <returns></returns>
        public static IEnumerable<Color> GetGradients(Color start, Color end, int steps) {
            if (steps < 2) throw new ArgumentOutOfRangeException("steps", "Must be at least 2");
            int stepA = ((end.A - start.A) / (steps - 1));
            int stepR = ((end.R - start.R) / (steps - 1));
            int stepG = ((end.G - start.G) / (steps - 1));
            int stepB = ((end.B - start.B) / (steps - 1));

            for (int i = 0; i < steps; i++) {
                yield return Color.FromArgb(start.A + (stepA * i),
                                            start.R + (stepR * i),
                                            start.G + (stepG * i),
                                            start.B + (stepB * i));
            }
        }
    }
}
