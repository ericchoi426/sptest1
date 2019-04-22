using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = "./BIGFILE/";
            string file_name = Console.ReadLine();
            string input_file_path = folder + file_name;
            string zip_file = folder + "CMP_" + file_name;
            List<string> read = new List<string>();
            if(CHelper.read_file_data(input_file_path, ref read))
            {
                CHelper.line_zip(zip_file, read);
            }
        }
    }
}
