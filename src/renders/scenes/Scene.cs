using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flags_csharp.src.datas;

namespace Flags_csharp.src.renders.scenes
{
    class Scene
    {
        protected bool mIsActive;

        public Scene() { }
        ~Scene() { }
        // --------------------- Ui elements --------------------- //
        public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public Dictionary<string, Text> texts = new Dictionary<string, Text>();
        public Dictionary<string, Button> buttons = new Dictionary<string, Button>();
        public Dictionary<string, ToggleButton> toggleButtons = new Dictionary<string, ToggleButton>();
        public Dictionary<string, TextButton> textButtons = new Dictionary<string, TextButton>();
        public Dictionary<string, InputBox> inputboxs = new Dictionary<string, InputBox>();
        public Dictionary<string, TexturedButton> texturedButtons = new Dictionary<string, TexturedButton>();
        public List<Country> countries = new List<Country>();
        public List<Country> countriesAfrique = new List<Country>();
        public List<Country> countriesEurope = new List<Country>();
        public List<Country> countriesAsie = new List<Country>();
        public List<Country> countriesOceanie = new List<Country>();
        public List<Country> countriesAmerique = new List<Country>();

        public void Update()
        {

        }
        public void Draw()
        {

        }

        public void SetActivity(bool activity)
        {
            mIsActive = activity;
        }
        public bool IsActive()
        {
            return mIsActive;
        }

    };
}
