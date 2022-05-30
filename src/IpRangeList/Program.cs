using System;
using System.IO;
using System.Linq;
using System.Text;

namespace IpRangeList
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args != null)
                CheckArgs(args);
            else
                WriteHelp();
        }

        private static void CheckArgs(string[] args)
        {
            string source = null, export = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-source", StringComparison.CurrentCultureIgnoreCase))
                {
                    try { source = args[++i]; }
                    catch { Console.WriteLine("Exception: Path not found. (HELP: -source [path])"); WriteHelp(); return; }
                }
                if (args[i].Equals("-export", StringComparison.CurrentCultureIgnoreCase))
                {
                    try { export = args[++i]; }
                    catch { Console.WriteLine("Exception: Path not found. (HELP: -export [path])"); WriteHelp(); return; }
                }
                if (!(args[i].Equals("-source", StringComparison.CurrentCultureIgnoreCase) ||
                    args[i].Equals("-export", StringComparison.CurrentCultureIgnoreCase)) &&
                    args[i].StartsWith("-", StringComparison.CurrentCultureIgnoreCase))
                {
                    WriteHelp(); return;
                }
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
                    WriteHelp();
                else
                {
                    try
                    {
                        var result = GetIpRangePerLine(args[0]);

                        if (string.IsNullOrEmpty(export))
                            Console.WriteLine(result);
                        else
                        {
                            try
                            {
                                if (!Directory.Exists(export))
                                {
                                    Directory.CreateDirectory(export);
                                }
                                File.WriteAllText(export, result);
                            }
                            catch
                            {
                                Console.WriteLine($"Exception: Invalid export path '{export}'.");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        WriteHelp();
                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(export))
                {
                    Console.WriteLine($"Exception: export path required '{export}'.");
                    WriteHelp();
                    return;
                }
                if (File.Exists(source))
                {
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        var ips = File.ReadAllLines(source);
                        foreach (string ip in ips)
                        {
                            try
                            {
                                sb.AppendLine(GetIpRangePerLine(ip));
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }
                        File.WriteAllText(export, sb.ToString());
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Exception: Check file contents '{source}'.");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            }
        }

        private static string GetIpRangePerLine(string value)
        {
            var ir = new IPRange(value);
            return string.Join("\r\n", ir.GetAllIP().Select(a => a.ToString()));
        }

        private static void WriteHelp() {
            Console.WriteLine("");
            Console.WriteLine("Welcome to ip range list.");
            Console.WriteLine("");
            Console.WriteLine("For generate ip list:");
            Console.WriteLine("\tIpRangeList.exe [ip range]");
            Console.WriteLine("");
            Console.WriteLine("\tExample of ip ranges:");
            Console.WriteLine("\t\t192.168.1.0/24");
            Console.WriteLine("\t\t192.168.1.1-128");
            Console.WriteLine("");
            Console.WriteLine("\t\tNote: For export to file,use: -export [path]");
            Console.WriteLine("");
            Console.WriteLine("For generate ip list from file");
            Console.WriteLine("\tIpRangeList.exe -source [path] -export [path]");
            Console.WriteLine("");
            Console.WriteLine("\t\tExample of file contents (each ip in new line):");
            Console.WriteLine("\t\t\t192.168.1.0/24");
            Console.WriteLine("\t\t\t192.168.1.1-128");
        }
    }
}
