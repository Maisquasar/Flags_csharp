using System;
using Flags_csharp.src;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.ConfigFlags;


namespace Flags_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int screenHeight = 1080;
            const int screenWeight = 1920;

            InitWindow(screenWeight, screenHeight, "Flags");
            InitAudioDevice();
            SetTargetFPS(60);
            var installDirectory = AppContext.BaseDirectory + "..\\..\\..\\";
            System.IO.Directory.SetCurrentDirectory(installDirectory);

            Game game = new Game();
            while (!WindowShouldClose())
            {
                BeginDrawing();

                ClearBackground(new Color(50,50,50,255));
                game.Update();
                game.Draw();

                EndDrawing();
            }
        }
    }
}
