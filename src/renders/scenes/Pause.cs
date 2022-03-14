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
            sprites["background"] = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            sprites["background"].SetTexture(ressources.UITextures["background"]);
            textButtons = new Dictionary<string, TextButton>();
            textButtons["Reprendre"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 125, GetScreenHeight() / 2 - 150), new Vector2(250, 100), SKYBLUE, new Vector2(2, 15), "Reprendre", BLACK, 50);
            textButtons["Quitter"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 125, GetScreenHeight() / 2 + 100 - 50), new Vector2(250, 100), SKYBLUE, new Vector2(5, 15), "Quitter", BLACK, 50);

            textButtons["Reprendre"].SetVisibility(true);
            textButtons["Quitter"].SetVisibility(true);
        }
        ~Pause() { }
        public new void Update()
        {
            DrawRectangleRounded(new Rectangle((float)GetScreenWidth() / 2 - 250, (float)GetScreenHeight() / 2 - 250, 500, 500), 0.10f, 0, GRAY);
             foreach (var i in buttons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in sprites) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in toggleButtons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in inputboxs) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in texts) { if (i.Value.GetTextVisibility()) i.Value.Draw(); }
             foreach (var i in textButtons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
        }
        public new void Draw()
        {
             foreach (var i in buttons) { if (i.Value.GetVisibility()) i.Value.Update(); }
             foreach (var i in toggleButtons) { if (i.Value.GetVisibility()) i.Value.Update(); }
             foreach (var i in inputboxs) { if (i.Value.GetVisibility()) i.Value.Update(); }
        }
    };
}
