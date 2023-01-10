using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace SpiritHunterEXGtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0) { Console.WriteLine("No arguments provided."); Console.ReadKey(); return; }
            BinaryReader br = new BinaryReader(new FileStream(args[0], FileMode.Open));
            if (ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)) != "EXG") 
            {
                Console.WriteLine("Not a valid EXG file. Press any key to close.");
                Console.ReadKey();
                return;
            };

            br.BaseStream.Position = 0x10;
            int Width = br.ReadUInt16();
            int Height = br.ReadUInt16();
            Console.WriteLine($"{Height} {Width}"); 

            Bitmap bmp = new Bitmap(Width, Height);
            br.BaseStream.Position = 0x28;

            for(int x = 0; x < Height; x++) 
            { 
                for (int y = 0; y < Width; y++)
                {
                    int R = (int)br.ReadByte();
                    int G = (int)br.ReadByte();
                    int B = (int)br.ReadByte();
                    int A = (int)br.ReadByte();

                    bmp.SetPixel(y, x, Color.FromArgb(A, B, G, R));

                }
            
            }
EXIT:
            br.Close();

            bmp.Save($"{args[0]}.png");
            bmp.Dispose();

        }
    }
}
