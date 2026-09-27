using NUnit.Framework;
using System.Collections.Generic;

public class LevelInfo
{
    public static List<LevelInfo> levels = new List<LevelInfo>();

    public static void CreateLevels()
    {
        if (levels.Count > 0) return;
        levels.Add(new LevelInfo(1, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309, 109, 406 },
            new List<int> { 2, 4, 6, 8, 9, 101, 102, 104, 107, 201, 203, 207, 301, 305, 308, 401, 405 }, new List<int> { 210 }));
        levels.Add(new LevelInfo(2, 212, 100, 50, new List<int> { 3, 5, 8, 106, 208, 304, 309 },
            new List<int> { 2, 4, 6, 7, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 301, 302, 305, 308, 310, 401, 402, 405 }, new List<int> { 211 }));
        levels.Add(new LevelInfo(3, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 210 }));
        levels.Add(new LevelInfo(4, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309, 311 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 211 }));
        levels.Add(new LevelInfo(5, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 210 }));
        levels.Add(new LevelInfo(6, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309, 404 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 211 }));
        levels.Add(new LevelInfo(7, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309, 203 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 210 }));
        levels.Add(new LevelInfo(8, 212, 100, 50, new List<int> { 3, 8, 106, 108, 208, 304, 309 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 211 }));
        levels.Add(new LevelInfo(9, 212, 100, 50, new List<int> { 3, 8, 106, 208, 304, 309 },
            new List<int> { 2, 3, 4, 5, 6, 7, 9, 101, 102, 103, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 210, 8 }));
        levels.Add(new LevelInfo(10, 212, 100, 50, new List<int> { 3, 8, 106, 102, 208, 304, 309 },
            new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 104, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 211, 307, 103 }));
        levels.Add(new LevelInfo(11, 212, 100, 50, new List<int> { 3, 8, 106, 208, 102, 304, 309, 404, 204 },
            new List<int> { 2, 3, 4, 5, 7, 8, 9, 101, 102, 103, 105, 201, 202, 203, 207, 209, 301, 302, 305, 308, 401, 402, 405 }, new List<int> { 210, 304, 6, 104 }));
    }
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
