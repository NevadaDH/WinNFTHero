
using System.Collections.Generic;

public class Chapter_uiVO {
    /**
    * 章节  
    */
    public ushort id = 0;
    
    /**
    * 名称  
    */
    public string name = "";
    
    /**
    * 名称面板  
    */
    public string nameboard = "";
    
    /**
    * 背景图  
    */
    public string background = "";
    
    /**
    * 章解锁条件  
    */
    public List<O1LimitVO> condition = null;
    
    public List<Chapter_uiChildVO> childs = null;
    
}



public class Chapter_uiChildVO {
    /**
    * 章节  
    */
    public ushort id = 0;
    
    /**
    * 类型  
    */
    public ushort type = 0;
    
    /**
    * 关卡配置  
    */
    public ushort chapter = 0;
    
}



