
using System.Collections.Generic;
public partial class EquipSocketConst 
{
	public const int wear = 500;
	public const int remove = 501;
	public const int enhance = 502;
}


public partial class Equip_wear_req {
    public ushort hero = 0;
    
    public ushort equip = 0;
    
}



public partial class Equip_wear_res {
    public string error = "";
    
}



public partial class Equip_remove_req {
    public ushort equip = 0;
    
    public ushort hero = 0;
    
}



public partial class Equip_remove_res {
    public string error = "";
    
}



public partial class Equip_enhance_req {
    public ushort id = 0;
    
}



public partial class Equip_enhance_res {
    public string message = "";
    
}



