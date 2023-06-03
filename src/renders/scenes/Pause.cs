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
    class Pause : Scene
    {
        public Pause(Ressource ressources)
        {
            UiComponents["background"] = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            ((Sprite)UiComponents["background"]).Texture = ressources.UITextures["background"];
            UiComponents["Reprendre"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 125, GetScreenHeight() / 2 - 150), new Vector2(250, 100), SKYBLUE, new Vector2(2, 15), "Reprendre", BLACK, 50);
            UiComponents["Quitter"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 125, GetScreenHeight() / 2 + 100 - 50), new Vector2(250, 100), SKYBLUE, new Vector2(5, 15), "Quitter", BLACK, 50);

            UiComponents["Reprendre"].Visible = true;
            UiComponents["Quitter"].Visible = true;
        }
        ~Pause() { }
        public new void Update()
        {
             foreach (var i in UiComponents) {i.Value.Draw(); }
        }
        public new void Draw()
        {
             foreach (var i in UiComponents) { i.Value.Update(); }
        }
    };
}
