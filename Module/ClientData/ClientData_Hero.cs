public partial class HeroItem
{
    public int GetPos(Mao.FormationMode mode)
    {
         if (mode == Mao.FormationMode.PVE)
            {
              return pos;
            }
            else if (mode == Mao.FormationMode.PVP_Rank_Defend)
            {
               return pos_rank_def;
            }
            else if (mode == Mao.FormationMode.PVP_Rank_Attack)
            {
                return pos_rank_atk;
            }
        return 0;
    }
}