
using System.Collections.Generic;

public class Effect_buffVO {
    /**
    * ID  
    */
    public string id = "";
    
    /**
    * 条件  
    */
    public List<O1LimitVO> condition = null;
    
    /**
    * 奖励  
    */
    public List<O1LimitVO> reward = null;
    
    /**
    * 事件  
    */
    public List<CodeFunctionVO> module = null;
    
    /**
    * 属性  
    */
    public object pro = null;
    
    /**
    * buff属性  
    */
    public object probuff = null;
    
}



