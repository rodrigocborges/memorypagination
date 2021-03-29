using System;
using System.Collections.Generic;
using System.IO;

namespace memorypagination
{
    /*
        Classe para manipular o arquivo de entrada (processes)
    */
    public class FileManipulation
    {
		//Função que abre o arquivo, lê linha a linha e cria os processos conforme definido, retornando a lista de processos
        public static List<MemoryProcess> GetAndListProcesses(string filename){
			List<MemoryProcess> list = new List<MemoryProcess>();
			try {
				StreamReader reader = new StreamReader(filename);
				string[] allLines = reader.ReadToEnd().Split('\n');
				if(allLines != null){
					foreach(string line in allLines){
						//Número de molduras de página na memória|número de páginas do processo|sequência em que as páginas são acessadas
						string[] info = line.Split('|');
						MemoryProcess mp = new MemoryProcess(Convert.ToInt32(info[0]), Convert.ToInt32(info[1]), info[2]);
						list.Add(mp);
					}
				}
				reader.Close();
			}
			catch(IOException ex){
				Console.WriteLine(ex.Message);
			}
			return list;
		}
    }
}
