using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Flags_csharp.src.renders
{
    class Window
    {
        static public Vector2 GetWindowSize()
        {
            return new Vector2(GetScreenWidth(), GetScreenHeight());
        }
    }
}
