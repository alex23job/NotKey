using NUnit.Framework;
using System.Collections.Generic;

public class LevelInfo
{
    public int Number { get; set; }
    public int FinishKey { get; set; }
    public int LevelExp { get; set; }
    public int LevelMany { get; set; }

    public List<int> BonusArr = new List<int>();
    public List<int> EffectArr = new List<int>();
    public List<int> MonstrArr = new List<int>();

    public LevelInfo() { }
    public LevelInfo(int num, int finish, int exp, int many, List<int> bonus, List<int> eff, List<int> monstr = null)
    {
        Number = num;
        FinishKey = finish;
        LevelExp = exp;
        LevelMany = many;
        int i;
        if (bonus.Count > 0) { for (i = 0; i < bonus.Count; i++) BonusArr.Add(bonus[i]); }
        if (eff.Count > 0) { for (i = 0; i < eff.Count; i++) EffectArr.Add(eff[i]); }
        if (monstr != null && monstr.Count > 0) { for (i = 0; i < monstr.Count; i++) MonstrArr.Add(monstr[i]); }
    }
}
