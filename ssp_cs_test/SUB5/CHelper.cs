using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SP_TEST
{
    class CHelper
    {
        static public bool read_file_data(string read_file,ref List<string> data)
        {
            if (File.Exists(read_file))
            {
                // using문을 사용하면 Diposal를 자동 처리 즉 file close를 알아서 처리해줌
                using (StreamReader reader = new StreamReader(read_file))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                            return true;
                        data.Add(line);
                    }
                }
            }
            return false;

        }
        static public bool write_file_data(string write_file,string data)
        {
            bool result = true;

            using (var writer = new StreamWriter(write_file, true, Encoding.ASCII))
            {
                writer.NewLine = "\n";
                writer.WriteLine(data);
            }
            return result;
        }

        static public void line_zip(string zip_file,List<string> data)
        {
            if (File.Exists(zip_file)) File.Delete(zip_file);
            int current = 0;
            string pibot = "";
            int total = data.Count;
            do
            {
                pibot = data[current];
                int num = 1;

                while ((++current < total) && (pibot.Equals(data[current]))) num++;

                string zip_data = (num > 1) ? (num.ToString() + "#" + pibot) : pibot;
                write_file_data(zip_file, zip_data);
            } while (total > current);
        }

        static public string char_zip(string data)
        {
            string results = "";
            int len = data.Length;
            int current = 0;
            char pibot = ' ';
            do
            {
                pibot = data[current];
                int num = 1;
                while ((++current < len) && (pibot == data[current])) num++;
                results += (num > 2)? (num.ToString() + pibot.ToString()) : string.Concat(Enumerable.Repeat(pibot.ToString(), num));
            } while (len > current);
            return results;
        }
        static public void char_zip(string path,List<string> data)
        {
            if (File.Exists(path)) File.Delete(path);
            foreach (string line in data){
                string zip = char_zip(line);
                write_file_data(path, zip);
            }
        }

        static public string caesar(string data)
        {
            string results = "";
            string enc_table = "VWXYZABCDEFGHIJKLMNOPQRSTU";
            string org_table = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach(char a in data)
            {
                results += org_table.Contains(a) ? enc_table[a - 'A']: a;
            }
            return results;
        }
        static public void  caesar(string path, List<string> data)
        {
            if (File.Exists(path)) File.Delete(path);
            foreach (string line in data)
            {
                string enc_str = caesar(line);
                write_file_data(path, enc_str);
            }
        }
        static public string key_enc(string data, string key)
        {
            string results = "";
            string org_tbl = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string tmp_tbl = new string(org_tbl.Except(key).ToArray());
            string enc_tbl = key + tmp_tbl;
            foreach (char a in data)
            {
                results += org_tbl.Contains(a) ? enc_tbl[a - 'A'] : a;
            }
            return results;
        }

        static public void key_enc(string path, string key, List<string> data)
        {
            if (File.Exists(path)) File.Delete(path);
            foreach (string line in data)
            {
                string enc_str = key_enc(line,key);
                write_file_data(path, enc_str);
            }
        }
    }
}
