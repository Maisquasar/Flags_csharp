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
        float ScrollSensitivity = 25f;

        public Settings() { }
        public Settings(Ressource ressources)
        {
            UiComponents["background"] = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            (UiComponents.Values.Last() as Sprite).Texture = ressources.UITextures["background"];
            UiComponents["mainMenu"] = new Button(true, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0.05f, BLUE, BLUE);
            UiComponents["back"] = new Button(true, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0.05f, BLUE, BLUE);
            UiComponents.Values.Last().Visible = false;
            // Define all texts buttons.
            for (int i = 0; i < CountriesDatas.countriesDatas.Count; i++)
            {
                countries.Add(new Country(i));
                UiComponents[countries[i].GetCountryName() + "TextButton"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 400, 200 + i * 175), new Vector2(800, 125), SKYBLUE, new Vector2(20, 50), countries[i].GetCountryName(), WHITE, 35);
                UiComponents[countries[i].GetCountryName() + "Sprite"] = new Sprite(false, new Vector2(GetScreenWidth() / 2 + 200, 225 + i * 175), new Vector2(), WHITE);

                UiComponents[countries[i].GetCountryName() + "TextButton"].Visible = false;
            }

            UiComponents["Flag"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 500), new Vector2(), WHITE);
            UiComponents["Map"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2), new Vector2(), WHITE);
            UiComponents["Country"] = new Text("", new Vector2(100, 20), 100, BLACK);
            UiComponents["Capitale"] = new Text("", new Vector2(100, 200), 75, DARKGRAY);
            UiComponents["Continent"] = new Text("", new Vector2(100, 500), 50, LIGHTGRAY);
            UiComponents["return"] = new Button(false, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0.05f, BLUE, BLUE);

            foreach (var i in UiComponents.Values.OfType<Text>()) { i.Visible = false; }
            UiComponents["Flag"].Visible = false;
        }
        ~Settings() { }

        Country CountryInfo = null;

        public new void Update()
        {
            // Update all objects
            foreach (var i in UiComponents)
            {
                i.Value.Update();
            }
            if (CountryInfo == null)
            {
                foreach (var i in countries)
                {
                    // Setting Sprite and TextButton Position
                    var t = UiComponents[i.GetCountryName() + "TextButton"] as TextButton;
                    var sprite = UiComponents[i.GetCountryName() + "Sprite"] as Sprite;
                    t.Position = new Vector2(t.Position.X, t.Position.Y + GetMouseWheelMove() * ScrollSensitivity);
                    sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + GetMouseWheelMove() * ScrollSensitivity);
                    // Load Texture if not yet loaded
                    if (sprite.Position.Y + sprite.Size.Y >= 0 && sprite.Position.Y - sprite.Size.Y <= GetScreenHeight())
                    {
                        if (i.GetFlag().Count == 0)
                        {
                            sprite.Texture = LoadTexture($"assets/flags/{i.GetIso()}.png");
                            i.SetFlag(sprite.Texture);
                            sprite.Size = new Vector2(0.05f * sprite.Texture.width, 0.05f * sprite.Texture.height);
                        }
                        sprite.Visible = true;
                        t.Visible = true;
                    }
                    // Hide Sprite if Visible and out of bound
                    else if (sprite.Visible && (sprite.Position.Y + sprite.Size.Y < 0 || sprite.Position.Y - sprite.Size.Y > GetScreenHeight()))
                    {
                        sprite.Visible = false;

                        t.Visible = false;
                        //Console.WriteLine(i.GetCountryName());
                    }
                    if (t.IsClicked())
                    {
                        ShowInformation(i);
                        break;
                    }
                }
            }
            else
            {
                if (UiComponents["return"].IsClicked())
                {
                    foreach (var i in UiComponents)
                    {
                        i.Value.Visible = false;
                    }
                    UiComponents["background"].Visible = true;
                    UiComponents["back"].Visible = true;

                    CountryInfo = null;
                }
            }
        }

        public void ShowInformation(Country country)
        {
            CountryInfo = country;
            foreach (var i in UiComponents)
            {
                i.Value.Visible = false;
            }
            UiComponents["background"].Visible = true;
            UiComponents["return"].Visible = true;

            UiComponents["Flag"].Visible = true;
            var flag = UiComponents["Flag"] as Sprite;
            flag.Texture = CountryInfo.GetFlag()[0];
            flag.Size = new Vector2(0.2f * flag.Texture.width, 0.2f * flag.Texture.height);

            UiComponents["Map"].Visible = true;
            var map = UiComponents["Map"] as Sprite;

            if (CountryInfo.GetMap().Count > 0)
                map.Texture = CountryInfo.GetMap()[0];
            else
                map.Texture = LoadTexture($"assets/maps/{CountryInfo.GetIso()}.png");
            map.Size = new Vector2(map.Texture.width, map.Texture.height);

            UiComponents["Country"].Visible = true;
            var countryName = UiComponents["Country"] as Text;
            countryName.Content = $"Name : {CountryInfo.GetCountryName()}";

            UiComponents["Capitale"].Visible = true;
            var capitale = UiComponents["Capitale"] as Text;
            capitale.Content = $"Capitale : {CountryInfo.GetCapitale()}";

            UiComponents["Continent"].Visible = true;
            var continent = UiComponents["Continent"] as Text;
            continent.Content = $"Continent : {CountryInfo.GetContinent()}";
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
