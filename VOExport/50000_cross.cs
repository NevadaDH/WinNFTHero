
using System.Collections.Generic;
public partial class CROSS_GAMESERVER 
{
	public const int BATTLE_DATA = 50010;
	public const int PVPRANK_RANK_CHANGE = 50020;
}

public partial class CROSS_PVPRANK 
{
	public const int RANK = 50100;
	public const int REFRESH_MATCH = 50101;
	public const int MATCH_ROLES = 50102;
	public const int FIGHT_START = 50103;
	public const int GET_FIGHT_MOVIE = 50104;
}

public partial class CROSS_BATTLE 
{
	public const int BATTLE = 50200;
	public const int GET_BATTLE_MOVIE = 50210;
}

public partial class FIGHT_START 
{
	public const int CODE_TARGET_BUSY = -3;
	public const int CODE_DEFENDING = -2;
	public const int CODE_SYNC = -1;
}

public partial class CROSS_MINE_PROTO 
{
	public const int GET_MINE = 50300;
	public const int PLEDGE = 50301;
	public const int TAKE_OUT = 50302;
	public const int REWARD = 50303;
	public const int GET_ALL_MINE = 50304;
	public const int HERO_CHA_CHANGE = 50305;
	public const int GET_HERO_CHA = 50306;
}

public partial class Mine_Type 
{
	public const int Cycle = 1;
	public const int Once = 2;
}

public partial class Mine_Status 
{
	public const int Prepare = 0;
	public const int Mining = 1;
	public const int Finish = 2;
	public const int FinishReward = 3;
}


public partial class CROSS_BATTLE_HERO {
    public uint id = 0;
    
    public uint lv = 0;
    
    public uint pos = 0;
    
}



public partial class CROSS_BATTLE_EQUIP {
    public uint id = 0;
    
    public uint heroId = 0;
    
}



public partial class CROSS_BATTLE_REQ {
    public CROSS_BATTLE_TEAM A = null;
    
    public CROSS_BATTLE_TEAM B = null;
    
    public bool save = false;
    
}



public partial class CROSS_BATTLE_MOVIE {
    public string guid = "";
    
    public ushort result = 0;
    
    public foundation.ByteArray movie = null;
    
    public double time = 0;
    
    public uint duration = 0;
    
}



public partial class CROSS_PVPRANK_DATA {
    public string roleId = "";
    
    public uint rankId = 0;
    
    public bool isRobot = false;
    
}



public partial class CROSS_BATTLE_TEAM {
    public string roleId = "";
    
    public uint rankId = 0;
    
    public string name = "";
    
    public uint face = 0;
    
    public uint fv = 0;
    
    public bool robot = false;
    
    public Dictionary<string, CROSS_BATTLE_HERO> heros = null;
    
    public Dictionary<string, CROSS_BATTLE_EQUIP> equips = null;
    
}



public partial class BATTLE_DATA_REQ {
    public double roleId = 0;
    
    public uint type = 0;
    
}



public partial class CROSS_PVPRANK_RANK_REQ {
    public double roleId = 0;
    
    public uint seq = 0;
    
    public uint limit = 0;
    
}



public partial class CROSS_PVPRANK_GET_FIGHT_MOVIE_REQ {
    public double mvId = 0;
    
}



public partial class CROSS_PVPRANK_USERINFO {
    public double roleId = 0;
    
    public string name = "";
    
    public uint face = 0;
    
    public uint fv = 0;
    
}



public partial class CROSS_PVPRANK_RANK_CHANGE {
    public double roleId = 0;
    
    public uint rankId = 0;
    
    public double mvId = 0;
    
    public bool win = false;
    
    public bool attacker = false;
    
    public CROSS_PVPRANK_USERINFO target = null;
    
    public double time = 0;
    
}



public partial class CROSS_PVPRANK_RANK_ACK {
    public uint rankId = 0;
    
    public List<CROSS_BATTLE_TEAM> ranks = null;
    
}



public partial class CROSS_PVPRANK_REFRESH_MATCH_REQ {
    public double roleId = 0;
    
}



public partial class CROSS_PVPRANK_REFRESH_MATCH_ACK {
    public List<CROSS_BATTLE_TEAM> matchRanks = null;
    
}



