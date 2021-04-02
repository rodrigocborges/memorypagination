using System;
using System.Collections.Generic;
using System.Linq;

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

        //Algoritmos
        private static int FIFO(MemoryProcess process){
            int auxIn = 0, changesAmount = 0, index = 0;
            for(int i = 0; i < process.Pages.Count; ++i){
                int currentPage = process.Pages[i];

                //Se não conter a página atual na moldura, adiciona e incrementa indice
                if(process.Frames.Count < process.AmountOfFrames && !process.Frames.Contains(currentPage)){
                    process.Frames.Insert(index, currentPage);
                    ++index;
                    ++changesAmount;
                }else if(process.Frames.Contains(currentPage)){ //Se conter a página na moldura atual, apenas continua
                    continue;
                }else{
                    //Aplicando a lógica do primeiro que entrar, sair 
                    process.Frames.RemoveAt(auxIn);
                    //Adiciona página atual na posição antiga do primeiro que entrou para ser o novo parametro
                    process.Frames.Insert(auxIn, currentPage);
                    ++auxIn;
                    index = 0;
                    ++changesAmount;
                }

                //Caso o indice do elemento para remover seja a capacidade da lista, volta pro começo
                if(auxIn == process.AmountOfFrames)
                    auxIn = 0;
            }

            return changesAmount;
        }

        private static int MRU(MemoryProcess process){
            //Para molduras que ainda não tem preenchimento
            int index = 0, aux = 0;

            //Moldura cheia
            int high = 0;

            int changesAmount = 0;

            //Tempo ocioso para cada posição
            int[] pos = new int[process.AmountOfFrames];

            for(int i = 0; i < process.Pages.Count; ++i){
                int currentPage = process.Pages[i];

                //Página nova
                if(!process.Frames.Contains(currentPage) && (aux < process.AmountOfFrames)){
                    process.Frames.Insert(index, currentPage);
                    ++aux;
                    IncreaseTime(aux, pos);
                    pos[index] = 0;
                    ++index;
                    ++changesAmount;
                }else if(process.Frames.Contains(currentPage)){ //Página repetida 
                    IncreaseTime(aux, pos);
                    pos[process.Frames.IndexOf(currentPage)] = 0;
                }else{
                    high = GetIndexMRU(pos);
                    process.Frames.RemoveAt(high);
                    process.Frames.Insert(high, currentPage);
                    IncreaseTime(aux, pos);
                    pos[high] = 0;
                    ++index;
                    ++changesAmount;
                }               
            }
            return changesAmount;
        }

        private static int NFU(MemoryProcess process){
            int index = 0, changesAmount = 0, indexInFrame = 0, lessUsed = 0;
            int[] pagesAccess = new int[process.AmountOfPages];

            for(int i = 0; i < process.Pages.Count; ++i){
                int currentPage = process.Pages[i];
                //Página nova
                if(!process.Frames.Contains(currentPage) && (index < process.AmountOfFrames)){
                    process.Frames.Insert(index, currentPage);
                    ++pagesAccess[currentPage - 1];
                    ++index;
                    ++changesAmount;
                }else if(process.Frames.Contains(currentPage)){ //Página repetida
                    ++pagesAccess[currentPage - 1];
                }
                else{ //Verificar trocas, moldura preenchida
                    lessUsed = GetNFU(process.Frames, pagesAccess);
                    indexInFrame = process.Frames.IndexOf(lessUsed);
                    process.Frames.RemoveAt(indexInFrame);
                    process.Frames.Insert(indexInFrame, currentPage);
                    pagesAccess[lessUsed - 1] = 0;
                    ++pagesAccess[currentPage - 1];
                    ++changesAmount;
                }
            }
            return changesAmount;
        }

        private static int Otimo(MemoryProcess process){
            int index = 0, aux = 0, changesAmount = 0, pageHigherTime = 0, indexPageHigherTime = 0;
            //Página nova
            for(int i = 0; i < process.Pages.Count; ++i){
                int currentPage = process.Pages[i];
                if(!process.Frames.Contains(currentPage) && (index < process.AmountOfFrames)){
                    process.Frames.Insert(index, currentPage);
                    ++index;
                    ++aux;
                    ++changesAmount;
                }else if(process.Frames.Contains(currentPage)){ //Página repetida
                    ++aux;
                }else{ //Moldura preenchida
                    pageHigherTime = GetHigherTime(process, aux);
                    indexPageHigherTime = process.Frames.IndexOf(pageHigherTime);
                    process.Frames.RemoveAt(indexPageHigherTime);
                    process.Frames.Insert(indexPageHigherTime, currentPage);
                    ++aux;
                    ++changesAmount;
                }
            }
            return changesAmount;
        }

        //Funções auxiliares para o MRU
       
        //Obtem indice do MRU na moldura
        private static int GetIndexMRU(int[] pos){
            int max = pos[0];
            int index = 0;
            for(int i = 0; i < pos.Length; ++i){
                if(pos[i] > max)
                {
                    max = pos[i];
                    index = i;
                }
            }
            return index;
        }

        // As páginas que não forem acessadas tem seu tempo aumentado em 1
        private static void IncreaseTime(int aux, int[] usedPos){
            for(int i = 0; i < aux; ++i)
                ++usedPos[i];
        }

        //Função auxiliar para NFU

        //Retorna elemento com menos acesso frequente
        private static int GetNFU(List<int> frames, int[] pagesAccess){
            int m = int.MaxValue;
            int r = -1;
            for(int i = 0; i < pagesAccess.Length; ++i){
                if(frames.Contains(i + 1) && (pagesAccess[i]) < m){
                    m = pagesAccess[i];
                    r = i + 1;
                }
            }
            return r;
        }

        //Função auxiliar para Ótimo
        
        //Encontra página que vai levar mais tempo
        private static int GetHigherTime(MemoryProcess process, int start){
            int time = 0;
            Dictionary<int, int> dict = new Dictionary<int, int>();

            for(int i = 0; i < process.Frames.Count; ++i){
                int currentFrame = process.Frames[i];
                for(int j = 0; j < process.Pages.Count; ++j){
                    ++time;
                    if(currentFrame == process.Pages[i])
                        break;
                }
                dict.Add(currentFrame, time);
                time = 0;
            }
            return dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }

        public static void Main(string[] args)
        {
            List<MemoryProcess> processes = FileManipulation.GetAndListProcesses(@"C:\dev\furg\memorypagination\processes.txt");
            PrintProcesses(processes);

            int[] changesAmount = new int[4];
            int min = 0;
            string betterAlg = string.Empty;
            bool simpleOutput = true;

            for(int i = 0; i < processes.Count; ++i){
                MemoryProcess currentProcess = processes[i];

                changesAmount[0] = FIFO(currentProcess);
                currentProcess.Frames.Clear();
                changesAmount[1] = MRU(currentProcess);
                currentProcess.Frames.Clear();
                changesAmount[2] = NFU(currentProcess);
                currentProcess.Frames.Clear();
                changesAmount[3] = Otimo(currentProcess);
                currentProcess.Frames.Clear();
                
                min = changesAmount.Min();

                for(int j = 0; j < changesAmount.Length; ++j){
                    if(changesAmount[j] == min){
                        switch(j){
                            case 0:
                                betterAlg += "FIFO,";
                                break;
                            case 1:
                                betterAlg += "MRU,";
                                break;
                            case 2: 
                                betterAlg += "NUF,";
                                break;
                            case 3:
                                betterAlg += "Otimo,";
                                break;
                        }
                    }
                }

                betterAlg = betterAlg.Substring(0, betterAlg.Length - 1); //Remove última virgula
                if(simpleOutput){
                    string aux = betterAlg;
                    if(betterAlg.Equals("FIFO,MRU,NUF,Otimo"))
                        aux = "Empate";
                    Console.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}", changesAmount[0], changesAmount[1], changesAmount[2], changesAmount[3], aux));
                }else{
                    Console.WriteLine(string.Format("Avaliação dos algoritmos (por trocas):\n--> FIFO [{0}]\n--> MRU [{1}]\n--> NFU [{2}]\n--> Ótimo [{3}]\n--> Melhor(es): [{4}]", changesAmount[0], changesAmount[1], changesAmount[2], changesAmount[3], betterAlg));
                }
                betterAlg = string.Empty;

            }

            Console.WriteLine("Fim");
        }
    }
}
