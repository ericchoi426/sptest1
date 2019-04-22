using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SP_TEST
{
    class Program
    {
        static Socket listener;
        public class Worker
        {
            public int FindMatchedFilesFromSubDirectory(string path, string file_name, out string[] result)
            {
                // find all files which is matched given file_name
                result = Directory.GetFiles(path, file_name, SearchOption.AllDirectories);
                return result.Length;
            }

            public bool enc_data(string file_name, ref List<string> results, string key_val,int count = 0)
            {
                string folder = "./BIGFILE/";

                string[] result;
                if (FindMatchedFilesFromSubDirectory(folder, file_name, out result) > 0)
                {
                    string path = result[0];
                    string input_file_path = folder + file_name;
                    string folder_path = Path.GetDirectoryName(path);
                    string zip_file = folder_path + "\\CMP_" + file_name;
                    List<string> read = new List<string>();
                    if (CHelper.read_file_data(path, ref read))
                    {
                        if (count > 0)
                        {
                            read.RemoveRange(0, count);
                        }
                        CHelper.line_zip(zip_file, read);
                    }

                    List<string> zip_data = new List<string>();
                    if (CHelper.read_file_data(zip_file, ref zip_data))
                    {
                        CHelper.char_zip(zip_file, zip_data);
                    }

                    List<string> key_data = new List<string>();
                    if (CHelper.read_file_data(zip_file, ref key_data))
                    {
                        CHelper.key_enc(zip_file, key_val, key_data);
                    }
                    CHelper.read_file_data(zip_file, ref results);
                    return true;
                }
                return false;
            }
            // This method will be called when the thread is started. 
            public void DoWork()
            {
                const int DEFAULT_BUFLEN = 4096;
                const int DEFAULT_PORT = 9876;

                // Data buffer for incoming data.
                byte[] recvbuf = new byte[DEFAULT_BUFLEN];
                int recvLen;

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, DEFAULT_PORT);

                // Create a TCP/IP socket.
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and 
                // listen for incoming connections.
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    // Start listening for connections.
                    while (true)
                    {
                        // Program is suspended while waiting for an incoming connection.
                        Socket handler = listener.Accept();
                        if ((recvLen = handler.Receive(recvbuf)) > 0)
                        {
                            int index = 0;
                            var input_str = System.Text.Encoding.Default.GetString(recvbuf);
                            input_str = input_str.Trim('\0');
                            string[] tokens = input_str.Split('#');
                            string str = tokens[0];
                            string keyVal = tokens[1];
                            List<string> out_data = new List<string>();
                            if (enc_data(str, ref out_data,keyVal))
                            {
                                byte[] byData = System.Text.Encoding.ASCII.GetBytes(out_data[index]);
                                handler.Send(byData);
                            }
                            Array.Clear(recvbuf, 0, recvbuf.Length);
                            while ((recvLen = handler.Receive(recvbuf)) > 0)
                            {
                                var result_str = System.Text.Encoding.Default.GetString(recvbuf);
                                result_str = result_str.Trim('\0');
                                if ("ACK".Equals(result_str))
                                {
                                    index++;
                                    if (index >= out_data.Count) break;
                                    byte[] byData = System.Text.Encoding.ASCII.GetBytes(out_data[index]);
                                    handler.Send(byData);
                                }
                                else if ("ERR".Equals(result_str))
                                {
                                    byte[] byData = System.Text.Encoding.ASCII.GetBytes(out_data[index]);
                                    handler.Send(byData);
                                }
                                else
                                {
                                    int num = Convert.ToInt32(result_str) - 1;
                                    out_data.Clear();
                                    if (enc_data(str, ref out_data, keyVal,num))
                                    {
                                        index = 0;
                                        byte[] byData = System.Text.Encoding.ASCII.GetBytes(out_data[index]);
                                        handler.Send(byData);
                                    }
                                }
                                Array.Clear(recvbuf, 0, recvbuf.Length);
                            }
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                listener.Close();
            }
        }
        static void Main(string[] args)
        {
            Worker workerObject = new Worker();
            Thread workerThread = new Thread(workerObject.DoWork);
            workerThread.Start();

            string strLine;
            while (true)
            {
                strLine = Console.ReadLine();

                if (strLine.Equals("QUIT"))
                {
                    listener.Close();
                    break;
                }
            }

            workerThread.Join();
        }
    }
}
