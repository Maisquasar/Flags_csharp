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
        public Dictionary<string, UIComponent> UiComponents = new Dictionary<string, UIComponent>();

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
