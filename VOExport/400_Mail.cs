
using System.Collections.Generic;
public partial class MailSocketConst 
{
	public const int read = 400;
	public const int read_all = 401;
	public const int mail_delete = 410;
	public const int mail_delete_all = 411;
	public const int new_mail = 412;
	public const int reward_get = 413;
	public const int reward_get_all = 415;
}


public partial class mail_reward_get_req {
    public string mailId = "";
    
}



public partial class mail_reward_get_res {
    public string result = "";
    
}



public partial class mail_reward_all_res {
    public string result = "";
    
}



