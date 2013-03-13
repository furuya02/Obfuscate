using System;

namespace Obfuscate {
    class Program {
        static void Main(string[] args){


            if (args.Length == 0){
                Console.WriteLine("use: Obfuscate.exe P@ss$$w0rD");
                return;
            }
            const int max = 128; //扱えるリテラル文字列の最大長
            const int magic = 1112455; //31が3個連続するポジション
            const int seed = 200;

            var pos = new Int32[max];
            for (var e = 0; e < max/3; e++){
                pos[e] = Encode(seed, args[0], e*3);
                if (pos[e] == magic) {
                    break;
                }
            }

            Console.Write("static string  Literal(){\n");
            Console.Write("    var pos = new[]{");
            for (int e = 0; e < max / 3 && pos[e] != magic; e++) {
                if(e!=0)
                    Console.Write(",");
                Console.Write(string.Format("{0}", pos[e]));

            }
            Console.Write("};\n");
            Console.Write("    var sb = new StringBuilder();\n");
            Console.Write("    foreach (var res in pos.Select(Decode)){\n");
            Console.Write("    for (var i = 0; i < 3; i++){\n");
            Console.Write("            var c = (char) (res[i] + 32);\n");
            Console.Write("            if (c == 31 + 32)\n");
            Console.Write("                break;\n");
            Console.Write("            sb.Append(c);\n");
            Console.Write("        }\n");
            Console.Write("    }\n");
            Console.Write("    return sb.ToString();\n");
            Console.Write("}\n");
            Console.Write("\n");
            Console.Write("static int[] Decode(int pos){\n");
            Console.Write("    var ran = new Random("+seed+");\n");
            Console.Write("    for (var i = 0; i < pos; i++)\n");
            Console.Write("        ran.Next(94);\n");
            Console.Write("    var res = new int[3];\n");
            Console.Write("    for (var e = 0; e < 3; e++)\n");
            Console.Write("        res[e] = ran.Next(94);\n");
            Console.Write("    return res;\n");
            Console.Write("}\n");

            Console.ReadKey();

        }

        static int Encode(int seed, String str ,int offset){
            
            const int len = 3; //３文字分を処理する
            
            var c = new Int32[len];
            for (var e = 0; e < len; e++){
                if (str.Length <= offset + e){
                    c[e] = 31;
                } else{
                    c[e] = str[offset + e] - 32;
                }
            }
            var ran = new Random(seed);
            for (var i = 0; ; i++) {
                for (var e = 0; e < len; e++) {
                    if (c[e] != ran.Next(94))
                        break;
                    i++;
                    if (e == len-1)
                        return i - len;
                }
            }
        }
    }
}
