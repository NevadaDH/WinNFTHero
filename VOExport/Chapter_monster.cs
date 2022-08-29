
using System.Collections.Generic;

public class Chapter_monsterVO {
    /**
    * ID  
    */
    public string id = "";
    
    public List<Chapter_monsterChildVO> childs = null;
    
}



public class Chapter_monsterChildVO {
    /**
    * ID  
    */
    public string id = "";
    
    /**
    * 怪物  
    */
    public ushort monster = 0;
    
    /**
    * 怪物等级  
    */
    public ushort level = 0;
    
    /**
    * 站位  
    */
    public ushort pos = 0;
    
    /**
    * 属性系数  
    */
    public Pro pro = null;
    
}



