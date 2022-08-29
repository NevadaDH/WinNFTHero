
using System.Collections.Generic;

public partial class Battle_Damage {
    public ushort type = 0;
    
    public ushort target = 0;
    
    public int damage = 0;
    
    public int hp = 0;
    
    public int dun = 0;
    
    public int hpmax = 0;
    
    public Battle_Buff buff = null;
    
}



public partial class Battle_Buff {
    public ushort guid = 0;
    
    public ushort id = 0;
    
    public ushort caster = 0;
    
    public ushort target = 0;
    
    public int time = 0;
    
}



public partial class Battle_Skill {
    public int time = 0;
    
    public ushort killtarget = 0;
    
    public ushort target = 0;
    
    public string id = "";
    
    public ushort pos = 0;
    
    public List<Battle_Damage> damages = null;
    
    public int atb = 0;
    
    public int power = 0;
    
}



public partial class Battle_Round {
    public ushort id = 0;
    
    public List<Battle_Skill> skill = null;
    
}



public partial class Battle_Hero {
    public ushort guid = 0;
    
    public ushort type = 0;
    
    public ushort id = 0;
    
    public string name = "";
    
    public Pro pro = null;
    
    public ushort pos = 0;
    
    public List<Battle_Equip> equips = null;
    
}



public partial class Battle_Equip {
    public uint id = 0;
    
}



public partial class Battle_User {
    public double roleid = 0;
    
    public string name = "";
    
    public List<Battle_Hero> heros = null;
    
    public List<int> bonds = null;
    
    public List<int> bond_actives = null;
    
}



public partial class Battle_Movie {
    public Battle_User A = null;
    
    public Battle_User B = null;
    
    public List<Battle_Round> rounds = null;
    
    public List<Battle_Skill> skill = null;
    
    public ushort result = 0;
    
    public int time = 0;
    
}



