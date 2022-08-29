
using System.Collections.Generic;
public partial class HeroSocketConst 
{
	public const int hero_get = 200;
	public const int hero_put = 210;
	public const int Hero_put_all = 211;
	public const int add_exp_req = 212;
	public const int add_exp_res = 213;
	public const int bond_req = 220;
	public const int add_baptize = 230;
}

public partial class HeroStateConst 
{
	public const int state_idel = 0;
	public const int state_mine = 1;
	public const int state_battle = 2;
}


public partial class Hero_get_req {
    public ushort id = 0;
    
}



public partial class Hero_get_ack {
    public string error = "";
    
    public ushort id = 0;
    
}



public partial class Hero_put_req {
    public ushort id = 0;
    
    public short pos = 0;
    
}



public partial class Hero_put_ack {
    public string error = "";
    
}



public partial class Hero_add_exp_req {
    public ushort id = 0;
    
    public ushort lv = 0;
    
}



public partial class Hero_add_exp_res {
    public uint exp = 0;
    
    public bool critical = false;
    
    public ushort lv = 0;
    
}



public partial class Hero_add_baptize_req {
    public ushort id = 0;
    
}



public partial class Hero_add_baptize_res {
    public ushort id = 0;
    
    public string message = "";
    
}



public partial class Bond_req {
    public List<uint> hero = null;
    
}



public partial class Bond_ack {
    public List<int> bonds = null;
    
    public List<int> bond_actives = null;
    
}



