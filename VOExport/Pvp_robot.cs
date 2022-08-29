
using System.Collections.Generic;

public partial class Pvp_robotVO {
    /**
    * 角色ID  
    */
    public int id = 0;
    
    /**
    *  角色名  
    */
    public string name = "";
    
    /**
    * 头像  
    */
    public string face = "";
    
    /**
    * 排名  
    */
    public uint rank = 0;
    
    public List<Pvp_robotChildVO> childs = null;
    
}



public partial class Pvp_robotChildVO {
    /**
    * 角色ID  
    */
    public int id = 0;
    
    /**
    * 上阵英雄ID  
    */
    public uint hero = 0;
    
    /**
    * 英雄等级  
    */
    public ushort hero_lv = 0;
    
    /**
    * 英雄pve章节站位  
    */
    public ushort hero_pos_chapter = 0;
    
    /**
    * 英雄pvp排位赛站位  
    */
    public ushort hero_pos_rank = 0;
    
    /**
    * 英雄pvp匹配赛站位  
    */
    public ushort hero_pos_match = 0;
    
    /**
    * 英雄装备  
    */
    public List<int> hero_equip = null;
    
}