public partial class CROSS_PVPRANK_MATCH_ROLES_REQ {
    public double roleId = 0;
    
    public List<uint> matchRankIds = null;
    
}



public partial class CROSS_PVPRANK_MATCH_ROLES_ACK {
    public uint rankId = 0;
    
    public List<CROSS_BATTLE_TEAM> matchRanks = null;
    
}



public partial class CROSS_PVPRANK_FIGHT_START_REQ {
    public CROSS_BATTLE_TEAM roleTeam = null;
    
    public double targetRoleId = 0;
    
}



public partial class CROSS_PVPRANK_FIGHT_START_ACK {
    public double code = 0;
    
    public CROSS_BATTLE_MOVIE mv = null;
    
}



public partial class CROSS_MINE_HERO {
    public uint id = 0;
    
    public uint mineId = 0;
    
    public uint bonus = 0;
    
    public uint status = 0;
    
    public uint speed = 0;
    
}



public partial class CROSS_MINE_ROLE {
    public Dictionary<string, CROSS_MINE_HERO> heros = null;
    
    public double roleBonus = 0;
    
}



public partial class CROSS_MINE {
    public uint id = 0;
    
    public double guid = 0;
    
    public uint totalbonus = 0;
    
    public uint miningbonus = 0;
    
    public Dictionary<string, CROSS_MINE_ROLE> roles = null;
    
    public double startTime = 0;
    
    public double endTime = 0;
    
    public uint speed = 0;
    
    public uint status = 0;
    
}



public partial class CROSS_MINE_SUMMARY {
    public uint id = 0;
    
    public double guid = 0;
    
    public uint totalbonus = 0;
    
    public uint miningbonus = 0;
    
    public uint speed = 0;
    
    public uint status = 0;
    
}



public partial class CROSS_MINE_ROLE_MINES {
    public uint mineId = 0;
    
    public uint miningbonus = 0;
    
    public Dictionary<string, CROSS_MINE_HERO> heros = null;
    
}



public partial class CROSS_MINE_GET_ALL_MINE_REQ {
    public uint roleId = 0;
    
}



public partial class CROSS_MINE_GET_ALL_MINE_ACK {
    public Dictionary<string, CROSS_MINE_SUMMARY> mines = null;
    
    public Dictionary<string, CROSS_MINE_HERO> heros = null;
    
}



public partial class CROSS_MINE_GET_MINE_REQ {
    public uint mineId = 0;
    
}



public partial class CROSS_MINE_GET_MINE_ACK {
    public CROSS_MINE mine = null;
    
}



public partial class CROSS_MINE_PLEDGE_REQ {
    public uint mineId = 0;
    
    public double roleId = 0;
    
    public uint heroId = 0;
    
    public uint cha = 0;
    
}



public partial class CROSS_MINE_PLEDGE_ACK {
    public CROSS_MINE mine = null;
    
}



public partial class CROSS_MINE_TAKE_OUT_REQ {
    public uint mineId = 0;
    
    public double roleId = 0;
    
    public uint heroId = 0;
    
}



public partial class CROSS_MINE_TAKE_OUT_ACK {
    public CROSS_MINE mine = null;
    
}



public partial class CROSS_MINE_REWARD_REQ {
    public uint id = 0;
    
    public uint mineId = 0;
    
    public double roleId = 0;
    
    public Dictionary<string, CROSS_MINE_HERO> heros = null;
    
}



public partial class CROSS_MINE_REWARD_ACK {

}



public partial class CROSS_MINE_HERO_CHA_CHANGE_REQ {
    public uint mineId = 0;
    
    public uint roleId = 0;
    
    public uint heroId = 0;
    
    public uint cha = 0;
    
}



public partial class CROSS_MINE_HERO_CHA_CHANGE_ACK {
    public uint code = 0;
    
}



public partial class CROSS_MINE_GET_HERO_CHA_REQ {
    public uint roleId = 0;
    
}



public partial class CROSS_MINE_HERO_PRO {
    public uint id = 0;
    
    public uint cha = 0;
    
    public uint status = 0;
    
}



public partial class CROSS_MINE_GET_HERO_CHA_ACK {
    public List<CROSS_MINE_HERO_PRO> heros = null;
    
}



