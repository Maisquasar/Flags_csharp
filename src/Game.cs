using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Flags_csharp.src.datas;
using Flags_csharp.src.renders;

namespace Flags_csharp.src
{
    
    class Game
    {
        int mPoints = 0;
        public SceneManager manager;
        public List<Country> countries = new List<Country>();
        public List<Country> countriesAfrique = new List<Country>();
        public List<Country> countriesEurope = new List<Country>();
        public List<Country> countriesAsie = new List<Country>();
        public List<Country> countriesOceanie = new List<Country>();
        public List<Country> countriesAmerique = new List<Country>();

        public Game() 
        {
            manager = new SceneManager();
        }
        ~Game() { }

        public void Draw()
        {
            manager.Draw();
        }
        public void Update()
        {
            manager.Update();
        }
        public static bool InferiorOrEqual(Vector2 a, Vector2 b)
        {
            if (a.X <= b.X && a.Y <= b.Y)
                return true;
            else
                return false;
        }
        public static bool SuperiorOrEqual(Vector2 a, Vector2 b)
        {
            if (a.X >= b.X && a.Y >= b.Y)
                return true;
            else
                return false;
        }
    };

}
