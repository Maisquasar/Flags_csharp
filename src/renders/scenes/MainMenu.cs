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
            sprites["background"] = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            sprites["background"].SetTexture(ressources.UITextures["background"]);
            texts["Title"] = new Text("The World Quiz", new Vector2((GetScreenWidth() / 2) - (MeasureText("The World Quiz", 100))/2, 100), 100, Color.ORANGE);
            texturedButtons["play"] = new TexturedButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 200), new Vector2(300, 100), ressources.UITextures["button"], ButtonType.Text);
            texturedButtons["play"].SetText("PLAY", new Vector2(75, 30), 50, BLACK);
            texturedButtons["settings"] = new TexturedButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 50), new Vector2(300, 100), ressources.UITextures["button"], ButtonType.Text);
            texturedButtons["settings"].SetText("SETTINGS", new Vector2(12, 30), 50, BLACK);
            texturedButtons["quit"] = new TexturedButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 100), new Vector2(300, 100), ressources.UITextures["button"], ButtonType.Text);
            texturedButtons["quit"].SetText("QUIT", new Vector2(75, 30), 50, BLACK);
        }
        ~MainMenu() { }
        public new void Update() 
        {
            foreach (var i in texturedButtons) { i.Value.Update(); }
            foreach (var i in textButtons) { i.Value.Update(); }
            foreach (var i in buttons) { i.Value.Update(); }
            foreach (var i in toggleButtons) { i.Value.Update(); }
            foreach (var i in inputboxs) { i.Value.Update(); }
        }
        public new void Draw()
        {
            foreach (var i in sprites) { i.Value.Draw(); }
            foreach (var i in texts) { i.Value.Draw(); }
            foreach (var i in textButtons) { i.Value.Draw(); }
            foreach (var i in textButtons)
                DrawRectangleLinesEx(new Rectangle((float)i.Value.GetPos().X, (float)i.Value.GetPos().Y, (float)i.Value.GetSize().X, (float)i.Value.GetSize().Y), 5, BLUE);
            foreach (var i in texturedButtons) { i.Value.Draw(); }
        }
    }
}
