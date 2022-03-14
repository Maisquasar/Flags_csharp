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
        NONE2,
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
        GameMode2 mMode2 = GameMode2.NONE2;
        int mRandomId;
        bool mWin = false;
        bool isDeclared = false;                // True if a country is taken.
        int mPoints = 0;
        public InGame() { }
        public InGame(Ressource ressources)
        {
            texts["Title"] = new Text("The World Quiz", new Vector2((GetScreenWidth() / 2) - (MeasureText("The World Quiz", 100)) / 2, 100), 100, Color.ORANGE);
            sprites["background"] = new Sprite(true, new Vector2(), new Vector2(GetScreenWidth(), GetScreenHeight()), GRAY);
            sprites["background"].SetTexture(ressources.UITextures["background"]);
            // Initialize every country in each category.
            for (int i = 0; i < 197; i++)
            {
                countries.Add(new Country(i));
                if (countries[i].GetContinent() == "Afrique") { countriesAfrique.Add(countries[i]); }
                if (countries[i].GetContinent() == "Europe") { countriesEurope.Add(countries[i]); }
                if (countries[i].GetContinent() == "Asie") { countriesAsie.Add(countries[i]); }
                if (countries[i].GetContinent() == "Oceanie") { countriesOceanie.Add(countries[i]); }
                if (countries[i].GetContinent() == "Amerique") { countriesAmerique.Add(countries[i]); }
            }
            buttons["Pause"] = new Button(false, new Vector2(100, 100), new Vector2(50, 50), 0.05f, SKYBLUE, SKYBLUE);

            // GameMode Buttons.
            textButtons["Capitale"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Find capitale\n by country", BLACK, 35);
            textButtons["Map"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 0), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Find country\n by map", BLACK, 35);
            textButtons["Flag"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Find country\n by Flag", BLACK, 35);
            buttons["back"] = new Button(true, new Vector2(100, GetScreenHeight() - 100), new Vector2(50, 50), 0, BLUE, BLUE);

            // GameMode 2 Buttons.
            textButtons["All"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "All continent", BLACK, 35);
            textButtons["Afrique"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 - 125), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Afrique", BLACK, 35);
            textButtons["Europe"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Europe", BLACK, 35);
            textButtons["Asie"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 125), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Asie", BLACK, 35);
            textButtons["Oceanie"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 250), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Oceanie", BLACK, 35);
            textButtons["Amerique"] = new TextButton(new Vector2(GetScreenWidth() / 2 - 150, GetScreenHeight() / 2 + 375), new Vector2(300, 100), SKYBLUE, new Vector2(50, 5), "Amerique", BLACK, 35);
            foreach (var i in textButtons)
                i.Value.SetVisibility(false);
            // Show Buttons for GameMode.
            textButtons["Capitale"].SetVisibility(true);
            textButtons["Map"].SetVisibility(true);
            textButtons["Flag"].SetVisibility(true);

            // Inputbox.
            inputboxs["input"] = new InputBox(new Vector2(GetScreenWidth() / 2 - 450, GetScreenWidth() / 2 - 100), new Vector2(900, 100), 34, SKYBLUE, new Vector2(20, 10), 50);
            inputboxs["input"].SetVisibility(false);

            // Texts.
            texts["Find"] = new Text("", new Vector2(GetScreenWidth() / 2 - 450, GetScreenWidth() / 2 - 175), 50, WHITE);
            texts["Country"] = new Text("", new Vector2(GetScreenWidth() / 2 - 500, 250), 50, BLACK);
            texts["Answer"] = new Text("", new Vector2(10, GetScreenHeight() - 200), 50, BLACK);
            texts["AnswerStatus"] = new Text("", new Vector2(GetScreenWidth() / 2 + 100, 800), 50, GREEN);
            texts["NumberOfCountries"] = new Text("", new Vector2(GetScreenWidth() - 300, GetScreenHeight() - 200), 30, YELLOW);
            sprites["Flag"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 200), new Vector2(), WHITE);
            sprites["Map"] = new Sprite(true, new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2 - 200), new Vector2(), WHITE);

            // Answer Button.
            toggleButtons["Answer"] = new ToggleButton(new Button(true, new Vector2(200, GetScreenHeight() - 100), new Vector2(50, 50), 0, SKYBLUE, PINK), false);
        }
        ~InGame() { }
        public new void Update()
        {
            // Update Ui.
            foreach (var i in toggleButtons) { i.Value.Update(); }
            foreach (var i in textButtons) { i.Value.Update(); }
            foreach (var i in inputboxs) { i.Value.Update(); }
            foreach (var i in buttons) { i.Value.Update(); }
            if (mMode == GameMode.NONE)
            {
                // Update GameMode Buttons.
                if (textButtons["Capitale"].IsClicked()) { mMode = GameMode.CAPITALE; }
                else if (textButtons["Map"].IsClicked()) { mMode = GameMode.MAP; }
                else if (textButtons["Flag"].IsClicked()) { mMode = GameMode.FLAGS; }

                // Check for all buttons if clicked, if true Set visible all buttons except GameMode buttons.
                foreach (var i in textButtons)
                {
                    if (i.Value.IsClicked())
                    {
                        foreach (var j in textButtons) { j.Value.SetVisibility(true); }

                        textButtons["Capitale"].SetVisibility(false);
                        textButtons["Map"].SetVisibility(false);
                        textButtons["Flag"].SetVisibility(false);
                    }
                }
            }
            else if (mMode2 == GameMode2.NONE2)
            {
                // Update GameMode2 Buttons.
                if (textButtons["All"].IsClicked()) { mMode2 = GameMode2.ALL; }
                else if (textButtons["Afrique"].IsClicked()) { mMode2 = GameMode2.AFRIQUE; }
                else if (textButtons["Europe"].IsClicked()) { mMode2 = GameMode2.EUROPE; }
                else if (textButtons["Asie"].IsClicked()) { mMode2 = GameMode2.ASIE; }
                else if (textButtons["Oceanie"].IsClicked()) { mMode2 = GameMode2.OCEANIE; }
                else if (textButtons["Amerique"].IsClicked()) { mMode2 = GameMode2.AMERIQUE; }
                else if (buttons["back"].IsClicked()) { mMode = GameMode.NONE; }

                // Check for all text buttons if clicked, if true Set visible all text buttons & buttons.
                foreach (var i in textButtons)
                {
                    if (i.Value.IsClicked())
                    {
                        foreach (var j in textButtons)
                            j.Value.SetVisibility(false);
                        foreach (var j in buttons)
                            j.Value.SetVisibility(false);
                    }
                }

                // Check if buttons back is clicked.
                foreach (var i in buttons)
                {
                    if (i.Value.IsClicked())
                    {
                        foreach (var j in textButtons)
                            j.Value.SetVisibility(false);
                        textButtons["Capitale"].SetVisibility(true);
                        textButtons["Map"].SetVisibility(true);
                        textButtons["Flag"].SetVisibility(true);
                    }
                }
            }
            // If GameMode choose.
            else
            {
                buttons["Pause"].SetVisibility(true);
                inputboxs["input"].SetVisibility(true);
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
            sprites["background"].Draw();
            foreach (var i in texts) { if (i.Value.GetTextVisibility()) i.Value.Draw(); }
            if (mMode != GameMode.NONE && mMode2 != GameMode2.NONE2)
            {
                // Draw Ui When a GameMode is Choosen.
                foreach (var i in inputboxs) { if (i.Value.GetVisibility()) i.Value.Draw(); }
                foreach (var i in toggleButtons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
                foreach (var i in texts) { if (i.Value.GetTextVisibility()) i.Value.Draw(); }
                foreach (var i in sprites) { if (i.Value.GetVisibility() && i.Key != "background") i.Value.Draw(); }
                texts["Title"].SetTextVisibility(false);
            }
            else
            {
                texts["Title"].SetTextVisibility(true);
            }
            // Draw Buttons if visible.
            foreach (var i in textButtons) { if (i.Value.GetVisibility()) i.Value.Draw(); }
            foreach (var i in buttons) { if (i.Value.GetVisibility()) i.Value.Draw(); }

            // Draw rectangle around textButtons.
            foreach (var i in textButtons)
                if (i.Value.GetVisibility())
                    DrawRectangleLinesEx(new Rectangle((float)i.Value.GetPos().X, (float)i.Value.GetPos().Y, (float)i.Value.GetSize().X, (float)i.Value.GetSize().Y), 5, BLUE);
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
                            sprites["Map"].SetTexture(countries[mRandomId].GetMap().Last());
                            if (sprites["Map"].GetTexture().height > 700)
                            {
                                sprites["Map"].SetSize(new Vector2(500, 300));
                                sprites["Map"].SetPos(new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2));
                            }
                            else
                            {
                                sprites["Map"].SetSize(new Vector2(countries[mRandomId].GetMap()[0].width, countries[mRandomId].GetMap()[0].height));
                                sprites["Map"].SetOrigin(new Vector2(sprites["Map"].GetTexture().width / 2, sprites["Map"].GetTexture().height / 2));
                            }
                            sprites["Map"].SetOrigin(new Vector2(sprites["Map"].GetTexture().width / 2, sprites["Map"].GetTexture().height / 2));
                            sprites["Map"].SetVisibility(true);
                        }
                        texts["Find"].SetText("Find Country :");
                        texts["Answer"].SetText($"{countries[mRandomId].GetCountryName()}");
                    }
                    break;
                case GameMode.CAPITALE:
                    {
                        texts["Country"].SetText($"Country : {countries[mRandomId].GetCountryName()}");
                        texts["Answer"].SetText($"{countries[mRandomId].GetCapitale()}");
                    }
                    break;
                case GameMode.FLAGS:
                    {
                        if (!isLoaded)
                        {
                            countries[mRandomId].SetFlag(LoadTexture($"assets/flags/{countries[mRandomId].GetIso()}.png"));
                            isLoaded = true;
                            sprites["Flag"].SetTexture(countries[mRandomId].GetFlag()[0]);
                            sprites["Flag"].SetSize(new Vector2(countries[mRandomId].GetFlag()[0].width / 4, countries[mRandomId].GetFlag()[0].height / 4));
                            sprites["Flag"].SetVisibility(true);
                            sprites["Flag"].SetOrigin(new Vector2((countries[mRandomId].GetFlag()[0].width / 4) / 2, (countries[mRandomId].GetFlag()[0].height / 4) / 2));
                        }
                        texts["Find"].SetText("Find Country :");
                        texts["Answer"].SetText($"{countries[mRandomId].GetCountryName()}");
                    }
                    break;
                default:
                    break;
            }
            if (IsKeyReleased(KeyboardKey.KEY_ENTER))
            {
                inputboxs["input"].SetClicked(true);
                texts["AnswerStatus"].SetTextVisibility(true);
                frameCount = 60;
                // If 75% of the characters are good then it's valid.
                if (IsValid(inputboxs["input"].GetText(), countries[mRandomId].GetCountryName(), 75) && frameCount > 0)
                {
                    texts["AnswerStatus"].SetText("Good result");
                    texts["AnswerStatus"].SetTextColor(GREEN);
                    countries.Remove(countries[mRandomId]);
                    isDeclared = false;
                    isLoaded = false;
                    mPoints++;
                }
                //Wrong result
                else
                {
                    texts["AnswerStatus"].SetText("Wrong result");
                    texts["AnswerStatus"].SetTextColor(RED);
                }

                inputboxs["input"].ClearInput();
            }

            // Number of Countries Text.
            texts["NumberOfCountries"].SetText($"{mPoints}/{size_at_start}");

            // Answer Button.
            if (toggleButtons["Answer"].IsOn())
                texts["Answer"].SetTextVisibility(true);
            else
                texts["Answer"].SetTextVisibility(false);

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
                texts["AnswerStatus"].SetTextVisibility(false);
                frameCount = 0;
            }

        }
        public GameMode GetGameMode() { return mMode; }
        public bool Win() { return mWin; }
    };
}
