using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace C_I
{
    //query_data
    class Program
    {
        static void Main(string[] args)
        {
            string[,] FF = new FormatFile().readIt(args[1]);
            IndexStructure Idx = new IndexStructure().ReadFromFile(args[2]);
            PointerBlock pb = Idx.Find(Convert.ToInt32(args[4]));
            BinaryReader br = new BinaryReader(File.Open(args[0], FileMode.Open));
            Console.WriteLine(args[3] + " : " + args[4] + " olan kayit(lar) listeleniyor..:\n");
            Console.WriteLine("---------------------------------------------");
            int totalSize = 0; // toplam kayýt boyutu
            for (int i = 0; i < (FF.Length / 5); i++)
                totalSize += Convert.ToInt32(FF[i, 2]);
            //pointerlist içerisindeki pointerlarýn gösterdiði bütün kayýtlarý getir
            int sayac = 1;
            while(true)
            {
                for (int i = 0; i < pb.pointerList.Length; i++)
                {
                    if (pb.pointerList[i] != 0)
                    {
                        if (pb.pointerList[i] == -1)
                            Console.WriteLine("Kayit Bulunamadi!");
                        else
                        {
                            br.BaseStream.Seek(pb.pointerList[i] * totalSize, SeekOrigin.Begin);
                            for (int j = 0; j < (FF.Length / 5); j++)
                            {
                                if (FF[j, 1] == "String")
                                {
                                    Console.WriteLine(FF[j, 0] + " : " + new string(Array.ConvertAll<byte, char>(br.ReadBytes(Convert.ToInt32(FF[j, 2])), Convert.ToChar)));
                                }
                                else
                                {
                                    for (int k = 0; k < (Convert.ToInt32(FF[j, 2]) - 4); k++)
                                        br.ReadByte();
                                    Console.WriteLine(FF[j, 0] + " : " + br.ReadInt32());
                                }
                            }
                        }
                        Console.WriteLine(sayac+"...---------------------------------------------");
                    }
                    sayac++;
                }
                //link varsa devam..yoksa bitti..
                if (pb.link == null)
                    break;
                else
                    pb = pb.link;
            }
        }
    }
}
