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
            string input = folder + args[0];
            string zip_file = folder + "CMP_" + args[0];
            List<string> read = new List<string>();
            if(CHelper.read_file_data(input, ref read))
            {
                CHelper.line_zip(zip_file, read);
            }

            
            string char_zip_file = folder + "ZIP_" + args[0];
            List<string> zip_data = new List<string>();
            if (CHelper.read_file_data(zip_file, ref zip_data))
            {
                CHelper.char_zip(char_zip_file, zip_data);
            }

            string char_caesar_file = folder + "CAESAR_" + args[0];
            List<string> caesar_data = new List<string>();
            if (CHelper.read_file_data(char_zip_file, ref caesar_data))
            {
                CHelper.caesar(char_caesar_file, caesar_data);
            }

            string char_key_file = folder + "KEY_" + args[0];
            List<string> key_data = new List<string>();
            if (CHelper.read_file_data(char_zip_file, ref key_data))
            {
                CHelper.key_enc(char_key_file, "LGCNS",caesar_data);
            }
        }
    }
}
