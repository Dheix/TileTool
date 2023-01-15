using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Lifetime;

namespace TileTool
{
    internal class Program
    {
        static void ObjToBin(string FilePath, string FilePathTo, bool ToDebug)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.ASCII);
            
            FileStream fsw = new FileStream(FilePathTo, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fsw);
            

            var counter = sr.Read();
            while( counter == 35) //ignore OBJ comments
            {
                sr.ReadLine();
                counter = sr.Read();
            }

            if(counter == 109 && sr.Read() == 116 && sr.Read() == 108 && sr.Read() == 108 && sr.Read() == 105 && sr.Read() == 98) //mtllib
            {
                sr.Read();
                if (ToDebug)
                {
                    Console.WriteLine(sr.ReadLine());
                }
                counter = sr.Read();
            }

            int Vcounter = 0;
            int VdeCounter = 0;

            for (var i = 0; i < 11; i++)
            {
                Int16 padd = 0;
                bool skipV = false;
                bool skipF = false;
                bool leave = false;
                VdeCounter += Vcounter;
                Vcounter = 0;
                if (counter == 111) //o
                {
                    int layer = int.Parse(Regex.Match(sr.ReadLine(), @"\d+").Value);

                    int diff = Math.Abs(i - (layer - 1));
                    for (int xx = diff; xx >= 0; xx--)  //when layer is found, write empty layers that should be skipped
                    {
                        if (ToDebug)
                        {
                            Console.WriteLine(layer - xx + " layer");
                        }
                        if (xx == 0)
                        {
                            bw.Write(padd);
                        }
                        else
                        {
                            bw.Write(padd);
                            bw.Write(padd);
                        }
                    }
                    i += diff;
                    counter = sr.Read();
                    do //v
                    {
                        counter = sr.Read();
                        string wv = sr.ReadLine();
                        if (ToDebug)
                        {
                            Console.WriteLine(wv);
                        }
                        var xyz = wv.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        bw.Write(float.Parse(xyz[0]));
                        bw.Write(float.Parse(xyz[1]));
                        bw.Write(float.Parse(xyz[2]));

                        counter = sr.Read();
                        switch (counter)
                        {
                            case (102): //f
                                break;
                            case (115): //s
                                sr.ReadLine();
                                counter = sr.Read();
                                break;
                            case (118): //v
                                var peak = sr.Peek();
                                if (peak == 116) //t
                                {
                                    leave = true;
                                }
                                break;
                        }
                        Vcounter++;
                    }
                    while (counter == 118 && leave == false);

                    int HexVcounter = 0x0 + (Vcounter * 3 * 4 + 2);
                    if (ToDebug)
                    {
                        Console.WriteLine(HexVcounter + " offset");
                    }
                    bw.BaseStream.Seek(-HexVcounter, SeekOrigin.Current);

                    Int16 lenght = Convert.ToInt16(Vcounter);
                    bw.Write(lenght);
                    bw.BaseStream.Seek(HexVcounter - 2, SeekOrigin.Current);
                }
                else
                {
                    skipV = true;
                }
                
                if (counter == 102) //f
                {
                    int Fcounter = 0;
                    bw.Write(padd);

                    Int16 VcounterShort = Convert.ToInt16(VdeCounter);
                    if (ToDebug)
                    {
                        Console.WriteLine(VdeCounter + " Vcounters");
                    }
                    do //f
                    {
                        counter = sr.Read();
                        string wf = sr.ReadLine();
                        var fabc = wf.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        Int16 x1 = 1;
                        Int16 a =  Convert.ToInt16(fabc[0]);
                        Int16 b = Convert.ToInt16(fabc[1]);
                        Int16 c = Convert.ToInt16(fabc[2]);
                        a-= x1;
                        b-= x1;
                        c-= x1;
                        a -= VcounterShort;
                        b -= VcounterShort;
                        c -= VcounterShort;
                        if (ToDebug)
                        {
                            Console.WriteLine(a + " " + b + " " + c);
                        }
                        bw.Write(a);
                        bw.Write(b);
                        bw.Write(c);

                        counter = sr.Read();
                        Fcounter++;
                    }
                    while (counter == 102);

                    int HexVcounter = 0x0 + (Fcounter * 3 * 2 + 2);
                    if (ToDebug)
                    {
                        Console.WriteLine(HexVcounter + "FF");
                    }
                    bw.BaseStream.Seek(-HexVcounter, SeekOrigin.Current);

                    Int16 lenght = Convert.ToInt16(Fcounter * 3);
                    bw.Write(lenght);
                    bw.BaseStream.Seek(HexVcounter - 2, SeekOrigin.Current);
                }
                else
                {
                    skipF = true;
                }

                if(skipV == true && skipF == true) //write skipped layers if no layers has been found
                {
                    Console.WriteLine(1+i);
                    bw.Write(padd);
                    bw.Write(padd);
                }
            }
            Vcounter = 0;
            //read vectors with uv coords
            for (int ii = 0; ii < 10; ii++)
            {
                Int16 padd = 0;
                bool skipV = false;
                bool skipF = false;
                bool leave = false;
                if (counter == 111) //o
                {
                    int layer = int.Parse(Regex.Match(sr.ReadLine(), @"\d+").Value);
                    int diff = Math.Abs(ii - (layer - 1));
                    for (int xx = diff; xx >= 0; xx--)  //when layer is found, write empty layers that should be skipped
                    {
                        if (ToDebug)
                        {
                            Console.WriteLine(layer - xx + " Texture layer");
                        }
                        if (xx == 0)
                        {
                            bw.Write(padd);
                        }
                        else
                        {
                            bw.Write(padd);
                            bw.Write(padd);
                        }
                    }
                    ii += diff;
                    counter = sr.Read();
                    do //v
                    {
                        counter = sr.Read();
                        string wv = sr.ReadLine();
                        if (ToDebug)
                        {
                            Console.WriteLine(wv);
                        }
                        var xyz = wv.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        bw.Write(float.Parse(xyz[0]));
                        bw.Write(float.Parse(xyz[1]));
                        bw.Write(float.Parse(xyz[2]));
                        bw.Write(padd + padd);
                        bw.Write(padd + padd);
                        counter = sr.Read();
                        switch (counter)
                        {
                            case (102): //f
                                break;
                            case (115): //s
                                sr.ReadLine();
                                counter = sr.Read();
                                break;
                            case (118): //v
                                var peak = sr.Peek();
                                if (peak == 116) //t
                                {
                                    leave = true;
                                }
                                break;
                        }
                        Vcounter++;
                    }
                    while (counter == 118 && leave == false);

                    int HexVcounter = 0x0 + (Vcounter * 5 * 4 + 2);
                    if (ToDebug)
                    {
                        Console.WriteLine(HexVcounter + "offset");
                    }
                    bw.BaseStream.Seek(-HexVcounter, SeekOrigin.Current);

                    Int16 lenght = Convert.ToInt16(Vcounter);
                    bw.Write(lenght);
                    bw.BaseStream.Seek(HexVcounter - 2, SeekOrigin.Current);
                }
                else
                {
                    skipV = true;
                }

                if(counter == 118 && sr.Peek() == 116)
                {
                    sr.Read();
                    do
                    {
                        string vt = sr.ReadLine();
                        var uv = vt.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        float u = Convert.ToSingle(uv[0]);
                        float v = Convert.ToSingle(uv[1]);
                        if (ToDebug)
                        {
                            Console.WriteLine(u + " " + v);
                        }
                        int HexVcounter = 0x0 + ((Vcounter - 1) * 5 * 4 + 8);
                        bw.BaseStream.Seek(-HexVcounter, SeekOrigin.Current);
                        bw.Write(u);
                        bw.Write(v);
                        bw.BaseStream.Seek(HexVcounter - 8, SeekOrigin.Current);
                        Vcounter -= 1;
                        counter = sr.Read();
                    }
                    while (counter == 118 && sr.Read() == 116);
                }
                else
                {
                    skipF = true;
                }
                if (sr.Read() == 115 && sr.Read() == 101 && sr.Read() == 109 && sr.Read() == 116 && sr.Read() == 108)
                {
                    sr.ReadLine();
                }
                counter = sr.Read();
                do
                {
                    sr.ReadLine();
                    counter = sr.Read();
                }
                while (counter == 102);

                if (skipV == true && skipF == true) //write skipped layers if no layers has been found
                {
                    if (ToDebug)
                    {
                        Console.WriteLine(1 + ii);
                    }
                    bw.Write(padd);
                    bw.Write(padd);
                }
            }

