using System;
using System.Collections.Generic;

namespace memorypagination
{
    /*
        Desenvolvido por José Vitor Gomes Braga e Rodrigo Campos Borges

        Molduras: são as unidades correspondentes na memória física
    */
    public class Program
    {
        private static void PrintProcesses(List<MemoryProcess> processes){
            foreach(MemoryProcess p in processes){
                Console.WriteLine(string.Format("#Processo ({0}):", p.ID));
                p.Print();
            }
        }
        public static void Main(string[] args)
        {
            List<MemoryProcess> processes = FileManipulation.GetAndListProcesses(@"C:\dev\furg\memorypagination\processes.txt");
            PrintProcesses(processes);
            Console.ReadKey();
        }
    }
}
