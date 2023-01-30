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
                    if (mainMenu.UiComponents["play"].IsClicked())
                        SwitchTo(SceneTypes.IN_GAME);
                    if (mainMenu.UiComponents["settings"].IsClicked())
                        SwitchTo(SceneTypes.SETTINGS_MENU);
                    if (mainMenu.UiComponents["quit"].IsClicked())
                        System.Environment.Exit(0);
                    break;

                case SceneTypes.IN_GAME:
                    inGame.Update();
                    if (inGame.Win() || (inGame.GetGameMode() == GameMode.NONE && inGame.UiComponents["back"].IsClicked()))
                    {
                        SwitchTo(SceneTypes.MAIN_MENU);
                        inGame = new InGame(ressource);
                    }
                    if (inGame.UiComponents["Pause"].IsClicked())
                        SwitchTo(SceneTypes.PAUSE_MENU);
                    break;

                case SceneTypes.SETTINGS_MENU:
                    SettingsMenu.Update();
                    if (SettingsMenu.UiComponents["mainMenu"].IsClicked() && SettingsMenu.UiComponents["mainMenu"].Visible)
                        SwitchTo(SceneTypes.MAIN_MENU);
                    break;

                case SceneTypes.PAUSE_MENU:
                    pauseMenu.Update();
                    if (pauseMenu.UiComponents["Reprendre"].IsClicked())
                        mType = SceneTypes.IN_GAME;
                    if (pauseMenu.UiComponents["Quitter"].IsClicked())
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
