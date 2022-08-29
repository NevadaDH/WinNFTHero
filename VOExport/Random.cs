
using System.Collections.Generic;

public class RandomVO {
    /**
    * 类型  
    */
    public string id = "";
    
    public List<RandomChildVO> childs = null;
    
}



public class RandomChildVO {
    /**
    * 类型  
    */
    public string id = "";
    
    /**
    * 条件  
    */
    public List<O1LimitVO> limit = null;
    
    /**
    * 条件module  
    */
    public List<CodeFunctionVO> boModule = null;
    
    /**
    * 权值  
    */
    public ushort weight = 0;
    
    /**
    * 奖励  
    */
    public List<O1LimitVO> reward = null;
    
    /**
    * 事件  
    */
    public List<CodeFunctionVO> module = null;
    
    /**
    * 再次随机  
    */
    public List<CodeFunctionVO> fdo = null;
    
    /**
    * 参数  
    */
    public object pro = null;
    
}



