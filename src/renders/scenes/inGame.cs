using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Flags_csharp.src.datas;
using System.Numerics;

namespace Flags_csharp.src.renders.scenes
{
    enum GameMode
    {
        NONE,
        MAP,
        CAPITALE,
        FLAGS
    };

    enum GameMode2
    {
        NONE,
        ALL,
        AFRIQUE,
        EUROPE,
        ASIE,
        OCEANIE,
        AMERIQUE
    };

    class InGame : Scene
    {
        GameMode mMode = GameMode.NONE;
        GameMode2 mMode2 = GameMode2.NONE;
        int mRandomId;
        bool mWin = false;
        bool isDeclared = false;                // True if a country is taken.
        int mPoints = 0;
        public InGame() { }
        public InGame(Ressource ressources)
        {
            var background = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            background.Texture = ressources.UITextures["background"];
            UiComponents["background"] = background;

            UiComponents["Title"] = new Text("The World Quiz", new Vector2((GetScreenWidth() / 2) - (MeasureText("The World Quiz", 100)) / 2, 100), 100, Color.ORANGE);
            // Initialize every country in each category.
            for (int i = 0; i < CountriesDatas.countriesDatas.Count; i++)
            {
                countries.Add(new Country(i));
                if (countries[i].GetContinent() == "Afrique") { countriesAfrique.Add(countries[i]); }
                if (countries[i].GetContinent() == "Europe") { countriesEurope.Add(countries[i]); }
                if (countries[i].GetContinent() == "Asie") { countriesAsie.Add(countries[i]); }
                if (countries[i].GetContinent() == "Oceanie") { countriesOceanie.Add(countries[i]); }
                if (countries[i].GetContinent() == "Amerique") { countriesAmerique.Add(countries[i]); }
            }
            UiComponents["Pause"] = new Button(false, new Vector2(100, 100), new Vector2(50, 50), 0.05f, SKYBLUE, SKYBLUE);

            // GameMode Buttons.
            UiComponents["Capitale"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Find capitale\n by country", BLACK, 35);
            UiComponents["Map"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 0), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Find country\n by map", BLACK, 35);
            UiComponents["Flag"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Find country\n by Flag", BLACK, 35);
            UiComponents["back"] = new Button(true, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0, BLUE, BLUE);

            // GameMode 2 Buttons.
            UiComponents["All"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "All continent", BLACK, 35);
            UiComponents.Values.Last().Visible = false;
            UiComponents["Afrique"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 125), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Afrique", BLACK, 35);
            UiComponents.Values.Last().Visible = false;
            UiComponents["Europe"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Europe", BLACK, 35);
            UiComponents.Values.Last().Visible = false;
            UiComponents["Asie"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 125), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Asie", BLACK, 35);
            UiComponents.Values.Last().Visible = false;
            UiComponents["Oceanie"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Oceanie", BLACK, 35);
            UiComponents.Values.Last().Visible = false;
            UiComponents["Amerique"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 375), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Amerique", BLACK, 35);
            UiComponents.Values.Last().Visible = false;

            // Inputbox.
            var input = new InputBox(new Vector2(GetScreenWidth() / 2 - 450, GetScreenWidth() / 2 - 100), new Vector2(900, 100), 34, SKYBLUE, new Vector2(20, 10), 50);
            input.Visible = false;
            UiComponents["input"] = input;

            // Texts.
            UiComponents["Find"] = new Text("", new Vector2(GetScreenWidth() / 2 - 450, GetScreenWidth() / 2 - 175), 50, WHITE);
            UiComponents["Country"] = new Text("", new Vector2(GetScreenWidth() / 2 - 500, 250), 50, BLACK);
            UiComponents["AnswerText"] = new Text("", new Vector2(10, GetScreenHeight() - 200), 50, BLACK);
            UiComponents["AnswerStatus"] = new Text("", new Vector2(GetScreenWidth() / 2 + 100, 800), 50, GREEN);
            UiComponents["NumberOfCountries"] = new Text("", new Vector2(GetScreenWidth() - 300, GetScreenHeight() - 200), 30, YELLOW);
            UiComponents["FlagSprite"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 200), new Vector2(), WHITE);
            UiComponents["MapSprite"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 200), new Vector2(), WHITE);

            // Answer Button.
            UiComponents["Answer"] = new ToggleButton(new Button(true, new Vector2(200, GetScreenHeight() - 100), new Vector2(50, 50), 0, SKYBLUE, PINK), false);

            foreach (var i in UiComponents) { Console.WriteLine($"{ i.Key} : {i.Value.ToString()}"); }
        }
        ~InGame() { }

        void SetGameMode1(GameMode mode)
        {
            mMode = mode;
            if (mode != GameMode.NONE)
            {
                UiComponents["Capitale"].Visible = false;
                UiComponents["Map"].Visible = false;
                UiComponents["Flag"].Visible = false;

                UiComponents["All"].Visible = true;
                UiComponents["Afrique"].Visible = true;
                UiComponents["Europe"].Visible = true;
                UiComponents["Asie"].Visible = true;
                UiComponents["Oceanie"].Visible = true;
                UiComponents["Amerique"].Visible = true;
            }
            else
            {
                UiComponents["Capitale"].Visible = true;
                UiComponents["Map"].Visible = true;
                UiComponents["Flag"].Visible = true;

                UiComponents["All"].Visible = false;
                UiComponents["Afrique"].Visible = false;
                UiComponents["Europe"].Visible = false;
                UiComponents["Asie"].Visible = false;
                UiComponents["Oceanie"].Visible = false;
                UiComponents["Amerique"].Visible = false;
            }
        }

        void SetGameMode2(GameMode2 mode)
        {
            mMode2 = mode;

            UiComponents["All"].Visible = false;
            UiComponents["Afrique"].Visible = false;
            UiComponents["Europe"].Visible = false;
            UiComponents["Asie"].Visible = false;
            UiComponents["Oceanie"].Visible = false;
            UiComponents["Amerique"].Visible = false;

            UiComponents["input"].Visible = true;
            UiComponents["Pause"].Visible = true;
            UiComponents["Title"].Visible = true;
        }

        public new void Update()
        {
            // Update Ui.
            foreach (var i in UiComponents) { i.Value.Update(); }
            if (mMode == GameMode.NONE)
            {
                // Update GameMode Buttons.
                if (UiComponents["Capitale"].IsClicked()) { SetGameMode1(GameMode.CAPITALE); }
                else if (UiComponents["Map"].IsClicked()) { SetGameMode1(GameMode.MAP); }
                else if (UiComponents["Flag"].IsClicked()) { SetGameMode1(GameMode.FLAGS); }
            }
            else if (mMode2 == GameMode2.NONE)
            {
                // Update GameMode2 Buttons.
                if (UiComponents["All"].IsClicked()) { SetGameMode2(GameMode2.ALL); }
                else if (UiComponents["Afrique"].IsClicked()) { SetGameMode2(GameMode2.AFRIQUE); }
                else if (UiComponents["Europe"].IsClicked()) { SetGameMode2(GameMode2.EUROPE); }
                else if (UiComponents["Asie"].IsClicked()) { SetGameMode2(GameMode2.ASIE); }
                else if (UiComponents["Oceanie"].IsClicked()) { SetGameMode2(GameMode2.OCEANIE); }
                else if (UiComponents["Amerique"].IsClicked()) { SetGameMode2(GameMode2.AMERIQUE); }
                else if (UiComponents["back"].IsClicked()) { SetGameMode1(GameMode.NONE); }
            }
            // If GameMode choose.
            else
            {
                // Get random Country for each GameModes (if is not declared).
                if (!isDeclared)
                {
                    switch (mMode2)
                    {
                        case GameMode2.ALL: mRandomId = GetRandomValue(0, (int)countries.Count() - 1); break;
                        case GameMode2.AFRIQUE: mRandomId = GetRandomValue(0, (int)countriesAfrique.Count() - 1); break;
                        case GameMode2.EUROPE: mRandomId = GetRandomValue(0, (int)countriesEurope.Count() - 1); break;
                        case GameMode2.ASIE: mRandomId = GetRandomValue(0, (int)countriesAsie.Count() - 1); break;
                        case GameMode2.OCEANIE: mRandomId = GetRandomValue(0, (int)countriesOceanie.Count() - 1); break;
                        case GameMode2.AMERIQUE: mRandomId = GetRandomValue(0, (int)countriesAmerique.Count() - 1); break;
                        default: break;
                    }
                    isDeclared = true;
                }

                // Play Mode by GameMode.
                switch (mMode2)
                {
                    case GameMode2.ALL: Question(mMode, countries, mRandomId); break;
                    case GameMode2.AFRIQUE: Question(mMode, countriesAfrique, mRandomId); break;
                    case GameMode2.EUROPE: Question(mMode, countriesEurope, mRandomId); break;
                    case GameMode2.ASIE: Question(mMode, countriesAsie, mRandomId); break;
                    case GameMode2.OCEANIE: Question(mMode, countriesOceanie, mRandomId); break;
                    case GameMode2.AMERIQUE: Question(mMode, countriesAmerique, mRandomId); break;
                    default: break;
                }
            }
        }
        public new void Draw()
        {
            foreach (var i in UiComponents)
            {
                i.Value.Draw();
            }
            if (mMode != GameMode.NONE && mMode2 != GameMode2.NONE)
            {
                UiComponents["Title"].Visible = (false);
            }
            else
            {
                UiComponents["Title"].Visible = (true);
            }
        }

        public bool IsValid(string text1, string text2, int ratio)
        {
            int good = 0;
            int length = (text1.Length < text2.Length) ? text1.Length : text2.Length;
            for (int i = 0; i < length; i++)
            {
                text1.ToCharArray()[i] = Char.ToLower(text1.ToCharArray()[i]);
                text2.ToCharArray()[i] = Char.ToLower(text2.ToCharArray()[i]);
                if (text1.ToCharArray()[i] == text2.ToCharArray()[i])
                    good++;
            }
            if (good >= ratio * text2.Length / 100)
                return true;
            else
                return false;
        }

        public static int frameCount = 0;
        public static int size_at_start = -1;
        public static bool isLoaded = false;

        public void Question(GameMode mode, List<Country> countries, int mRandomId)
        {
            if (size_at_start == -1)
                size_at_start = (int)countries.Count();
            switch (mode)
            {
                case GameMode.MAP:
                    {
                        if (!isLoaded)
                        {
                            countries[mRandomId].SetMap(LoadTexture($"assets/maps/{countries[mRandomId].GetIso()}.png"));
                            isLoaded = true;
                            var Map = (UiComponents["MapSprite"] as Sprite);
                            Map.Texture = (countries[mRandomId].GetMap().Last());
                            if (Map.Texture.height > 700)
                            {
                                Map.Size = (new Vector2(500, 300));
                                Map.Position = (new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2));
                            }
                            else
                            {
                                Map.Size = (new Vector2(countries[mRandomId].GetMap()[0].width, countries[mRandomId].GetMap()[0].height));
                                Map.Origin = (new Vector2(Map.Texture.width / 2, Map.Texture.height / 2));
                            }
                            Map.Origin = (new Vector2(Map.Texture.width / 2, Map.Texture.height / 2));
                            Map.Visible = true;
                        }
                        (UiComponents["Find"] as Text).Content = ("Find Country :");
                        (UiComponents["AnswerText"] as Text).Content = ($"{countries[mRandomId].GetCountryName()}");
                    }
                    break;
                case GameMode.CAPITALE:
                    {
                        (UiComponents["Country"] as Text).Content = ($"Country : {countries[mRandomId].GetCountryName()}");
                        (UiComponents["AnswerText"] as Text).Content = ($"{countries[mRandomId].GetCapitale()}");
                    }
                    break;
                case GameMode.FLAGS:
                    {
                        if (!isLoaded)
                        {
                            countries[mRandomId].SetFlag(LoadTexture($"assets/flags/{countries[mRandomId].GetIso()}.png"));
                            isLoaded = true;
                            var flag = (Sprite)UiComponents["FlagSprite"];
                            flag.Texture = (countries[mRandomId].GetFlag()[0]);
                            flag.Size = (new Vector2(countries[mRandomId].GetFlag()[0].width / 4, countries[mRandomId].GetFlag()[0].height / 4));
                            flag.Visible = true;
                            flag.Origin = (new Vector2((countries[mRandomId].GetFlag()[0].width / 4) / 2, (countries[mRandomId].GetFlag()[0].height / 4) / 2));
                        }
                        (UiComponents["Find"] as Text).Content = ("Find Country :");
                        (UiComponents["AnswerText"] as Text).Content = ($"{countries[mRandomId].GetCountryName()}");
                    }
                    break;
                default:
                    break;
            }
            if (IsKeyReleased(KeyboardKey.KEY_ENTER))
            {
                (UiComponents["input"] as InputBox).SetClicked(true);
                (UiComponents["AnswerStatus"] as Text).TextVisibility = (true);
                frameCount = 60;
                // If 75% of the characters are good then it's valid.
                if (IsValid((UiComponents["input"] as InputBox).GetText(), countries[mRandomId].GetCountryName(), 75) && frameCount > 0)
                {
                    (UiComponents["AnswerStatus"] as Text).Content = "Good result";
                    (UiComponents["AnswerStatus"] as Text).TextColor = (GREEN);
                    countries.Remove(countries[mRandomId]);
                    isDeclared = false;
                    isLoaded = false;
                    mPoints++;
                }
                //Wrong result
                else
                {
                    (UiComponents["AnswerStatus"] as Text).Content = "Wrong result";
                    (UiComponents["AnswerStatus"] as Text).TextColor = (RED);
                }

                (UiComponents["input"] as InputBox).ClearInput();
            }

            // Number of Countries Text.
            (UiComponents["NumberOfCountries"] as Text).Content = $"{mPoints}/{size_at_start}";

            // Answer Button.
            if ((UiComponents["Answer"] as ToggleButton).IsToggle())
                (UiComponents["AnswerText"] as Text).TextVisibility = (true);
            else
                (UiComponents["AnswerText"] as Text).TextVisibility = (false);

            // Check if all was found, if true win.
            if (countries.Count() <= 0)
            {
                mWin = true;
                size_at_start = -1;
                frameCount = 0;
            }
            frameCount--;

            // Set Visibility to false when time is elapsed.
            if (frameCount <= 0)
            {
                (UiComponents["AnswerStatus"] as Text).TextVisibility = (false);
                frameCount = 0;
            }

        }
        public GameMode GetGameMode() { return mMode; }
        public bool Win() { return mWin; }
    };
}
