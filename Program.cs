using System;
using System.Runtime.CompilerServices;
using System.Transactions;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CommandlineCSHARP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Mycl";

            string intro = "github repo https://github.com/Aihakaiha/clcs \nAuthor: Elias Sivonen\n\ntype 'help' or ? to get help\n";
            string prompt = "<cl> ";

            var cmds = new Commands();

            string cd = @"";
            cd = Directory.GetCurrentDirectory() + @"\";


            string[,] help_list = new string[,]{ 
                {"help", "show this message" }, 
                {"cd", "change/display current directory" }, 
                {"cls", "clears screen" }, 
                {"exit", "exit program" },
                {"cls", "clears console" },
                {"cdir", "creates directory in cd" },
                {"rdir", "removes directory in cd" },
                {"cfil", "creates file in cd" },
                {"rfil", "removes file in cd" },
                {"ls", "lists files in cd" },
                {"read", "reads file and displays content" }
            };

            Console.Write(intro);
            Console.WriteLine();

            while (true)
            {
                string cmd = "";
                string newinp = "";
                Console.Write(prompt);
                string inp = Console.ReadLine();
                //Console.WriteLine(inp);

                bool checker = false;
                if (inp != "")
                {
                    newinp = inp.Substring(inp.IndexOf(' ') + 1);
                    try { cmd = inp.Substring(0, inp.IndexOf(' ')); } catch { checker = true; cmd = inp.Substring(0, inp.Length); }
                }
                else
                {
                    continue;
                }




                switch (cmd.ToLower())
                {
                    case "echo":
                        cmds.Echo(newinp);
                        break;

                    case "cd":
                        if (newinp.Length == 0 | checker == true)
                        {
                            Console.WriteLine(cd);
                            break;
                        }
                        else
                        {
                            if (Directory.Exists(newinp))
                            {
                                cd = cmds.Cd(newinp);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid path {0}", newinp);
                                break;
                            }
                        }

                    case "exit":
                        cmds.Exit();
                        break;

                    case "help":
                        cmds.Help(help_list);
                        break;

                    case "?":
                        cmds.Help(help_list);
                        break;

                    case "cls":
                        cmds.Clear();
                        break;

                    case "cdir":
                        if (newinp.Length == 0 | checker == true)
                        {
                            Console.WriteLine("cdir usage\t\t\t{0, 10}", "cdir [foldername]");
                            break;
                        }
                        else
                        {
                            cmds.Create_directory(cd + newinp);
                            break;
                        }

                    case "rdir":
                        if (newinp.Length == 0 | checker == true)
                        {
                            Console.WriteLine("rdir usage\t\t\t{0, 10}", "rdir [foldername]");
                            break;
                        }
                        else
                        {
                            cmds.Remove_directory(cd + newinp);
                            break;
                        }

                    case "cfil":
                        if (newinp.Length == 0 | checker == true)
                        {
                            Console.WriteLine("cfil usage\t\t\t{0, 10}", "cfil [filename]");
                            break;
                        }
                        else
                        {
                            cmds.Create_file(cd + newinp);
                            break;
                        }

                    case "rfil":
                        if (newinp.Length == 0 | checker == true)
                        {
                            Console.WriteLine("rfil usage\t\t\t{0, 10}", "rfil [filename]");
                            break;
                        }
                        else
                        {
                            cmds.Remove_file(cd + newinp);
                            break;
                        }

                    case "ls":
                        cmds.List_files(cd);
                        break;

                    case "read":
                        if (newinp.Length == 0 | checker == true)
                        {
                            Console.WriteLine("read usage\t\t\t{0, 10}", "read [filename]");
                            break;
                        }
                        else
                        {
                            string output = "";
                            try { output = newinp.Substring(newinp.IndexOf(' ') + 1); } catch { output = ""; }
                            Console.WriteLine(output);
                            cmds.Read(cd, newinp, output);
                            break;
                        }

                    default:
                        Console.WriteLine(cmd + " is not a recognized command");
                        break;
                }
            }
        }
    }
    class Commands
    {
        public void Echo(string text)
        {
            Console.WriteLine(text);
        }
        public string Cd(string path)
        {
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }
            return path;
        }
        public void Exit()
        {
            Environment.Exit(0);
        }
        public void Clear()
        {
            Console.Clear();
        }
        public void Help(string[,] help)
        {
            int len = help.Length / 2;
            int c = 0;
            Console.WriteLine("{0,0}\t\t\t{1,5}", "command", "description");
            string sepe = new string('-', 50);
            Console.WriteLine(sepe);
            while (c != len)
            {
                Console.WriteLine("{0,0}\t\t\t{1,5}", help[c, 0], help[c, 1]);
                c++;
            }
            Console.WriteLine(sepe);
        }
        public void Create_directory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Created directory {0}", path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error {0}", e);
                }
            }
            
        }
        public void Remove_directory(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Console.Write("Are you sure you want to delete {0}? (Y/N) ", path);
                    string question = Console.ReadLine();
                    if (question.ToLower() == "y")
                    {
                        Directory.Delete(path, recursive: true);
                        Console.WriteLine("Deleted directory {0}", path);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error {0}", e);
                }
            }
        }
        public void Create_file(string path) 
        { 
            if (!File.Exists(path))
            {
                try
                {
                    var file = File.Create(path);
                    file.Close();
                    Console.WriteLine("Created file {0}", path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error {0}", e);
                }
            }
        }
        public void Remove_file(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    Console.WriteLine("Deleted file {0}", path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error {0}", e);
                }
            }
        }
        public void List_files(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    foreach (string dirpath in Directory.GetDirectories(path))
                    {
                        Console.WriteLine("{0,0}\t\t\t{1,15}",dirpath.Remove(0, path.Length), "<dir>");
                    }
                    foreach (string filepath in Directory.GetFiles(path))
                    {
                        FileInfo filepathinf = new FileInfo(filepath);
                        Console.WriteLine("{0,0}\t\t\t{1,15}mb", filepath.Remove(0, path.Length),((filepathinf.Length / 1024f) / 1024f).ToString("0.00"));
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error {0}", e);
                }
                
            }
        }
        public void Read(string tpath, string filename, string output)
        {
            string path = tpath + filename;
            Console.WriteLine(output);
            if (File.Exists(path))
            {
                try
                {
                    string file = File.ReadAllText(path);
                    StringBuilder sb = new StringBuilder();
                    if (output == "")
                    {
                        sb.Append(file);
                        Console.WriteLine(sb.ToString());
                    }
                    else
                    {
                        File.WriteAllText(tpath + output, file);
                        Console.WriteLine("Output file created {0}", tpath+output);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error {0}", e);
                }
            }
        }
    }
}
