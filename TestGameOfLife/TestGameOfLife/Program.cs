using System;

namespace TestGameOfLife
{
    public class LifeSimulation
    {
        private const int Heigth = 25;
        private int Width = 110;
        private static bool[,] cells; //Przechowuje stan osobnika - żywy / martwy
        private static bool[,] newCells;

        public LifeSimulation()
        {
            cells = new bool[Heigth, Width];
            GenerateField("glider");
        }

        public void DrawAndGrow()
        {
            DrawGame();
            Grow();
        }

        private void Grow()
        {
            newCells = new bool[Heigth,Width];
            Array.Copy(cells, 0, newCells, 0, cells.Length);

            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int numOfAliveNeighbors = GetNeighbors(i, j);
                    if (cells[i, j])
                    {
                        if (numOfAliveNeighbors < 2)
                        {
                            newCells[i, j] = false; //Umiera z samotności
                        }
                        if (numOfAliveNeighbors > 3)
                        {
                            newCells[i, j] = false; //Umiera z przeludnienia
                        }
                    }
                    else
                    {
                        if (numOfAliveNeighbors == 3)
                        {
                            newCells[i, j] = true; //Przeżywa
                        }
                    }
                }
            }
            Array.Copy(newCells, 0, cells, 0, newCells.Length);
        }

        private int GetNeighbors(int x, int y)
        {
            int NumOfAliveNeighbors = 0;
            int count = 0;
            int outerLoopCount = 0;
            int innerLoopCount = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                outerLoopCount++;
                for (int j = y - 1; j <= y + 1; j++)
                {
                    innerLoopCount++;
                    if (!(outerLoopCount == 2 && innerLoopCount == 5))
                    {
                        if (!((i < 0 || j < 0) || i >= Heigth || j >= Width))
                        {
                            if (cells[i, j] == true)
                            {
                                NumOfAliveNeighbors++;
                            }
                            count++;
                        }
                    }
                }
            }
            return NumOfAliveNeighbors;
        }
        private void DrawGame()
        {
            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(cells[i, j] ? "o" : " ");
                    if (j == Width - 1) Console.WriteLine("\r");
                }
            }
            Console.SetCursorPosition(0, Console.WindowTop);
        }

        private void GenerateField(string type)
        {
            if (type == "glider") //GLIDER
            {
                resetField();

                cells[5, 4] = true; cells[5, 5] = true; cells[5, 6] = true;
                cells[4, 6] = true;
                cells[3, 5] = true;
            }
            else if (type == "block") //BLOCK
            {
                resetField();
                cells[1, 1] = true; cells[1, 2] = true; 
                cells[2, 1] = true; cells[2, 2] = true;
            }
            else if (type == "diehard")
            {
                resetField();
                cells[10, 17] = true;
                cells[11, 11] = true; cells[11, 12] = true;
                cells[12, 12] = true; cells[12, 16] = true; cells[12, 17] = true; cells[12, 18] = true;
            }
            else if (type == "singular")
            {
                resetField();
                cells[10, 17] = true;
            }
            else //RANDOM
            {
                resetField();

                Random rand = new Random();
                for (int i = 0; i < Heigth; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        /* LOSUJEMY POPULACJE POCZĄTKOWĄ */
                        if (rand.NextDouble() < .8)
                            cells[i, j] = false;
                        else
                            cells[i, j] = true;
                    }
                }
            }
        }

        private void resetField()
        {
            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    cells[i, j] = false;
                }
            }
        }
    }

    internal class Program
    {
        private const uint MaxRuns = 200;

        private static void Main(string[] args)
        {
            int runs = 0;
            LifeSimulation sim = new LifeSimulation();
            while (runs++ < MaxRuns)
            {
                sim.DrawAndGrow();

                System.Threading.Thread.Sleep(100);
            }
            System.Threading.Thread.Sleep(10000);
        }
    }
}