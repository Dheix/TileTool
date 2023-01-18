using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Lifetime;
using System.Linq;
using System.IO.Compression;

namespace TileTool
{
    internal class Program
    {
        static void WriteV(string k, BinaryWriter bw)
        {


            var xyz = k.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bw.Write(float.Parse(xyz[0]));
            bw.Write(float.Parse(xyz[1]));
            bw.Write(float.Parse(xyz[2]));
            //Console.WriteLine(xyz[0] + " " + xyz[1] + " " + xyz[2]);
        }
        static void WriteF(string k, BinaryWriter bw, int f_layer_sub)
        {
            
            var xyz = k.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    xyz[i] = xyz[i].Substring(0, xyz[i].IndexOf("/"));
                }
            }
            catch { }
            int n1 = int.Parse(xyz[0]) - f_layer_sub - 1;
            int n2 = int.Parse(xyz[1]) - f_layer_sub - 1;
            int n3 = int.Parse(xyz[2]) - f_layer_sub - 1;
            bw.Write(Convert.ToInt16(n1));
            bw.Write(Convert.ToInt16(n2));
            bw.Write(Convert.ToInt16(n3));
            //Console.WriteLine(n1 + " " + n2 + " " + n3);
        }
        static void WriteV_VT(string k, BinaryWriter bw, string vt)
        {
            var xyz = k.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bw.Write(float.Parse(xyz[0]));
            bw.Write(float.Parse(xyz[1]));
            bw.Write(float.Parse(xyz[2]));
            var uv = vt.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bw.Write(float.Parse(uv[0]));
            bw.Write(float.Parse(uv[1]));
        }
        
 
        static void ObjToBin(string FilePath, string FilePathTo)
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
                sr.ReadLine();
                //Console.WriteLine();
                
                counter = sr.Read();
            }

            
            int Vcounter = 0;
            int Fcounter = 0;
            string wv = "";
            List<string> layer_buff = new List<string>();
            List<string> layer_buff_f = new List<string>();
            List<string> layer_buff_vt = new List<string>();
            string[] ar_layer_1 =   new string[]{};
            string[] ar_layer_2 =   new string[]{};
            string[] ar_layer_3 =   new string[]{};
            string[] ar_layer_4 =   new string[]{};
            string[] ar_layer_5 =   new string[]{};
            string[] ar_layer_6 =   new string[]{};
            string[] ar_layer_7 =   new string[]{};
            string[] ar_layer_8 =   new string[]{};
            string[] ar_layer_9 =   new string[]{};
            string[] ar_layer_10 =  new string[]{};
            string[] ar_layer_11 =  new string[]{};

            string[] ar_Tlayer_1 = new string[] { };
            string[] ar_Tlayer_2 = new string[] { };
            string[] ar_Tlayer_3 = new string[] { };
            string[] ar_Tlayer_4 = new string[] { };
            string[] ar_Tlayer_5 = new string[] { };
            string[] ar_Tlayer_6 = new string[] { };
            string[] ar_Tlayer_7 = new string[] { };
            string[] ar_Tlayer_8 = new string[] { };
            string[] ar_Tlayer_9 = new string[] { };
            string[] ar_Tlayer_10 = new string[] { };

            string[] ar_layer_1_f = new string[] { };
            string[] ar_layer_2_f = new string[] { };
            string[] ar_layer_3_f = new string[] { };
            string[] ar_layer_4_f = new string[] { };
            string[] ar_layer_5_f = new string[] { };
            string[] ar_layer_6_f = new string[] { };
            string[] ar_layer_7_f = new string[] { };
            string[] ar_layer_8_f = new string[] { };
            string[] ar_layer_9_f = new string[] { };
            string[] ar_layer_10_f = new string[] { };
            string[] ar_layer_11_f = new string[] { };

            string[] ar_Tlayer_1_vt = new string[] { };
            string[] ar_Tlayer_2_vt = new string[] { };
            string[] ar_Tlayer_3_vt = new string[] { };
            string[] ar_Tlayer_4_vt = new string[] { };
            string[] ar_Tlayer_5_vt = new string[] { };
            string[] ar_Tlayer_6_vt = new string[] { };
            string[] ar_Tlayer_7_vt = new string[] { };
            string[] ar_Tlayer_8_vt = new string[] { };
            string[] ar_Tlayer_9_vt = new string[] { };
            string[] ar_Tlayer_10_vt = new string[] { };


            string o_group_name = "";

            int vertex_layer_count_1 = 0;
            int vertex_layer_count_2 = 0;
            int vertex_layer_count_3 = 0;
            int vertex_layer_count_4 = 0;
            int vertex_layer_count_5 = 0;
            int vertex_layer_count_6 = 0;
            int vertex_layer_count_7 = 0;
            int vertex_layer_count_8 = 0;
            int vertex_layer_count_9 = 0;
            int vertex_layer_count_10 = 0;
            int vertex_layer_count_11 = 0;

            int face_layer_count_1 = 0;
            int face_layer_count_2 = 0;
            int face_layer_count_3 = 0;
            int face_layer_count_4 = 0;
            int face_layer_count_5 = 0;
            int face_layer_count_6 = 0;
            int face_layer_count_7 = 0;
            int face_layer_count_8 = 0;
            int face_layer_count_9 = 0;
            int face_layer_count_10 = 0;
            int face_layer_count_11 = 0;

            int f_layer_sub_1 = 0;
            int f_layer_sub_2 = 0;
            int f_layer_sub_3 = 0;
            int f_layer_sub_4 = 0;
            int f_layer_sub_5 = 0;
            int f_layer_sub_6 = 0;
            int f_layer_sub_7 = 0;
            int f_layer_sub_8 = 0;
            int f_layer_sub_9 = 0;
            int f_layer_sub_10 = 0;
            int f_layer_sub_11 = 0;

            int T_vertex_layer_count_1 = 0;
            int T_vertex_layer_count_2 = 0;
            int T_vertex_layer_count_3 = 0;
            int T_vertex_layer_count_4 = 0;
            int T_vertex_layer_count_5 = 0;
            int T_vertex_layer_count_6 = 0;
            int T_vertex_layer_count_7 = 0;
            int T_vertex_layer_count_8 = 0;
            int T_vertex_layer_count_9 = 0;
            int T_vertex_layer_count_10 = 0;

            var vertex_sequence_counter = 0;
            var vt = 0;
            for (var i = 0; i < 21; i++)
            {
                layer_buff.Clear();
                layer_buff_f.Clear();
                layer_buff_vt.Clear();
                bool leave = false;
                
                if (counter == 111) //o
                {
                    o_group_name = sr.ReadLine();
                    layer_buff.Add(o_group_name);

                    vertex_sequence_counter += Vcounter;
                    Vcounter = 0;

                    counter = sr.Read();
                    do //v
                    {
                        counter = sr.Read();
                        wv = sr.ReadLine();

                        //Console.WriteLine(wv);
                        
                        layer_buff.Add(wv);

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

                }

                if (counter == 118 && leave)//vt
                {
                    
                    do
                    {
                        sr.Read();
                        string uv = sr.ReadLine();
                        layer_buff_vt.Add(uv);
                        counter = sr.Read();
                    }
                    while (counter == 118 && sr.Peek() == 116);
                }

                if (sr.Read() == 115 && sr.Read() == 101 && sr.Read() == 109 && sr.Read() == 116 && sr.Read() == 108)
                {
                    sr.ReadLine();
                    counter = sr.Read();
                }

                if (counter == 115) //s
                {
                    sr.ReadLine();
                    counter = sr.Read();
                }
                

                if (counter == 102) //f
                {
                    Fcounter = 0;




                    do //f
                    {
                        
                        string wf = sr.ReadLine();
                        layer_buff_f.Add(wf);
                        counter = sr.Read();
                        sr.Read();
                        Fcounter++;
                    }
                    while (counter == 102);
                }



                string what_layer = " ";
                what_layer = "";
                what_layer = layer_buff.FirstOrDefault();
                try
                {
                    what_layer = string.Join("", what_layer.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                }
                catch
                {
                    //Console.WriteLine("err");
                }

                if (what_layer == "layer_1")
                {
                    ar_layer_1 = layer_buff.ToArray();
                    ar_layer_1_f = layer_buff_f.ToArray();
                    vertex_layer_count_1 = Vcounter;
                    f_layer_sub_1 = vertex_sequence_counter;
                    face_layer_count_1 = Fcounter * 3;
                }
                if (what_layer == "layer_2")
                {
                    ar_layer_2 = layer_buff.ToArray();
                    ar_layer_2_f = layer_buff_f.ToArray();
                    vertex_layer_count_2 = Vcounter;
                    f_layer_sub_2 = vertex_sequence_counter;
                    face_layer_count_2 = Fcounter * 3;
                }
                if (what_layer == "layer_3")
                {
                    ar_layer_3 = layer_buff.ToArray();
                    ar_layer_3_f = layer_buff_f.ToArray();
                    vertex_layer_count_3 = Vcounter;
                    f_layer_sub_3 = vertex_sequence_counter;
                    face_layer_count_3 = Fcounter * 3;
                }
                if (what_layer == "layer_4")
                {
                    ar_layer_4 = layer_buff.ToArray();
                    ar_layer_4_f = layer_buff_f.ToArray();
                    vertex_layer_count_4 = Vcounter;
                    f_layer_sub_4 = vertex_sequence_counter;
                    face_layer_count_4 = Fcounter * 3;
                }
                if (what_layer == "layer_5")
                {
                    ar_layer_5 = layer_buff.ToArray();
                    ar_layer_5_f = layer_buff_f.ToArray();
                    vertex_layer_count_5 = Vcounter;
                    f_layer_sub_5 = vertex_sequence_counter;
                    face_layer_count_5 = Fcounter * 3;
                }
                if (what_layer == "layer_6")
                {
                    ar_layer_6 = layer_buff.ToArray();
                    ar_layer_6_f = layer_buff_f.ToArray();
                    vertex_layer_count_6 = Vcounter;
                    f_layer_sub_6 = vertex_sequence_counter;
                    face_layer_count_6 = Fcounter * 3;
                }
                if (what_layer == "layer_7")
                {
                    ar_layer_7 = layer_buff.ToArray();
                    ar_layer_7_f = layer_buff_f.ToArray();
                    vertex_layer_count_7 = Vcounter;
                    f_layer_sub_7 = vertex_sequence_counter;
                    face_layer_count_7 = Fcounter * 3;
                }
                if (what_layer == "layer_8")
                {
                    ar_layer_8 = layer_buff.ToArray();
                    ar_layer_8_f = layer_buff_f.ToArray();
                    vertex_layer_count_8 = Vcounter;
                    f_layer_sub_8 = vertex_sequence_counter;
                    face_layer_count_8 = Fcounter * 3;
                }
                if (what_layer == "layer_9")
                {
                    ar_layer_9 = layer_buff.ToArray();
                    ar_layer_9_f = layer_buff_f.ToArray();
                    vertex_layer_count_9 = Vcounter;
                    f_layer_sub_9 = vertex_sequence_counter;
                    face_layer_count_9 = Fcounter * 3;
                }
                if (what_layer == "layer_10")
                {
                    ar_layer_10 = layer_buff.ToArray();
                    ar_layer_10_f = layer_buff_f.ToArray();
                    vertex_layer_count_10 = Vcounter;
                    f_layer_sub_10 = vertex_sequence_counter;
                    face_layer_count_10 = Fcounter * 3;
                }
                if (what_layer == "layer_11")
                {
                    ar_layer_11 = layer_buff.ToArray();
                    ar_layer_11_f = layer_buff_f.ToArray();
                    vertex_layer_count_11 = Vcounter;
                    f_layer_sub_11 = vertex_sequence_counter;
                    face_layer_count_11 = Fcounter * 3;
                }



                if (what_layer == "Tlayer_1")
                {
                    ar_Tlayer_1 = layer_buff.ToArray();
                    ar_Tlayer_1_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_1 = Vcounter;
                }
                if (what_layer == "Tlayer_2")
                {
                    ar_Tlayer_2 = layer_buff.ToArray();
                    ar_Tlayer_2_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_2 = Vcounter;
                }
                if (what_layer == "Tlayer_3")
                {
                    ar_Tlayer_3 = layer_buff.ToArray();
                    ar_Tlayer_3_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_3 = Vcounter;
                }
                if (what_layer == "Tlayer_4")
                {
                    ar_Tlayer_4 = layer_buff.ToArray();
                    ar_Tlayer_4_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_4 = Vcounter;
                }
                if (what_layer == "Tlayer_5")
                {
                    ar_Tlayer_5 = layer_buff.ToArray();
                    ar_Tlayer_5_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_5 = Vcounter;
                }
                if (what_layer == "Tlayer_6")
                {
                    ar_Tlayer_6 = layer_buff.ToArray();
                    ar_Tlayer_6_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_6 = Vcounter;
                }
                if (what_layer == "Tlayer_7")
                {
                    ar_Tlayer_7 = layer_buff.ToArray();
                    ar_Tlayer_7_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_7 = Vcounter;
                }
                if (what_layer == "Tlayer_8")
                {
                    ar_Tlayer_8 = layer_buff.ToArray();
                    ar_Tlayer_8_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_8 = Vcounter;
                }
                if (what_layer == "Tlayer_9")
                {
                    ar_Tlayer_9 = layer_buff.ToArray();
                    ar_Tlayer_9_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_9 = Vcounter;
                }
                if (what_layer == "Tlayer_10")
                {
                    ar_Tlayer_10 = layer_buff.ToArray();
                    ar_Tlayer_10_vt = layer_buff_vt.ToArray();
                    T_vertex_layer_count_10 = Vcounter;
                }

            }


            bw.Write(Convert.ToInt16(vertex_layer_count_1));
            foreach (var k in ar_layer_1.Skip(1))
            {
                //Console.WriteLine(k);
                var xyz = k.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                bw.Write(float.Parse(xyz[0]));
                bw.Write(float.Parse(xyz[1]));
                bw.Write(float.Parse(xyz[2]));

            }
            bw.Write(Convert.ToInt16(face_layer_count_1));
            foreach (var k in ar_layer_1_f)
            {
                //Console.WriteLine(k);
                var xyz = k.Split("".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        xyz[i] = xyz[i].Substring(0, xyz[i].IndexOf("/"));
                    }
                }
                catch { }
                int n1 = int.Parse(xyz[0]) - 1;
                int n2 = int.Parse(xyz[1]) - 1;
                int n3 = int.Parse(xyz[2]) - 1;
                bw.Write(Convert.ToInt16(n1));
                bw.Write(Convert.ToInt16(n2));
                bw.Write(Convert.ToInt16(n3));
                //Console.WriteLine(n1 + " " + n2 + " " + n3);
            }

                bw.Write(Convert.ToInt16(vertex_layer_count_2));
                foreach (var k in ar_layer_2.Skip(1))
                {
                    WriteV(k, bw);
                }
                bw.Write(Convert.ToInt16(face_layer_count_2));
                foreach (var k in ar_layer_2_f)
                {
                    WriteF(k, bw, f_layer_sub_2);
                }

                    bw.Write(Convert.ToInt16(vertex_layer_count_3));
                    foreach (var k in ar_layer_3.Skip(1))
                    {
                        WriteV(k, bw);
                    }
                    bw.Write(Convert.ToInt16(face_layer_count_3));
                    foreach (var k in ar_layer_3_f)
                    {
                        WriteF(k, bw, f_layer_sub_3);
                    }

                        bw.Write(Convert.ToInt16(vertex_layer_count_4));
                        foreach (var k in ar_layer_4.Skip(1))
                        {
                            WriteV(k, bw);
                        }
                        bw.Write(Convert.ToInt16(face_layer_count_4));
                        foreach (var k in ar_layer_4_f)
                        {
                            WriteF(k, bw, f_layer_sub_4);
                        }

            bw.Write(Convert.ToInt16(vertex_layer_count_5));
            foreach (var k in ar_layer_5.Skip(1))
            {
                WriteV(k, bw);
            }
            bw.Write(Convert.ToInt16(face_layer_count_5));
            foreach (var k in ar_layer_5_f)
            {
                WriteF(k, bw, f_layer_sub_5);
            }

                bw.Write(Convert.ToInt16(vertex_layer_count_6));
                foreach (var k in ar_layer_6.Skip(1))
                {
                    WriteV(k, bw);
                }
                bw.Write(Convert.ToInt16(face_layer_count_6));
                foreach (var k in ar_layer_6_f)
                {
                    WriteF(k, bw, f_layer_sub_6);
                }

                    bw.Write(Convert.ToInt16(vertex_layer_count_7));
                    foreach (var k in ar_layer_7.Skip(1))
                    {
                        WriteV(k, bw);
                    }
                    bw.Write(Convert.ToInt16(face_layer_count_7));
                    foreach (var k in ar_layer_7_f)
                    {
                        WriteF(k, bw, f_layer_sub_7);
                    }

                        bw.Write(Convert.ToInt16(vertex_layer_count_8));
                        foreach (var k in ar_layer_8.Skip(1))
                        {
                            WriteV(k, bw);
                        }
                        bw.Write(Convert.ToInt16(face_layer_count_8));
                        foreach (var k in ar_layer_8_f)
                        {
                            WriteF(k, bw, f_layer_sub_8);
                        }

            bw.Write(Convert.ToInt16(vertex_layer_count_9));
            foreach (var k in ar_layer_9.Skip(1))
            {
                WriteV(k, bw);
            }
            bw.Write(Convert.ToInt16(face_layer_count_9));
            foreach (var k in ar_layer_9_f)
            {
                WriteF(k, bw, f_layer_sub_9);
            }

                bw.Write(Convert.ToInt16(vertex_layer_count_10));
                foreach (var k in ar_layer_10.Skip(1))
                {
                    WriteV(k, bw);
                }
                bw.Write(Convert.ToInt16(face_layer_count_10));
                foreach (var k in ar_layer_10_f)
                {
                    WriteF(k, bw, f_layer_sub_10);
                }

                    bw.Write(Convert.ToInt16(vertex_layer_count_11));
                    foreach (var k in ar_layer_11.Skip(1))
                    {
                        WriteV(k, bw);
                    }
                    bw.Write(Convert.ToInt16(face_layer_count_11));
                    foreach (var k in ar_layer_11_f)
                    {
                        WriteF(k, bw, f_layer_sub_11);
                    }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_1));
            foreach (var k in ar_Tlayer_1.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_1_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_2));
            foreach (var k in ar_Tlayer_2.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_2_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_3));
            foreach (var k in ar_Tlayer_3.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_3_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_4));
            foreach (var k in ar_Tlayer_4.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_4_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_5));
            foreach (var k in ar_Tlayer_5.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_5_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_6));
            foreach (var k in ar_Tlayer_6.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_6_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_7));
            foreach (var k in ar_Tlayer_7.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_7_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_8));
            foreach (var k in ar_Tlayer_8.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_8_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_9));
            foreach (var k in ar_Tlayer_9.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_9_vt[vt]);
                vt++;
            }

            vt = 0;
            bw.Write(Convert.ToInt16(T_vertex_layer_count_10));
            foreach (var k in ar_Tlayer_10.Skip(1))
            {
                WriteV_VT(k, bw, ar_Tlayer_10_vt[vt]);
                vt++;
            }

            Vcounter = 0;

            sr.Close();
            fs.Close();
            fsw.Close();
            bw.Close();
            
        }

        static void BinToObj(string FilePath, string FilePathTo)
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
                    
                    //Console.WriteLine(layer);
                    
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
                


                for (int i = 0; i < max1; i++) //write vertices
                {

                    string floatout = "v " + br.ReadSingle() + " " + br.ReadSingle() + " " + br.ReadSingle() + " " + vr + " " + vg + " " + vb;

                    //Console.WriteLine(floatout);

                    sw.WriteLine(floatout);

                }


                Int16 max2 = br.ReadInt16(); //leanght of polygon faces list
                max2 /= 3;

                for (int i = 0; i < max2; i++) //write faces
                {
                    string intout = "f " + (br.ReadInt16() + 1 + counter) + " " + (br.ReadInt16() + 1 + counter) + " " + (br.ReadInt16() + 1 + counter);

                    //Console.WriteLine(intout);

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

                    //Console.WriteLine("textured layer_" + (iii + 1));
                    
                    sw.WriteLine("o Tlayer_" + (iii + 1));
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
                            x = 1f - UVcounter;
                            y = 1f - UVcounter;
                            z = 1f - UVcounter;
                            w = 1f - UVcounter;
                        }
                        string UVout = "f " + (x + UVcounter) + "/" + (vtlist_ar[(r - 2)] + max5) + " " + (y + UVcounter) + "/" + (vtlist_ar[(r - 1)] + max5) + " " + (z + UVcounter) + "/" + (vtlist_ar[(r - 0)] + max5) + " " + (w + UVcounter) + "/" + (vtlist_ar[(r + 1)] + max5);

                        //Console.WriteLine(UVout);
                        
                        sw.WriteLine(UVout);
                    }
                    rr += 4;
                }


        
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
                            BinToObj(Pfrom, Pto);
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
                        try
                        {
                            Pto = Path.ChangeExtension(Pfrom, ".bin");
                            ObjToBin(Pfrom, Pto);
                            Console.WriteLine("Done");
                        }
                        catch
                        {
                            Console.WriteLine("Error converting to .bin");
                        }
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
            

        }
    }
}
