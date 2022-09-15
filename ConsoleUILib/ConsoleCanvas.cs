using ConsoleUILib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib
{
    public class ConsoleCanvas
    {
        private const string fillChar = "█";

        public static void DrawRect(int x, int y, int w, int h, ConsoleColor clr)
        {
            Console.ResetColor();
            Console.ForegroundColor = clr;
            string lineFill = fillChar.Repeat(w);

            for (int yT = y; yT < y + h; yT++)
            {
                Console.SetCursorPosition(x, yT);
                Console.Write(lineFill);
            }
        }

        public static void DrawString(string str, int x, int y, ConsoleColor fore, ConsoleColor back)
        {
            DrawString(str, x, y, 1000, 1000, fore, back);
        }

        public static void DrawString(string str, int x, int y, int w, int h, ConsoleColor fore, ConsoleColor back, int lineScrollText = 0)
        {
            List<string> tempLines = str.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            List<string> lines = new();

            Console.ResetColor();
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;

            foreach(string line in tempLines)
            {
                if(line.Length > w)
                {
                    int wadd = 0;

                    while(wadd < line.Length-w)
                    {
                        lines.Add(line.Substring(wadd, w));
                        wadd += w;
                    }
                }else
                {
                    lines.Add(line);
                }
            }

            lines = lines.Skip(lineScrollText).ToList();

            int off = 0;
            foreach(string line in lines)
            {
                Console.SetCursorPosition(x, y+off);
                Console.Write(line);

                off++;

                if (off >  h) break;
            }
        }
    }
}
