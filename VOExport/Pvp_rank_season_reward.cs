
using System.Collections.Generic;

public class Pvp_rank_season_rewardVO {
    /**
    * 主key  
    */
    public ushort id = 0;
    
    /**
    * 区间上限  
    */
    public ushort top = 0;
    
    /**
    * 区间下限  
    */
    public ushort bottom = 0;
    
    /**
    * 奖励  
    */
    public List<O1LimitVO> reward = null;
    
}



