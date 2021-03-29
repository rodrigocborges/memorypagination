using System;
using System.Collections.Generic;

namespace  memorypagination
{
	public class MemoryProcess {
		public int ID { get; set; }
		public List<int> Frames { get; set; }
		public List<int> Pages { get; set; } = new List<int>();
		public int AmountOfPages { get; set; }
		public int AmountOfFrames { get; set; }

		//Cria o processo com base na linha do arquivo de entrada que a classe FileManipulation vai enviar
		public MemoryProcess(int amountOfFrames, int amountOfPages, string pages){
			ID = new Random().Next(1, 99999);
			Frames = new List<int>(amountOfFrames);
			AmountOfFrames = amountOfFrames;
			AmountOfPages = amountOfPages;
			string[] arrayPages = pages.Split(' ');
			foreach(string p in arrayPages)
				Pages.Add(Convert.ToInt32(p));
		}

		//Exibe na tela algumas informações do processo
		public void Print(){
			Console.WriteLine(string.Format("--> Quantidade de molduras (frames): \t{0}\n--> Acesso as páginas (pages): \t{1}\n--> Quantidade de páginas (pages): \t{2}", AmountOfFrames, Pages.Count, AmountOfPages));
		}
	}
}