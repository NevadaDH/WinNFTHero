
using System.Collections.Generic;
public partial class MineSocketConst 
{
	public const int pledge = 800;
	public const int takeback = 801;
	public const int get_mine = 802;
	public const int get_all_mine = 803;
}

public partial class MineSocketCode 
{
	public const int CODE_SUCCESS = 200;
	public const int CODE_HERO_STATE = 1000;
	public const int CODE_HERO_ID = 1001;
	public const int CODE_HERO_PLEDGE = 1002;
	public const int CODE_HERO_TAKEBACK = 1003;
	public const int CODE_HERO_CHA = 1004;
	public const int CODE_SPEND_INSUFFICIENT = 2000;
	public const int CODE_SPEND_INSUFFICIENT_MODEL = 2001;
	public const int CODE_MINE_NOTOPEN = 3000;
	public const int CODE_MINE_ID = 3001;
	public const int CODE_MINE_NOTFOUND = 3002;
	public const int CODE_ROLE_ID = 4001;
}


public partial class GS_MINE {
    public uint id = 0;
    
    public double guid = 0;
    
    public uint totalbonus = 0;
    
    public uint miningbonus = 0;
    
    public List<uint> heroIds = null;
    
    public double startTime = 0;
    
    public double endTime = 0;
    
    public uint speed = 0;
    
    public uint status = 0;
    
    public uint roleBonus = 0;
    
}



public partial class mine_pledge_req {
    public ushort heroId = 0;
    
    public ushort pledgeId = 0;
    
}



public partial class mine_pledge_res {
    public float code = 0;
    
    public string message = "";
    
    public GS_MINE mine = null;
    
}



public partial class mine_takeback_req {
    public ushort heroId = 0;
    
    public ushort pledgeId = 0;
    
}



public partial class mine_takeback_res {
    public float code = 0;
    
    public string message = "";
    
    public GS_MINE mine = null;
    
}



public partial class mine_get_mine_req {
    public ushort mineId = 0;
    
}



public partial class mine_get_mine_res {
    public float code = 0;
    
    public string message = "";
    
    public CROSS_MINE mine = null;
    
}



public partial class mine_get_all_mine_req {
    public uint roleId = 0;
    
}



public partial class mine_get_all_mine_res {
    public float code = 0;
    
    public string message = "";
    
    public Dictionary<string, CROSS_MINE_SUMMARY> mines = null;
    
    public Dictionary<string, CROSS_MINE_HERO> heros = null;
    
}



