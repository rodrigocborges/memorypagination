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
        public static void Main(string[] args)
        {
            List<MemoryProcess> processes = FileManipulation.GetAndListProcesses(@"C:\dev\furg\memorypagination\processes.txt");
            
            Console.ReadKey();
        }
    }
}
