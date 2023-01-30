using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Flags_csharp.src.datas;
using Flags_csharp.src;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Raylib_cs;

namespace Flags_csharp.src.renders.scenes
{
    class Settings : Scene
    {
        bool mButtonIsClicked;
        Country mCountry;
        
        public Settings() { }
        public Settings(Ressource ressources)
        {
            /*
            sprites["background"] = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            sprites["background"].SetTexture(ressources.UITextures["background"]);
            buttons["mainMenu"] = new Button(true, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0.05f, BLUE, BLUE);
            buttons["back"] = new Button(true, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0.05f, BLUE, BLUE);
            buttons["back"].SetVisibility(false);
            // Define all texts buttons.
            for (int i = 0; i < 197; i++)
            {
                countries.Add(new Country(i));
                textButtons[countries[i].GetCountryName()] = new TextButton(new Vector2(GetScreenWidth() / 2 - 400, 200 + i * 175), new Vector2(800, 125), SKYBLUE, new Vector2(20, 50), countries[i].GetCountryName(), WHITE, 35);
                sprites[countries[i].GetCountryName()] = new Sprite(false, new Vector2(GetScreenWidth() / 2 + 200, 207 + i * 175), new Vector2(), WHITE);

                textButtons[countries[i].GetCountryName()].SetVisibility(false);
            }

            sprites["Flag"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 200), new Vector2(), WHITE);
            sprites["Map"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 200), new Vector2(), WHITE);
            texts["Country"] = new Text("", new Vector2(100, 20), 100, BLACK);
            texts["Capitale"] = new Text("", new Vector2(100, 200), 75, DARKGRAY);
            texts["Continent"] = new Text("", new Vector2(100, 500), 50, LIGHTGRAY);

             foreach (var i in texts) { i.Value.SetTextVisibility(false); }
                sprites["Flag"].SetVisibility(false);
        }
        ~Settings() { }
        public new void Update()
        {
            // Update all objects
             foreach (var i in textButtons) { if (i.Value.GetVisibility()) i.Value.Update(); }
             foreach (var i in buttons) { if (i.Value.GetVisibility()) i.Value.Update(); }
             foreach (var i in toggleButtons) { if (i.Value.GetVisibility()) i.Value.Update(); }
             foreach (var i in inputboxs) { if (i.Value.GetVisibility()) i.Value.Update(); }
        }
        public new void Draw()
        {
            sprites["background"].Draw();
            DrawButtons();
             foreach (var i in textButtons)
            {
                if (i.Value.GetVisibility())
                {
                    i.Value.Draw();
                    DrawRectangleLinesEx(new Rectangle((float)i.Value.GetPos().X, (float)i.Value.GetPos().Y, (float)i.Value.GetSize().X, (float)i.Value.GetSize().Y), 5, BLUE);
                }
            }
             foreach (var i in buttons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in toggleButtons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in inputboxs) { if (i.Value.GetVisibility()) i.Value.Draw(); }
             foreach (var i in texts) { if (i.Value.GetTextVisibility()) i.Value.Draw(); }
             foreach (var i in sprites) { if (i.Value.GetVisibility() && i.Key != "background") i.Value.Draw(); }

        }
        static int sTime = 1;
        public void DrawButtons()
        {
            if (!mButtonIsClicked && sTime < 0)
            {
                buttons["mainMenu"].SetVisibility(true);
                for (int i = 0; i < 197; i++)
                {
                    if (GetMouseWheelMove() > 0 && Game.InferiorOrEqual(textButtons[countries[0].GetCountryName()].GetPos(),new Vector2(GetScreenWidth(), 80)))
                    {
                         foreach (var j in textButtons)
                            j.Value.SetPos(new Vector2(j.Value.GetPos().X, j.Value.GetPos().Y + 1));
                         foreach (var j in sprites)
                            if (j.Key != "background")
                                j.Value.SetPos(new Vector2(j.Value.GetPos().X, j.Value.GetPos().Y + 1));
                    }
                    else if (GetMouseWheelMove() < 0 && Game.SuperiorOrEqual(textButtons[countries[(int)(countries.Count() - 1)].GetCountryName()].GetPos(),new Vector2(0, GetScreenHeight() - 200)))
                    {
                         foreach (var j in textButtons)
                            j.Value.SetPos(new Vector2(j.Value.GetPos().X, j.Value.GetPos().Y - 1));
                         foreach (var j in sprites)
                            if (j.Key != "background")
                                j.Value.SetPos(new Vector2(j.Value.GetPos().X, j.Value.GetPos().Y - 1));
                    }
                    // Check if the button is visible
                    if (Game.SuperiorOrEqual(textButtons[countries[i].GetCountryName()].GetPos(), new Vector2(0, 0) - textButtons[countries[i].GetCountryName()].GetSize()) && Game.InferiorOrEqual(textButtons[countries[i].GetCountryName()].GetPos(), new Vector2(GetScreenWidth(), GetScreenHeight()) + textButtons[countries[i].GetCountryName()].GetSize()))
                    {
                        // Check if flag is loaded
                        if ((int)countries[i].GetFlag().Count() == 0)
                        {
                            // If not, load flag
                            countries[i].SetFlag(LoadTexture($"assets/flags/{countries[i].GetIso()}.png"));
                            sprites[countries[i].GetCountryName()].SetTexture(countries[i].GetFlag()[0]);
                            // Set size.
                            if (countries[i].GetFlag()[0].height / 20 < textButtons[countries[i].GetCountryName()].GetSize().Y)
                                sprites[countries[i].GetCountryName()].SetSize(new Vector2(countries[i].GetFlag()[0].width, countries[i].GetFlag()[0].height) / 20);
                            else
                                sprites[countries[i].GetCountryName()].SetSize(new Vector2(countries[i].GetFlag()[0].width / 20, textButtons[countries[i].GetCountryName()].GetSize().Y - 20));
                        }
                        textButtons[countries[i].GetCountryName()].SetVisibility(true);
                        sprites[countries[i].GetCountryName()].SetVisibility(true);
                    }
                    else
                    {
                        textButtons[countries[i].GetCountryName()].SetVisibility(false);
                        sprites[countries[i].GetCountryName()].SetVisibility(false);
                    }
                    if (textButtons[countries[i].GetCountryName()].IsClicked() && textButtons[countries[i].GetCountryName()].GetVisibility())
                    {
                        mButtonIsClicked = true;
                        mCountry = countries[i];
                    }
                }
            }
            else if (sTime < 0)
            {
                //-Show Info of country
                 foreach (var i in textButtons) { i.Value.SetVisibility(false); }
                 foreach (var i in sprites) { i.Value.SetVisibility(false); }
                 foreach (var i in buttons) { i.Value.SetVisibility(false); }
                buttons["back"].SetVisibility(true);
                //-Set Flag pos
                sprites["Flag"].SetVisibility(true);
                sprites["Flag"].SetTexture(mCountry.GetFlag()[0]);
                sprites["Flag"].SetPos(new Vector2(200, 200));
                sprites["Flag"].SetSize(new Vector2(mCountry.GetFlag()[0].width, mCountry.GetFlag()[0].height) / 7);
                //-Set Text pos
                texts["Country"].SetTextPos(new Vector2(sprites["Flag"].GetPos().X + sprites["Flag"].GetSize().X + 250, 200));
                texts["Country"].SetText(mCountry.GetCountryName());
                texts["Country"].SetTextVisibility(true);
                texts["Capitale"].SetTextPos(new Vector2(sprites["Flag"].GetPos().X + sprites["Flag"].GetSize().X + 250, 315));
                texts["Capitale"].SetText($"Capitale : {mCountry.GetCapitale()}");
                texts["Capitale"].SetTextVisibility(true);
                texts["Continent"].SetTextPos(new Vector2(sprites["Flag"].GetPos().X + sprites["Flag"].GetSize().X + 250, 400));
                texts["Continent"].SetText($"Continent : {mCountry.GetContinent()}");
                texts["Continent"].SetTextVisibility(true);
                //-Set Map pos
                sprites["Map"].SetVisibility(true);
                // Check if map is loaded
                if ((int)mCountry.GetMap().Count() == 0)
                    mCountry.SetMap(LoadTexture($"assets/maps/{mCountry.GetIso()}.png"));
                sprites["Map"].SetTexture(mCountry.GetMap()[0]);
                if (sprites["Map"].GetTexture().width > 500)
                    sprites["Map"].SetSize(new Vector2(500, 300));
                else
                    sprites["Map"].SetSize(new Vector2(sprites["Map"].GetTexture().width, sprites["Map"].GetTexture().height));
                sprites["Map"].SetPos(new Vector2(200, sprites["Flag"].GetPos().Y + sprites["Flag"].GetSize().Y + 20));

                DrawRectangle((int)sprites["Map"].GetPos().X, (int)sprites["Map"].GetPos().Y, (int)sprites["Map"].GetSize().X, (int)sprites["Map"].GetSize().Y, BLUE);
                DrawRectangleLinesEx(new Rectangle((float)sprites["Map"].GetPos().X - 4, (float)sprites["Map"].GetPos().Y - 4, (float)sprites["Map"].GetSize().X + 8, (float)sprites["Map"].GetSize().Y + 8), 4, BLACK);
                if (buttons["back"].IsClicked())
                {
                    mButtonIsClicked = false;
                     foreach (var i in textButtons) { i.Value.SetVisibility(false); }
                     foreach (var i in sprites) { i.Value.SetVisibility(false); }
                     foreach (var i in buttons) { i.Value.SetVisibility(false); }
                     foreach (var i in texts) { i.Value.SetTextVisibility(false); }
                }
            }
            sTime--;
            */
        }
        
    }
}
