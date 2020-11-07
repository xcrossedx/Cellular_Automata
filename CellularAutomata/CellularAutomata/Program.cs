using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLIGE_2;

namespace CellularAutomata
{
    class Program
    {
        static void Main()
        {
            Game.Init(new Coord(0, 0), new Coord(1000, 1000), 2);
            Game.gameTicker.delay = 0.15f;
            Hive.Init();

            bool playing = true;
            bool running = false;
            bool placing = false;

            while (playing)
            {
                while (!Console.KeyAvailable)
                {
                    Game.PreUpdate();
                    if (running & !placing) { Hive.Update(); if (Hive.CheckCells()) { running = false; Game.viewport.position = new Coord(Game.viewport.size, "centerongame"); } }
                    Game.Update(true);
                }

                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.Escape) { playing = false; }
                if (placing)
                {
                    if (input == ConsoleKey.Spacebar) { Hive.ChangeHighlight(); }
                    else if (input == ConsoleKey.Enter) { placing = false; Hive.ClearHighlight(); }
                    else if (input == ConsoleKey.UpArrow | input == ConsoleKey.W) 
                    {
                        if (Hive.highlight.y <= Game.viewport.position.y + (Game.viewport.size.y / 2)) { Game.MoveViewport(new Vector(0, -1), false); }
                        Hive.MoveHighlight(new Vector(0, -1));
                    }
                    else if (input == ConsoleKey.RightArrow | input == ConsoleKey.D)
                    {
                        if (Hive.highlight.x >= Game.viewport.position.x + (Game.viewport.size.x / 2)) { Game.MoveViewport(new Vector(1, 0), false); }
                        Hive.MoveHighlight(new Vector(1, 0));
                    }
                    else if (input == ConsoleKey.DownArrow | input == ConsoleKey.S)
                    {
                        if (Hive.highlight.y >= Game.viewport.position.y + (Game.viewport.size.y / 2)) { Game.MoveViewport(new Vector(0, 1), false); }
                        Hive.MoveHighlight(new Vector(0, 1));
                    }
                    else if (input == ConsoleKey.LeftArrow | input == ConsoleKey.A)
                    {
                        if (Hive.highlight.x <= Game.viewport.position.x + (Game.viewport.size.x / 2)) { Game.MoveViewport(new Vector(-1, 0), false); }
                        Hive.MoveHighlight(new Vector(-1, 0));
                    }
                }
                else
                {
                    if (input == ConsoleKey.Spacebar)
                    {
                        if (!running) { running = true; Hive.Start(); }
                        else { running = false; Game.viewport.position = new Coord(Game.viewport.size, "centerongame"); Hive.Clear(); }
                    }
                    else if (input == ConsoleKey.Enter) { placing = true; Hive.SetHighlight(); }
                    else if (input == ConsoleKey.R & running) { running = false; Hive.Restart(); }
                    else if (input == ConsoleKey.UpArrow | input == ConsoleKey.W) { Game.MoveViewport(new Vector(0, -1), false); }
                    else if (input == ConsoleKey.RightArrow | input == ConsoleKey.D) { Game.MoveViewport(new Vector(1, 0), false); }
                    else if (input == ConsoleKey.DownArrow | input == ConsoleKey.S) { Game.MoveViewport(new Vector(0, 1), false); }
                    else if (input == ConsoleKey.LeftArrow | input == ConsoleKey.A) { Game.MoveViewport(new Vector(-1, 0), false); }
                }
            }
        }
    }
}
