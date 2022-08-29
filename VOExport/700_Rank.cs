
using System.Collections.Generic;
public partial class RankSocketConst 
{
	public const int ranking = 700;
	public const int match_list = 701;
	public const int match_refresh = 702;
	public const int playback = 705;
	public const int formation = 706;
	public const int battle = 708;
}


public partial class rank_ranking_req {
    public ushort page = 0;
    
    public ushort num = 0;
    
}



public partial class rank_ranking_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
    public List<CROSS_BATTLE_TEAM> role_list = null;
    
}



public partial class rank_match_list_req {

}



public partial class rank_match_list_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
    public uint rank = 0;
    
    public List<CROSS_BATTLE_TEAM> role_list = null;
    
}



public partial class rank_match_refresh_req {

}



public partial class rank_match_refresh_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
    public List<CROSS_BATTLE_TEAM> role_list = null;
    
}



public partial class rank_record_list_req {

}



public partial class rank_record_list_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
    public List<RankRecordItem> record_list = null;
    
}



public partial class rank_playback_req {
    public string id = "";
    
}



public partial class rank_playback_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
    public CROSS_BATTLE_MOVIE mv = null;
    
}



public partial class rank_formation_req {
    public List<Hero_put_req> heros = null;
    
}



public partial class rank_formation_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
}



public partial class rank_battle_req {
    public List<Hero_put_req> heros = null;
    
    public string targetRoleId = "";
    
}



public partial class rank_battle_rsp {
    public ushort result = 0;
    
    public string reason = "";
    
    public CROSS_BATTLE_MOVIE mv = null;
    
}



