using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLIGE_2;

namespace CellularAutomata
{
    static class Automata
    {
        public static Entity[][] cellGrid;

        public static void Init()
        {
            cellGrid = new Entity[Game.size.y][];

            for (int y = 0; y < Game.size.y; y++)
            {
                cellGrid[y] = new Entity[Game.size.x];

                for (int x = 0; x < Game.size.x; x++)
                {
                    cellGrid[y][x] = new Entity($"Cell {y}-{x}", 0, new Coord(x, y), new Sprite[2] { new Sprite(0, 0), new Sprite(0, 8) });
                }
            }
        }

        public static void Randomize()
        {
            Clear();

            int remainingCells = (cellGrid.Length * cellGrid[0].Length) / 10;

            while (remainingCells > 0)
            {
                int y = Tools.rng.Next(0, cellGrid.Length);
                int x = Tools.rng.Next(0, cellGrid[0].Length);

                if (cellGrid[y][x].activeSprite == 0)
                {
                    cellGrid[y][x].activeSprite = 1;
                    remainingCells -= 1;
                }
            }
        }

        public static void Update()
        {
            if (Game.gameTicker.Check(false))
            {
                for (int y = 0; y < cellGrid.Length; y++)
                {
                    for (int x = 0; x < cellGrid[0].Length; x++)
                    {
                        int yMin = y - 1;
                        int yMax = y + 2;
                        int xMin = x - 1;
                        int xMax = x + 2;

                        if (yMin < 0) { yMin = 0; }
                        if (yMax > cellGrid.Length) { yMax = cellGrid.Length; }
                        if (xMin < 0) { xMin = 0; }
                        if (xMax > cellGrid[0].Length) { xMax = cellGrid[0].Length; }

                        int neighbors = 0;

                        for (int cy = yMin; cy < yMax; cy++)
                        {
                            for (int cx = xMin; cx < xMax; cx++)
                            {
                                if (cellGrid[cy][cx].activeSprite == 1)
                                {
                                    neighbors += 1;
                                }
                            }
                        }

                        if (neighbors == 3) { cellGrid[y][x].activeSprite = 1; }
                        else if (neighbors == 2 & cellGrid[y][x].activeSprite == 1) { cellGrid[y][x].activeSprite = 1; }
                        else { cellGrid[y][x].activeSprite = 0; }
                        cellGrid[y][x].Refresh();
                    }
                }
            }
        }

        public static void Clear()
        {
            for (int y = 0; y < cellGrid.Length; y++)
            {
                for (int x = 0; x < cellGrid[0].Length; x++)
                {
                    cellGrid[y][x].activeSprite = 0;
                    cellGrid[y][x].Refresh();
                }
            }
        }
    }
}
