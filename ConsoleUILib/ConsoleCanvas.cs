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
    public class ConsoleCanvas {
        private const string fillChar = "█";

        public static void DrawRect(int x, int y, int w, int h, Color clr) {
            Console.ResetColor();
            SetForegroundColor(clr);
            string lineFill = fillChar.Repeat(w);

            for (int yT = y; yT < y + h; yT++) {
                Console.SetCursorPosition(x, yT);
                Console.Write(lineFill);
            }
        }

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

        private static void DrawRectGradientHorizontally(int x, int y, int w, int h, Color start, Color end) {
            if (w < 2) throw new ArgumentOutOfRangeException("w", "Width must be more than or equal to 2 for horizontal gradients.");

            Console.ResetColor();
            List<Color> gradient = GetGradients(start, end, w).ToList();

            for (int yT = y; yT < y + h; yT++) {
                Console.SetCursorPosition(x, yT);

                for(int xOff = 0; xOff < w; xOff++) {
                    SetBackgroundColor(gradient[xOff]);
                    Console.Write(" ");
                }
            }
        }

        public static void DrawString(string str, int x, int y, Color fore, Color back) {
            DrawString(str, x, y, 1000, 1000, fore, back);
        }

        public static void DrawString(string str, int x, int y, int w, int h, Color fore, Color back, int lineScrollText = 0) {
            List<string> tempLines = str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            List<string> lines = new();

            Console.ResetColor();
            SetForegroundColor(fore);
            SetBackgroundColor(back);

            foreach (string line in tempLines) {
                if (line.Length > w) {
                    int wadd = 0;

                    while (wadd < line.Length - w) {
                        lines.Add(line.Substring(wadd, w));
                        wadd += w;
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

                if (off > h) break;
            }
        }

        public static void SetForegroundColor(Color clr) {
            Console.Write("\x1b[38;2;" + clr.R + ";" + clr.G + ";" + clr.B + "m");
        }

        public static void SetBackgroundColor(Color clr) {
            Console.Write("\x1b[48;2;" + clr.R + ";" + clr.G + ";" + clr.B + "m");
        }

        public static IEnumerable<Color> GetGradients(Color start, Color end, int steps) {
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

    public enum Orientation {
        HORIZONTAL,
        VERTICAL
    }
}
