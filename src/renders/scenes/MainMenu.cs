using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Raylib_cs;

namespace Flags_csharp.src.renders.scenes
{
    class MainMenu : Scene
    {
        public MainMenu() { }
        public MainMenu(Ressource ressources)
        {
            var background = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            background.Texture = ressources.UITextures["background"];
            UiComponents["background"] = background;

            UiComponents["Title"] = new Text("The World Quiz", new Vector2((GetScreenWidth() / 2) - (MeasureText("The World Quiz", 100)) / 2, 100), 100, Color.ORANGE);
            //texts["Title"];
            UiComponents["play"] = new TexturedButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 200), new Vector2(300, 100), ressources.UITextures["button"], ButtonType.Text);
            (UiComponents.Values.Last() as TexturedButton).SetText("PLAY", new Vector2(75, 30), 50, BLACK);
            UiComponents["settings"] = new TexturedButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 50), new Vector2(300, 100), ressources.UITextures["button"], ButtonType.Text);
            (UiComponents.Values.Last() as TexturedButton).SetText("SETTINGS", new Vector2(12, 30), 50, BLACK);
            UiComponents["quit"] = new TexturedButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 100), new Vector2(300, 100), ressources.UITextures["button"], ButtonType.Text);
            (UiComponents.Values.Last() as TexturedButton).SetText("QUIT", new Vector2(75, 30), 50, BLACK);
        }
        ~MainMenu() { }
        public new void Update()
        {
            foreach (var i in UiComponents)
            {
                i.Value.Update();
            }
        }
        public new void Draw()
        {
            foreach (var i in UiComponents)
            {
                i.Value.Draw();
            }
        }
    }
}