            sr.Close();
            fs.Close();
            fsw.Close();
            bw.Close();
            
        }

        static void BinToObj(string FilePath, string FilePathTo, bool ToDebug)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US"); //change culture because OBJ requries decimal with dot

            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            FileStream fsw = new FileStream(FilePathTo, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fsw, Encoding.ASCII);


            System.IO.File.Copy("textures/TileToolTextures.mtl", System.IO.Path.Combine(Path.GetDirectoryName(FilePathTo), "TileToolTextures.mtl"), true);
            System.IO.File.Copy("textures/map_dashed_line.png", System.IO.Path.Combine(Path.GetDirectoryName(FilePathTo), "map_dashed_line.png"), true);
            System.IO.File.Copy("textures/map_solid_line.png", System.IO.Path.Combine(Path.GetDirectoryName(FilePathTo), "map_solid_line.png"), true);

            int counter = 0;
            float vr = 0;
            float vg = 0;
            float vb = 0;
            sw.WriteLine("mtllib TileToolTextures.mtl");
            for (int ii = 0; ii < 11; ii++)
            {
                Int16 max1 = br.ReadInt16(); //leanght of vertex list
                if(max1 > 0)
                {
                    string layer = "o layer_" + (1 +ii);
                    if (ToDebug)
                    {
                        Console.WriteLine(layer);
                    }
                    sw.WriteLine(layer);

                    
                    switch (ii + 1)
                    {
                        case 1:
                            vr = 0.816f;
                            vg = 0.816f;
                            vb = 0.773f;
                            break;
                        case 2:
                            vr = 0.643f;
                            vg = 0.722f;
                            vb = 0.455f;
                            break;
                        case 3:
                            vr = 0.89f;
                            vg = 0.816f;
                            vb = 0.549f;
                            break;
                        case 4:
                            vr = 0.322f;
                            vg = 0.722f;
                            vb = 0.82f;
                            break;
                        case 5:
                            vr = 1.0f;
                            vg = 1.0f;
                            vb = 1.0f;
                            break;
                        case 6:
                            vr = 0.545f;
                            vg = 0.427f;
                            vb = 0.357f;
                            break;
                        case 7:
                            vr = 0.345f;
                            vg = 0.243f;
                            vb = 0.173f;
                            break;
                        case 8:
                            vr = 0.322f;
                            vg = 0.722f;
                            vb = 0.82f;
                            break;
                        case 9:
                            vr = 0.278f;
                            vg = 0.639f;
                            vb = 0.722f;
                            break;
                        case 10:
                            vr = 0.239f;
                            vg = 0.553f;
                            vb = 0.624f;
                            break;
                        case 11:
                            vr = 0.196f;
                            vg = 0.475f;
                            vb = 0.522f;
                            break;
                    }
                }
                /*else
                {
                    string layer = "# skip layer_" + (1 + ii);
                    Console.WriteLine(layer);
                    sw.WriteLine(layer);
                }*/
                


                for (int i = 0; i < max1; i++) //write vertices
                {

                    string floatout = "v " + br.ReadSingle() + " " + br.ReadSingle() + " " + br.ReadSingle() + " " + vr + " " + vg + " " + vb;
                    if (ToDebug)
                    {
                        Console.WriteLine(floatout);
                    }
                    sw.WriteLine(floatout);

                }


                Int16 max2 = br.ReadInt16(); //leanght of polygon faces list
                max2 /= 3;

                /*if (ii != 0 && max1 > 0 && ii < 11)
                {
                        counter += max1;
                }*/

                for (int i = 0; i < max2; i++) //write faces
                {
                    string intout = "f " + (br.ReadInt16() + 1 + counter) + " " + (br.ReadInt16() + 1 + counter) + " " + (br.ReadInt16() + 1 + counter);
                    if (ToDebug)
                    {
                        Console.WriteLine(intout);
                    }
                    sw.WriteLine(intout);
                }

                
                 counter += max1;
                
            }


            float u;
            float v;
            float UVcounter = 0;
            float max4 = counter;
            float max5 = 0;
            //read vectors with uv coords
            for (int iii = 0; iii < 10; iii++)
            {
                var list_v = new List<float>();
                var list_uv = new List<float>();

                Int16 max3 = br.ReadInt16();
                if (max3 > 0) //o
                {
                    if (ToDebug)
                    {
                        Console.WriteLine("textured layer_" + (iii + 1));
                    }
                    sw.WriteLine("o Tlayer_" + (iii + 1) /*+ " layer lenght " + max3*/);
                }

                
                var errorAtline = new List<float>();

                for (int i = 0; i < max3; i++) //v
                {
                    bool error = false;
                    float v1 = br.ReadSingle();
                    if(float.IsNaN(v1) || float.IsInfinity(v1))
                    {
                        v1 = float.NaN;
                        errorAtline.Add(i);
                        error = true;
                    }

                    float v2 = br.ReadSingle();
                    if(float.IsNaN(v2) || float.IsInfinity(v2) || error)
                    {
                        v2 = float.NaN;
                        if(error == false)
                        {
                            errorAtline.Add(i);
                            error = true;
                        }
                    }

                    float v3 = br.ReadSingle();
                    if(float.IsNaN(v3) || float.IsInfinity(v3) || error)
                    {
                        v3 = float.NaN;
                        if (error == false)
                        {
                            errorAtline.Add(i);
                        }
                    }

                    string floatout2 = "v " + v1 + " " + v2 + " " + v3;
                    //Console.WriteLine(floatout2);
                    sw.WriteLine(floatout2);
                    

                    //string floatout3 = br.ReadSingle() + " " + br.ReadSingle();
                    //Console.WriteLine(floatout3);
                    
                    u = br.ReadSingle();
                    if(float.IsNaN(u) || float.IsInfinity(u))
                    {
                        u = 0;
                    }
                    v = br.ReadSingle();

                    
                    list_uv.Add(u);
                    list_uv.Add(v);
                    list_v.Add(v);
                }
                var sds = list_uv.ToArray();
                var vtlist = new List<float>();
                for (int r = 0; r < sds.Length; r++) //vt
                {
                    float VTu = sds[r];
                    string vtout = "vt " + VTu;
                    vtlist.Add(r);
                    r++;
                    float VTv = sds[r];
                    vtout = vtout + " " + VTv;
                    sw.WriteLine(vtout);
                    vtlist.Add(r);
                }

                if(max3 > 0 && iii % 2 == 0)
                {
                    sw.WriteLine("usemtl map_solid_line");
                }
                else if(max3 > 0 && iii % 2 == 1)
                {
                    sw.WriteLine("usemtl map_dashed_line");
                }

                if (max3 > 0 && iii < 10)
                {
                    UVcounter = max4;
                }

                var dsd = list_v.ToArray();
                bool isQuad = false;
                bool isQuadInsequence = false;
                var vtlist_ar = vtlist.ToArray();
                int rr = 0;
                for (int r = 0; r <dsd.Length; r++)//f
                {
                    float x = 0;
                    float y = 0;
                    float z = 0;
                    float w = 0;
                    if (r == rr)
                    {
                        x += r + 1;
                        r++;
                    }
                    if(r == rr+1)
                    {
                        y += r + 1;
                        r++;
                    }
                    if(r  == rr +2)
                    {
                        z += r + 1;
                        r++;
                    }
                    if(r == rr + 3)
                    {
                        w += r + 1;
                        if ((r + 1) == dsd.Length)
                        {
                            max4 += Math.Max(Math.Max(Math.Max(x, y), z), w);
                        }

                        if (errorAtline.Contains(x) || errorAtline.Contains(y) || errorAtline.Contains(z) || errorAtline.Contains(w))
                        {
                            x = 0f - UVcounter;
                            y = 0f - UVcounter;
                            z = 0f - UVcounter;
                            w = 0f - UVcounter;
                        }
                        string UVout = "f " + (x + UVcounter) + "/" + (vtlist_ar[(r - 2)] + max5) + " " + (y + UVcounter) + "/" + (vtlist_ar[(r - 1)] + max5) + " " + (z + UVcounter) + "/" + (vtlist_ar[(r - 0)] + max5) + " " + (w + UVcounter) + "/" + (vtlist_ar[(r + 1)] + max5);
                        if (ToDebug)
                        {
                            Console.WriteLine(UVout);
                        }
                        sw.WriteLine(UVout);
                    }
                    rr += 4;
                }




                /*for (int r = 0; r < dsd.Length; r++) //f
                {
                    float x = 0;
                    float y = 0;
                    float z = 0;
                    float w = 0;
                    if (dsd[r] == 1)
                    {              
                        x+=r + 1;
                        isQuad = true;
                        r++;                     
                    }
                    if(isQuad && dsd[r] == 0)
                    {
                        y += r + 1;
                        isQuadInsequence = true;
                        r++;
                    }
                    if(isQuad && isQuadInsequence && dsd[r] == 0)
                    {    
                        z += r + 1;
                        r++;
                    }
                    if(dsd[r] == 1 && isQuad && isQuadInsequence)
                    {
                        w += r + 1;
                        if ((r +1) == dsd.Length)
                        {
                            max4 += Math.Max(Math.Max(Math.Max(x, y), z), w);
                        }

                        if(errorAtline.Contains(x) || errorAtline.Contains(y) || errorAtline.Contains(z) || errorAtline.Contains(w))
                        {
                            x = 0f - UVcounter;
                            y = 0f - UVcounter;
                            z = 0f - UVcounter;
                            w = 0f - UVcounter;
                        }
                        string UVout = "f " + (x + UVcounter) + "/" + (vtlist_ar[(r - 2)] + max5) + " " + (y + UVcounter) + "/" + (vtlist_ar[(r - 1)] + max5) + " " + (z + UVcounter) + "/" + (vtlist_ar[(r - 0)] + max5) + " " + (w + UVcounter) + "/" + (vtlist_ar[(r + 1)] + max5);
                        Console.WriteLine(UVout);
                        sw.WriteLine(UVout);
                        isQuad = false;
                        isQuadInsequence= false;
                        
                    }

                }*/
        
                max5 += dsd.Length;
            }


            fs.Close();
            br.Close();

            sw.Close();
            fsw.Close();

        }
        static void Main(string[] args)
        {
            string Pfrom = "";
            string Pto = "";
            string toswtich = "";
            string ext = "";
            bool ToDebug = true;
            while (toswtich != "quit")
            {
                toswtich = "";
                toswtich = Console.ReadLine();
                
                Pfrom = toswtich.Substring(0);
                Pfrom = Pfrom.Trim();
                ext = Path.GetExtension(Pfrom);
                if (ext == ".bin")
                {
                    Console.WriteLine("To obj");
                    if (File.Exists(Pfrom))
                    {
                        try
                        {
                            Pto = Path.ChangeExtension(Pfrom, ".obj");
                            BinToObj(Pfrom, Pto,ToDebug);
                            Console.WriteLine("Done");
                        }
                        catch
                        {
                            Console.WriteLine("Error converting to .obj");
                        }
                    }
                    else
                    {
                        Console.WriteLine("File path doesn't exist");
                    }
                }
                else if(ext == ".obj")
                { 
                    Console.WriteLine("To bin");
                    if ( File.Exists(Pfrom))
                    {
                        //try
                        //{
                            Pto = Path.ChangeExtension(Pfrom, ".bin");
                            ObjToBin(Pfrom, Pto,ToDebug);
                            Console.WriteLine("Done");
                        //}
                        /*catch
                        {
                            Console.WriteLine("Error converting to .bin");
                        }*/
                    }
                    else
                    {
                        Console.WriteLine("File path doesn't exist");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong file format");
                }
            }
            


            /*
            while (toswtich != "quit")
            {
                toswtich = "";
                toswtich = Console.ReadLine();
                for (int i = 7; i > 0; i--)
                {
                    try
                    {
                        Pfrom = toswtich.Substring(i);
                        toswtich = toswtich.Substring(0, i);
                        break;
                    }
                    catch
                    {
                        //Console.WriteLine(i);
                    }
                }
                switch (toswtich)
                {
                    case "bin2obj":
                        Console.WriteLine("to obj");
                        Pfrom = Pfrom.Trim();
                        ext = Path.GetExtension(Pfrom);
                        if(ext == ".bin" && File.Exists(Pfrom))
                        {
                            Pto = Path.ChangeExtension(Pfrom, ".obj");
                            BinToObj(Pfrom,Pto);
                        }
                        else
                        {
                            Console.WriteLine("Wrong file format or path");
                        }
                        break;
                    case "obj2bin":
                        Console.WriteLine("to bin");
                        Pfrom = Pfrom.Trim();
                        ext = Path.GetExtension(Pfrom);
                        if (ext == ".obj" && File.Exists(Pfrom))
                        {
                            Pto = Path.ChangeExtension(Pfrom, ".bin");
                            ObjToBin(Pfrom, Pto);
                        }
                        else
                        {
                            Console.WriteLine("Wrong file format or path");
                        }
                        break;
                }
            }*/

        }
    }
}
