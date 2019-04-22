using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_TEST
{
    class Program
    {
        public static int FindMatchedFilesFromSubDirectory(string path, string file_name, out string[] result)
        {
            // find all files which is matched given file_name
            result = Directory.GetFiles(path, file_name, SearchOption.AllDirectories);
            return result.Length;
        }

        static void Main(string[] args)
        {
            string folder = "./BIGFILE/";
            string file_name = Console.ReadLine();

            string[] result;
            if (FindMatchedFilesFromSubDirectory(folder, file_name, out result) > 0)
            {
                foreach (string path in result)
                {
                    string input_file_path = folder + file_name;
                    string folder_path = Path.GetDirectoryName(path);
                    string zip_file = folder_path + "\\CMP_" + file_name;
                    List<string> read = new List<string>();
                    if (CHelper.read_file_data(path, ref read))
                    {
                        CHelper.line_zip(zip_file, read);
                    }

                    List<string> zip_data = new List<string>();
                    if (CHelper.read_file_data(zip_file, ref zip_data))
                    {
                        CHelper.char_zip(zip_file, zip_data);
                    }

                    List<string> caesar_data = new List<string>();
                    if (CHelper.read_file_data(zip_file, ref caesar_data))
                    {
                        CHelper.caesar(zip_file, caesar_data);
                    }
                }
            }
            
        }
    }
}
