using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Flags_csharp.src.renders
{
    class Ressource
    {
        public List<Texture2D> flags;
        public Dictionary<string,Texture2D> UITextures = new Dictionary<string, Texture2D>();
        public Ressource()
        {
            UITextures["background"] = LoadTexture($"assets/Ui/ButtonDefault.png");
            UITextures["button"] = LoadTexture($"assets/Ui/ButtonActiveRounded.png");
        }
        ~Ressource() { }
    };
}
