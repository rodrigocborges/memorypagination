using System;
using System.Collections.Generic;

namespace  memorypagination
{
	public class MemoryProcess {
		public List<int> Frames { get; set; }
		public List<int> Pages { get; set; }
		public int AmountOfPages { get; set; }

		//Cria o processo com base na linha do arquivo de entrada que a classe FileManipulation vai enviar
		public MemoryProcess(int amountOfFrames, int amountOfPages, string pages){
			Frames = new List<int>(amountOfFrames);
			AmountOfPages = amountOfPages;
			string[] arrayPages = pages.Split(' ');
			foreach(string p in arrayPages)
				Pages.Add(Convert.ToInt32(p));
		}

		//Exibe na tela algumas informações do processo
		public void Print(){
			Console.WriteLine(string.Format("Quantidade de molduras (frames): \t{0}\nAcesso as páginas (pages): \t{1}\nQuantidade de páginas (pages): \t{2}", Frames.Count, Pages.Count, AmountOfPages));
		}
	}
}