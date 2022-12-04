using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Sudoku_Solver_2022_.Program;
using static Sudoku_Solver_2022_.Program.Sudoku;

namespace Sudoku_Solver_2022_
{
    internal class Program
    {
        public static int S;
        public static int globalScore;
        public static int besteScore;
        public static int[][] keyLijst = new int[18][];
        public static Sudoku.Vakje[] bijhouden = new Sudoku.Vakje[2];
        public static Sudoku.Vakje[] sudoku = new Sudoku.Vakje[81];

        static void Main(string[] args)
        {
            Random random = new Random();
            Sudoku sudoku1 = new Sudoku();
            besteScore = 144;
            globalScore = 144;

            //0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0

            string[] input1 = Console.ReadLine().Split(' ');
            int[] input = new int[input1.Length];
            for (int i = 0; i < input1.Length; i++)
            {
                input[i] = int.Parse(input1[i]);
            }

            Sudoku.Vakje[] arrayVlakjes = new Sudoku.Vakje[input.Length];

            int row = 1;
            int column = 1;

            Console.ReadLine();

            for (int x = 0; x < input.Length; x++)
            {
                if (input[x] == 0)
                {
                    Sudoku.Vakje temp3 = new Sudoku.Vakje(0);
                    arrayVlakjes[x] = sudoku1.Toevoegen0(temp3, 0, row, column);

                    if (column < 9)
                    {
                        column++;
                    }
                    else
                    {
                        column = 1;
                        row++;
                    }
                }
                else
                {
                    Sudoku.Vakje temp2 = new Sudoku.Vakje(input[x]);

                    arrayVlakjes[x] = sudoku1.ToevoegenDefault(temp2, input[x], row, column);
                    if (column < 9)
                    {
                        column++;
                    }
                    else
                    {
                        column = 1;
                        row++;
                    }
                }
            }

            Printen(arrayVlakjes);
            Console.ReadLine();
            VulSudokus(MaakAlleBlokken(arrayVlakjes));
            Printen(arrayVlakjes);
            Console.ReadLine();
            CI(arrayVlakjes);
            int checkPlateau = 0;
            S = 1;
            int iscool = 1;
            while (globalScore != 0)
            {
                besteScore = 0;
                var randomIndex = random.Next(0, 9);
                var randomBlok = MaakAlleBlokken(arrayVlakjes)[randomIndex]; //kiesrandom blok               

                if (iscool % 9 == 0)
                {
                    if (globalScore == checkPlateau || globalScore == checkPlateau - 1 || globalScore == checkPlateau + 1 || globalScore == checkPlateau + 2 || globalScore == checkPlateau - 2)
                    {
                        for (int i = 0; i < S; i++)
                        {
                            var randomIndex1 = random.Next(0, 9);
                            var randomIndex2 = random.Next(0, 9);

                            WisselBeleid(arrayVlakjes, randomBlok[randomIndex1], randomBlok[randomIndex2]);
                        }
                    }
                    checkPlateau = globalScore;
                }

                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 8; j > 0; j--)
                        {
                            UpdateScoreBijhouden(arrayVlakjes, randomBlok[i], randomBlok[j]);
                        }
                    }
                    WisselBeleid(arrayVlakjes, bijhouden[0], bijhouden[1]);
                    globalScore = CI(arrayVlakjes);
                }
                iscool++;
            }
            Console.WriteLine(iscool);
            Printen(arrayVlakjes);
            Console.WriteLine("dit is met wissel");
            Console.ReadLine();
        }

        public static void VulSudokus(Sudoku.Vakje[][] arrayVlakjes)
        {
            Sudoku.Vakje searchElement1 = new Sudoku.Vakje(1);
            Sudoku.Vakje searchElement2 = new Sudoku.Vakje(2);
            Sudoku.Vakje searchElement3 = new Sudoku.Vakje(3); 
            Sudoku.Vakje searchElement4 = new Sudoku.Vakje(4); 
            Sudoku.Vakje searchElement5 = new Sudoku.Vakje(5); 
            Sudoku.Vakje searchElement6 = new Sudoku.Vakje(6);  
            Sudoku.Vakje searchElement7 = new Sudoku.Vakje(7);  
            Sudoku.Vakje searchElement8 = new Sudoku.Vakje(8);  
            Sudoku.Vakje searchElement9 = new Sudoku.Vakje(9); 

            foreach (var e in arrayVlakjes)
            {
                if (!Array.Exists(e, element => element.key == searchElement1.key))
                {
                    VulSudoku(e, 1);
                }
                if (!Array.Exists(e, element => element.key == searchElement2.key))
                {
                    VulSudoku(e, 2);
                }
                if (!Array.Exists(e, element => element.key == searchElement3.key))
                {
                    VulSudoku(e, 3);
                }
                if (!Array.Exists(e, element => element.key == searchElement4.key))
                {                                                           
                    VulSudoku(e, 4);                                        
                }                                                           
                if (!Array.Exists(e, element => element.key == searchElement5.key))
                {                                                          
                    VulSudoku(e, 5);                                       
                }                                                          
                if (!Array.Exists(e, element => element.key == searchElement6.key))
                {                                                           
                    VulSudoku(e, 6);                                        
                }                                                           
                if (!Array.Exists(e, element => element.key == searchElement7.key))
                {                                                          
                    VulSudoku(e, 7);                                       
                }                                                          
                if (!Array.Exists(e, element => element.key == searchElement8.key))
                {                                                          
                    VulSudoku(e, 8);                                       
                }                                                          
                if (!Array.Exists(e, element => element.key == searchElement9.key))
                {
                    VulSudoku(e, 9);
                }
            }
        }

        public static void VulSudoku(Sudoku.Vakje[] arrayVlakjes, int key)
        {
            for(int i =0; i < arrayVlakjes.Length; i++)
            {
                if (arrayVlakjes[i].vast == false && arrayVlakjes[i].begonnenNulgetal == true)
                {
                    arrayVlakjes[i].key = key;

                    arrayVlakjes[i].begonnenNulgetal = false;
                    break;
                }
            }
        }

        public static Sudoku.Vakje[][] MaakAlleBlokken(Sudoku.Vakje[] arrayVlakjes)
        {
            Sudoku.Vakje[][] SudokuBlokkenArray = new Sudoku.Vakje[9][];
            SudokuBlokkenArray[0] = MaakBlok(arrayVlakjes, 0);
            SudokuBlokkenArray[1] = MaakBlok(arrayVlakjes, 3);
            SudokuBlokkenArray[2] = MaakBlok(arrayVlakjes, 6);
            SudokuBlokkenArray[3] = MaakBlok(arrayVlakjes, 27);
            SudokuBlokkenArray[4] = MaakBlok(arrayVlakjes, 30);
            SudokuBlokkenArray[5] = MaakBlok(arrayVlakjes, 33);
            SudokuBlokkenArray[6] = MaakBlok(arrayVlakjes, 54);
            SudokuBlokkenArray[7] = MaakBlok(arrayVlakjes, 57);
            SudokuBlokkenArray[8] = MaakBlok(arrayVlakjes, 60);

            return SudokuBlokkenArray;
        }

        public static void PrintAlleblokken(Sudoku.Vakje[] arrayVlakjes) //was een check, niet nodig
        {
            Printen(MaakBlok(arrayVlakjes, 0));
            Printen(MaakBlok(arrayVlakjes, 3));
            Printen(MaakBlok(arrayVlakjes, 6));
            Printen(MaakBlok(arrayVlakjes, 27));
            Printen(MaakBlok(arrayVlakjes, 30));
            Printen(MaakBlok(arrayVlakjes, 33));
            Printen(MaakBlok(arrayVlakjes, 54));
            Printen(MaakBlok(arrayVlakjes, 57));
            Printen(MaakBlok(arrayVlakjes, 60));
        }         

        public static void Printen(Sudoku.Vakje[] arrayVlakjes)
        {
            int counter = 0;
            for (int i = 0; i < Math.Sqrt(arrayVlakjes.Length); ++i)
            {
                for (int j = 0; j < Math.Sqrt(arrayVlakjes.Length); ++j)
                {
                    Console.Write(arrayVlakjes[j + counter].key + "|");                  

                }
                counter += (int)Math.Sqrt(arrayVlakjes.Length);
                Console.WriteLine();
            }
        }    
        
        public static Sudoku.Vakje[] MaakBlok(Sudoku.Vakje[] arrayVlakjes, int beginIndex)
        {
            Sudoku.Vakje[] sudokuNieuw = new Sudoku.Vakje[9];

            int i = 1;
            int counter = 0;
            while(i < 22)
            {
                if(i % 3 == 0)
                {
                    if (arrayVlakjes[i - 1 + beginIndex].key == 0)
                    {
                        sudokuNieuw[counter] = arrayVlakjes[i - 1 + beginIndex];

                    }
                    else
                    {
                        sudokuNieuw[counter] = arrayVlakjes[i - 1 + beginIndex];
                    }
                    counter++;
                    i += 7;                    
                }

                else
                {
                    
                    if (arrayVlakjes[i-1 + beginIndex].key == 0)
                    {
                        sudokuNieuw[counter] = arrayVlakjes[i - 1 + beginIndex];
                    }
                    else
                    {
                        sudokuNieuw[counter] = arrayVlakjes[i - 1 + beginIndex];
                    }
                    counter++;
                    i++;
                }               
            }

            return sudokuNieuw;
        }

        public static void WisselBeleid(Sudoku.Vakje[] arrayvlakjes, Sudoku.Vakje links, Sudoku.Vakje rechts)
        {
            if (links.vast == false && rechts.vast == false)
            {
                (rechts.key, links.key) = (links.key, rechts.key);                
            }
        }
        public static void SwapTweeVlakjes(Sudoku.Vakje[] arrayvlakjes)
        {
            for(int i = 0; i < arrayvlakjes.Length; i++)
            {
                for (int j = arrayvlakjes.Length - 1; j >= 0; j--) {
                    if (arrayvlakjes[i].vast == false)
                    {
                        (arrayvlakjes[j], arrayvlakjes[i]) = (arrayvlakjes[i], arrayvlakjes[j]);
                    }
                }
            }           
        }

        public static void UpdateScoreBijhouden(Sudoku.Vakje[] arrayVlakjes, Sudoku.Vakje eerste, Sudoku.Vakje tweede) // deze kan wss sneller
        {
            if (eerste.vast == false && tweede.vast == false)
            {
                var temp = new int[4][];
                var temp1 = new int[9];
                var temp2 = new int[9];
                var temp3 = new int[9];
                var temp4 = new int[9];
             
                temp[0] = temp1;
                temp[1] = temp2;
                temp[2] = temp3;
                temp[3] = temp4;

                if (eerste.column == tweede.column && eerste.row != tweede.row)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        temp[0][i] = keyLijst[eerste.row + 8][i];
                        temp[1][i] = keyLijst[tweede.row + 8][i];
                    }
                }

                if (eerste.row == tweede.row && eerste.column != tweede.column)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        temp[0][i] = keyLijst[eerste.column - 1][i];
                        temp[1][i] = keyLijst[tweede.column - 1][i];
                    }
                }
                if (eerste.row != tweede.row && eerste.column != tweede.column)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        temp[0][i] = keyLijst[eerste.row + 8][i];
                        temp[1][i] = keyLijst[tweede.row + 8][i];
                        temp[2][i] = keyLijst[eerste.column - 1][i];
                        temp[3][i] = keyLijst[tweede.column - 1][i];
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    if (temp[0][i] == eerste.key)
                    {
                        temp[0][i] = tweede.key;
                        break;
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (temp[1][i] == tweede.key)
                    {
                        temp[1][i] = eerste.key;
                        break;
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    if (temp[2][i] == eerste.key)
                    {
                        temp[2][i] = tweede.key;
                        break;
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    if (temp[3][i] == tweede.key)
                    {
                        temp[3][i] = eerste.key;
                        break;
                    }
                }
             
                VergelijkUpdatedScoreMetBestaande(temp, eerste, tweede);
            }
        }

        public  static void VergelijkUpdatedScoreMetBestaande(int[][] nieuweScore, Sudoku.Vakje eerste, Sudoku.Vakje tweede)
        {
            int tempScore = 0;

            if (eerste.column == tweede.column && eerste.row != tweede.row)
            {
                int iniScoreRow = (keyLijst[eerste.row + 8].Distinct().Count());
                int tweedeScoreRow = (nieuweScore[0].Distinct().Count());
                int iniScoreRow2 = (keyLijst[tweede.row + 8].Distinct().Count());
                int tweedeScoreRow2 = (nieuweScore[1].Distinct().Count());
              
                tempScore = ((iniScoreRow + iniScoreRow2) - (tweedeScoreRow + tweedeScoreRow2));

            }

            if (eerste.row == tweede.row && eerste.column != tweede.column)
            {
                int iniScorecolumn = (keyLijst[eerste.column].Distinct().Count());
                int tweedeScorecolumn = (nieuweScore[0].Distinct().Count());
                int iniScorecolumn2 = (keyLijst[tweede.column].Distinct().Count());
                int tweedeScorecolumn2 = (nieuweScore[1].Distinct().Count());

                tempScore = ( (iniScorecolumn + iniScorecolumn2) - (tweedeScorecolumn + tweedeScorecolumn2));


            }
            if (eerste.row != tweede.row && eerste.column != tweede.column)
            {
                int iniScorecolumn = ( keyLijst[eerste.column].Distinct().Count());
                int tweedeScorecolumn = ( nieuweScore[2].Distinct().Count());
                int iniScoreRow = ( keyLijst[eerste.row + 8].Distinct().Count());
                int tweedeScoreRow = ( nieuweScore[0].Distinct().Count());
                int iniScorecolumn2 = (keyLijst[tweede.column].Distinct().Count());
                int tweedeScorecolumn2 = ( nieuweScore[3].Distinct().Count());
                int iniScoreRow2 = (keyLijst[tweede.row + 8].Distinct().Count());
                int tweedeScoreRow2 = (nieuweScore[1].Distinct().Count());

                tempScore = (iniScoreRow + iniScoreRow2 + iniScorecolumn + iniScorecolumn2) - (tweedeScoreRow + tweedeScoreRow2 + tweedeScorecolumn + tweedeScorecolumn2);

            }

            if (tempScore < besteScore)
            {
                besteScore = tempScore;
                // update best scorende tweetal
                bijhouden[0] = eerste;
                bijhouden[1] = tweede;
            }
        }
        public static int CI (Sudoku.Vakje[] arrayVlakjes) //parameter meegeven om de juiste twee rijen en kolommen te updaten
        {
            int score = 0;
            int counter1 = 0;
            int counter2 = 0;
            int counter3 = 0;
            int counter4 = 0;
            int counter5 = 0;
            int counter6 = 0;
            int counter7 = 0;
            int counter8 = 0;
            int counter9 = 0;
            int counter10 = 0;
            int counter11 = 0;
            int counter12 = 0;
            int counter13 = 0;
            int counter14 = 0;
            int counter15 = 0;
            int counter16 = 0;
            int counter17 = 0;
            int counter18 = 0;

            int[] keylijst1 = new int[9];
            int[] keylijst2 = new int[9];
            int[] keylijst3 = new int[9];
            int[] keylijst4 = new int[9];
            int[] keylijst5 = new int[9];
            int[] keylijst6 = new int[9];
            int[] keylijst7 = new int[9];
            int[] keylijst8 = new int[9];
            int[] keylijst9 = new int[9];
            int[] keylijst10 = new int[9];
            int[] keylijst11 = new int[9];
            int[] keylijst12 = new int[9];
            int[] keylijst13 = new int[9];
            int[] keylijst14 = new int[9];
            int[] keylijst15 = new int[9];
            int[] keylijst16 = new int[9];
            int[] keylijst17 = new int[9];
            int[] keylijst18 = new int[9];
          

            for (int i = 0; i < arrayVlakjes.Length; i++)
            {
                if (arrayVlakjes[i].column == 1)
                {
                    keylijst1[counter1] = arrayVlakjes[i].key;

                    counter1++;

                }
                if (arrayVlakjes[i].column == 2)
                {
                    keylijst2[counter2] = arrayVlakjes[i].key;
                    counter2++;
                }
                if (arrayVlakjes[i].column == 3)
                {
                    keylijst3[counter3] = arrayVlakjes[i].key;
                    counter3++;
                }
                if (arrayVlakjes[i].column == 4)
                {
                    keylijst4[counter4] = arrayVlakjes[i].key;
                    counter4++;
                }
                if (arrayVlakjes[i].column == 5)
                {
                    keylijst5[counter5] = arrayVlakjes[i].key;
                    counter5++;
                }
                if (arrayVlakjes[i].column == 6)
                {
                    keylijst6[counter6] = arrayVlakjes[i].key;
                    counter6++;
                }
                if (arrayVlakjes[i].column == 7)
                {
                    keylijst7[counter7] = arrayVlakjes[i].key;
                    counter7++;
                }
                if (arrayVlakjes[i].column == 8)
                {
                    keylijst8[counter8] = arrayVlakjes[i].key;
                    counter8++;
                }
                if (arrayVlakjes[i].column == 9)
                {
                    keylijst9[counter9] = arrayVlakjes[i].key;
                    counter9++;
                }

                //rows

                if (arrayVlakjes[i].row == 1)
                {
                    keylijst10[counter10] = arrayVlakjes[i].key;
                    counter10++;
                }
                if (arrayVlakjes[i].row == 2)
                {
                    keylijst11[counter11] = arrayVlakjes[i].key;
                    counter11++;
                }
                if (arrayVlakjes[i].row == 3)
                {
                    keylijst12[counter12] = arrayVlakjes[i].key;
                    counter12++;
                }
                if (arrayVlakjes[i].row == 4)
                {
                    keylijst13[counter13] = arrayVlakjes[i].key;
                    counter13++;
                }
                if (arrayVlakjes[i].row == 5)
                {
                    keylijst14[counter14] = arrayVlakjes[i].key;
                    counter14++;
                }
                if (arrayVlakjes[i].row == 6)
                {
                    keylijst15[counter15] = arrayVlakjes[i].key;
                    counter15++;
                }
                if (arrayVlakjes[i].row == 7)
                {
                    keylijst16[counter16] = arrayVlakjes[i].key;
                    counter16++;
                }
                if (arrayVlakjes[i].row == 8)
                {
                    keylijst17[counter17] = arrayVlakjes[i].key;
                    counter17++;
                }
                if (arrayVlakjes[i].row == 9)

                {
                    keylijst18[counter18] = arrayVlakjes[i].key;
                    counter18++;
                }            

            }

            keyLijst[1] = keylijst2;
            keyLijst[2] = keylijst3;
            keyLijst[3] = keylijst4;
            keyLijst[4] = keylijst5;
            keyLijst[5] = keylijst6;
            keyLijst[6] = keylijst7;
            keyLijst[7] = keylijst8;
            keyLijst[8] = keylijst9;
            keyLijst[9] = keylijst10;
            keyLijst[10] = keylijst11;
            keyLijst[11] = keylijst12;
            keyLijst[12] = keylijst13;
            keyLijst[13] = keylijst14;
            keyLijst[14] = keylijst15;
            keyLijst[15] = keylijst16;
            keyLijst[16] = keylijst17;
            keyLijst[17] = keylijst18;
            keyLijst[0] = keylijst1;


            foreach (var e in keyLijst)
            {                
                score += (9 - e.Distinct().Count());                
            }

            return score;
        }

        public class Sudoku
        {
            public class Vakje
            {
                public int key;
                public int row;
                public int column;
                public bool vast;
                public bool begonnenNulgetal;

                public Vakje(int key)
                {
                    this.key = key;
                }                
            }

            public Vakje Toevoegen0(Vakje vakje, int key, int row, int column)
            {
                vakje.key = key;
                vakje.row = row;
                vakje.column = column;
                vakje.vast = false;
                vakje.begonnenNulgetal = true;
                return vakje; 
            }
            public Vakje ToevoegenDefault(Vakje vakje, int key, int row, int column)
            {
                vakje.key = key;
                vakje.row = row;
                vakje.column = column;
                vakje.vast = true;
                return vakje;
            }                  
        }
    }
}