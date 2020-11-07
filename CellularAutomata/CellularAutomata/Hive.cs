using CLIGE_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace CellularAutomata
{
    static class Hive
    {
        //This bool determines if the random cell placement is limited to the viewport or if it can fill the entire game grid.
        public static bool limitRandomizedCells = false;

        //This is the update delay is seconds for the cell grid.
        public static float updateDelay = 0.1f;

        //These are the color settings for when a cell is dead or alive.
        public static int deadColor = 0;
        public static int aliveColor = 14;

        //These are the color settings for when a highlighted cell will place or remove the cell under it.
        public static int placeColor = 10;
        public static int removeColor = 4;

        //This is where any and all rules are checked for every cell. the values passed into this method are the state of the cell being checked and the number of living neighbors it has to be used to define rules.
        private static bool CheckRules(bool alive, int neighbors)
        {
            bool cellLives = false;

            if (alive)
            {
                if (neighbors == 2 | neighbors == 3) { cellLives = true; }
            }
            else
            {
                if (neighbors == 3) { cellLives = true; }
            }

            return cellLives;
        }

        public static Entity cells;
        public static Sprite start;
        public static Ticker ticker;
        public static Coord highlight;
        public static bool highlightAlive;

        public static void Init()
        {
            ticker = new Ticker(updateDelay);
            cells = new Entity("Cell Grid", 0, new Coord(Game.size, "centerongame"), new Sprite(Game.size, "  ", 0, deadColor));
        }

        public static void Start()
        {
            if (CheckCells()) { Randomize(); }
            start = new Sprite(cells.spriteSheet[0][0].grid);
        }

        public static void Restart()
        {
            cells.spriteSheet[0][0] = new Sprite(start.grid);
        }

        public static bool CheckCells()
        {
            bool isClear = true;

            for (int y = 0; y < Game.size.y; y++)
            {
                for (int x = 0; x < Game.size.x; x++)
                {
                    if (cells.spriteSheet[0][0].grid[y][x].background == aliveColor) { isClear = false; }
                }
            }

            return isClear;
        }

        public static void Randomize()
        {
            Clear();
            Sprite startingCells = new Sprite(cells.spriteSheet[0][0].grid);

            int remainingCells;

            if (limitRandomizedCells) { remainingCells = (Game.viewport.size.x * Game.viewport.size.y) / 10; }
            else { remainingCells = (Game.size.x * Game.size.y) / 10; }

            while (remainingCells > 0)
            {
                int y;
                int x;

                if (limitRandomizedCells)
                {
                    y = Tools.rng.Next(Game.viewport.position.y, Game.viewport.position.y + Game.viewport.size.y);
                    x = Tools.rng.Next(Game.viewport.position.x, Game.viewport.position.x + Game.viewport.size.x);
                }
                else
                {
                    y = Tools.rng.Next(0, Game.size.y);
                    x = Tools.rng.Next(0, Game.size.x);
                }

                if (startingCells.grid[y][x].background == deadColor)
                {
                    startingCells.grid[y][x].background = aliveColor;
                    remainingCells -= 1;
                }
            }

            cells.spriteSheet[0][0] = startingCells;
            cells.Refresh();
        }

        public static void Update()
        {
            if (ticker.Check(true))
            {
                Sprite updatedCells = new Sprite(Game.size, "  ", 0, deadColor);

                for (int y = 0; y < Game.size.y; y++)
                {
                    for (int x = 0; x < Game.size.x; x++)
                    {
                        int yMin = y - 1;
                        int yMax = y + 2;
                        int xMin = x - 1;
                        int xMax = x + 2;

                        if (yMin < 0) { yMin = 0; }
                        if (yMax > Game.size.y) { yMax = Game.size.y; }
                        if (xMin < 0) { xMin = 0; }
                        if (xMax > Game.size.x) { xMax = Game.size.x; }

                        int neighbors = 0;

                        for (int cy = yMin; cy < yMax; cy++)
                        {
                            for (int cx = xMin; cx < xMax; cx++)
                            {
                                if (cy != y | cx != x)
                                {
                                    if (cells.spriteSheet[0][0].grid[cy][cx].background == aliveColor) { neighbors += 1; }
                                }
                            }
                        }

                        if (CheckRules(cells.spriteSheet[0][0].grid[y][x].background == aliveColor, neighbors)) { updatedCells.grid[y][x].background = aliveColor; }
                        else { updatedCells.grid[y][x].background = deadColor; }
                    }
                }

                cells.spriteSheet[0][0] = updatedCells;
                cells.Refresh();
            }
        }

        public static void Clear()
        {
            cells.spriteSheet[0][0] = new Sprite(Game.size, "  ", 0, deadColor);
        }

        public static void SetHighlight()
        {
            highlight = new Coord(Game.viewport.position.x + (Game.viewport.size.x / 2), Game.viewport.position.y + (Game.viewport.size.y / 2));
            CheckHighlightAlive();
            UpdateHighlight();
        }

        public static void CheckHighlightAlive()
        {
            highlightAlive = cells.spriteSheet[0][0].grid[highlight.y][highlight.x].background == aliveColor;
        }

        public static void MoveHighlight(Vector direction)
        {
            ClearHighlight();
            if (highlight.x + direction.x > 0 & highlight.x + direction.x < Game.size.x - 1 & highlight.y + direction.y > 0 & highlight.y + direction.y < Game.size.y - 1) { highlight.Update(direction); }
            CheckHighlightAlive();
            UpdateHighlight();
        }

        public static void UpdateHighlight()
        {
            if (highlightAlive) { cells.spriteSheet[0][0].grid[highlight.y][highlight.x].background = removeColor; }
            else { cells.spriteSheet[0][0].grid[highlight.y][highlight.x].background = placeColor; }
        }

        public static void ChangeHighlight()
        {
            highlightAlive = !highlightAlive;
            UpdateHighlight();
        }

        public static void ClearHighlight()
        {
            if (highlightAlive) { cells.spriteSheet[0][0].grid[highlight.y][highlight.x].background = aliveColor; }
            else { cells.spriteSheet[0][0].grid[highlight.y][highlight.x].background = deadColor; }
        }
    }
}
