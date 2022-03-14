using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flags_csharp.src.renders.scenes;

namespace Flags_csharp.src.renders
{
    enum SceneTypes
    {
        MAIN_MENU,
        IN_GAME,
        PAUSE_MENU,
        SETTINGS_MENU,
    };

    class SceneManager
    {
    SceneTypes mType;
        public Ressource ressource = new Ressource();
        public MainMenu mainMenu;
        public InGame inGame;
        public Pause pauseMenu;
        public Settings SettingsMenu;

        public SceneManager()
        {
            mainMenu = new MainMenu(ressource);
            inGame = new InGame(ressource);
            pauseMenu = new Pause(ressource);
            SettingsMenu = new Settings(ressource);
        }
        ~SceneManager() { }
        public void SwitchTo(SceneTypes type) 
        {
            if (type == SceneTypes.IN_GAME)
                inGame = new InGame(ressource);
            else if (type == SceneTypes.SETTINGS_MENU)
                SettingsMenu = new Settings(ressource);
            mType = type;
        }
        public void Update() 
        {
            switch (mType)
            {
                case SceneTypes.MAIN_MENU:
                    mainMenu.Update();
                    if (mainMenu.texturedButtons["play"].IsClicked())
                        SwitchTo(SceneTypes.IN_GAME);
                    if (mainMenu.texturedButtons["settings"].IsClicked())
                        SwitchTo(SceneTypes.SETTINGS_MENU);
                    if (mainMenu.texturedButtons["quit"].IsClicked())
                        System.Environment.Exit(0);
                    break;

                case SceneTypes.IN_GAME:
                    inGame.Update();
                    if (inGame.Win() || (inGame.GetGameMode() == GameMode.NONE && inGame.buttons["back"].IsClicked()))
                    {
                        SwitchTo(SceneTypes.MAIN_MENU);
                        inGame = new InGame(ressource);
                    }
                    if (inGame.buttons["Pause"].IsClicked())
                        SwitchTo(SceneTypes.PAUSE_MENU);
                    break;

                case SceneTypes.SETTINGS_MENU:
                    SettingsMenu.Update();
                    if (SettingsMenu.buttons["mainMenu"].IsClicked() && SettingsMenu.buttons["mainMenu"].GetVisibility())
                        SwitchTo(SceneTypes.MAIN_MENU);
                    break;

                case SceneTypes.PAUSE_MENU:
                    pauseMenu.Update();
                    if (pauseMenu.textButtons["Reprendre"].IsClicked())
                        mType = SceneTypes.IN_GAME;
                    if (pauseMenu.textButtons["Quitter"].IsClicked())
                        SwitchTo(SceneTypes.MAIN_MENU);
                    break;
                default:
                    break;
            }
        }
        public void Draw() 
        {
            if (mType == SceneTypes.MAIN_MENU)
                mainMenu.Draw();
            if (mType == SceneTypes.IN_GAME)
                inGame.Draw();
            if (mType == SceneTypes.PAUSE_MENU)
                pauseMenu.Draw();
            if (mType == SceneTypes.SETTINGS_MENU)
                SettingsMenu.Draw();
        }

        public SceneTypes GetSceneType() { return mType; }
    };
}
