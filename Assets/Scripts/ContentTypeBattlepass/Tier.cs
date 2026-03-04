using System;
using System.Collections.Generic;

namespace BattlePass
{
    [Serializable]
    public class Tier
    {
        public int Level;
        public List<Reward> Rewards;
    }
}
