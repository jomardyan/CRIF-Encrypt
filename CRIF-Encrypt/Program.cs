﻿using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CRIF_Encrypt
{


    internal class Program
    {

        private static void Main(string[] args)
        {
            create(); 

            StartingPoint();
            if (args.Length == 0)
                return; // exit if no file was dragged onto program
            string text = File.ReadAllText(args[0]);
            if (Path.GetExtension(args[0]) is not ".dat" & Path.GetExtension(args[0]) is not ".DAT")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Imput is not .dat file. ::  Your file -> {0}", Path.GetExtension(args[0]));
                Console.ReadLine();
                return; // exit if  the file is not dat. 
            }






            text = text.Replace("~", "~\r\n");

            //FOR ZIP PURPOSE
            string dir = Path.GetDirectoryName(args[0])
               + Path.DirectorySeparatorChar;
            string filename = Path.GetFileNameWithoutExtension(args[0]);
            //FOR ZIP PURPOSE END 

            string path = Path.GetDirectoryName(args[0])
               + Path.DirectorySeparatorChar
               + Path.GetFileNameWithoutExtension(args[0])
               + Path.GetExtension(args[0]);
            File.WriteAllText(path, text);

            //DEBUG PURPOSE
            //Console.WriteLine(" \n " + "--Selected file: " + path + "\n");
            //Console.WriteLine(" \n " + "--Selected dir: " + dir + "\n");
            //Console.WriteLine("Command: " + SignAndEncrypt(dir, filename) + "\n");

            //simplified:: System.Diagnostics.Process.Start("CMD.exe", EncryptCommand(path));



            //It's time so save the file into fileserver 

            if (Zipper(dir, filename))
            {
                Thread.Sleep(8000);
                try
                {
                    Console.WriteLine("Sign end encrypt...");
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = SignAndEncrypt(dir, filename);
                    process.StartInfo = startInfo;
                    process.Start();


                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("Unable to zip");

            }


            Console.WriteLine("Operation completed... ");
            var x = Console.ReadLine();
        }

        private static void StartingPoint()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("(c) Hayk Jomardyan 2021. All rights reserved.\n");
            Console.ResetColor();
            Console.WriteLine("Please wait... \n");
            Thread.Sleep(500);
        }

        static string SignAndEncrypt(string ImputDir, string FileName)
        {
            //Long codding in order to be readable.
            StringBuilder st = new StringBuilder();
            string command;
            string b = "\"";
            command = "/C gpg.exe -v -se  -r CRIF-SWO-PROD  --passphrase \"\"";
            st.Append(command);
            st.Append(" ");
            st.Append(b + ImputDir + FileName + ".zip" + b);
            return st.ToString();
        }

        static bool Zipper(string ImputDir, string FileName)
        {
            //Long codding in order to be readable. 
            StringBuilder cm = new StringBuilder();
            cm.Append("a ");
            cm.Append("\"" + ImputDir + FileName + ".zip" + "\" ");
            cm.Append("\"" + ImputDir + FileName + ".dat" + "\"");
            Console.WriteLine("----------");
            //DEBUG PURPOSE
            //Console.WriteLine("7Z Command:  " + cm.ToString());
            Console.WriteLine("----------");
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "C:\\Program Files\\7-Zip\\7z.exe";
                startInfo.Arguments = cm.ToString();
                process.StartInfo = startInfo;
                process.Start();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }


        }

        static void create(string FileName, String Directory)
        {
            string text = File.ReadAllText(FileName);
            text = text.Replace("	", "~^");
            text = text.ToUTF8();
            File.WriteAllText("J:\\BI Analysis\\CRIF REPORTS\\CL_SWO2_ICREDIT_20210717_0630.dat", text);
        }

    }


    public static class StringExtensions
    {
        public static string ToUTF8(this string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }
    }


}