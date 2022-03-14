using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Flags_csharp.src.datas.CountriesDatas;

namespace Flags_csharp.src.datas
{
    class Country
    {
        string mCapitale;
        string mCountry;
        string mContinent;
        string mIso;

        List<Texture2D> mFlag = new List<Texture2D>();
        List<Texture2D> mMap = new List<Texture2D>();

        public Country() { }
        public Country(int id) 
        {
            Countries tmp = CountriesDatas.GetCountryById(id);
            mCapitale = tmp.capitale;
            mCountry = tmp.country;
            mContinent = tmp.continent;
            mIso = tmp.flag;
        }
        public Country(int id, Texture2D flag) 
        {
            Countries tmp = GetCountryById(id);
            mCapitale = tmp.capitale;
            mCountry = tmp.country;
            mContinent = tmp.continent;
            mIso = tmp.flag;
            mFlag[0] = flag;
        }
        ~Country() { }

        public string GetCapitale() { return mCapitale; }
        public string GetCountryName() { return mCountry; }
        public string GetContinent() { return mContinent; }
        public string GetIso() { return mIso; }
        public List<Texture2D> GetFlag() { return mFlag; }
        public List<Texture2D> GetMap() { return mMap; }

        public void SetFlag(Texture2D flag)
        {
            mFlag.Clear();
            mFlag.Add(flag);
        }
        public void SetMap(Texture2D map)
        {
            mMap.Clear();
            mMap.Add(map);
        }
    };
}
