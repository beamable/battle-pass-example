using System.Collections.Generic;
using BattlePass;
using Beamable.Common.Content;

namespace Beamable.Common
{
    [ContentType("battlepass")]
    public class Battlepass : ContentObject
    {
        public string Name;
        public string EndDate; 
        public List<Tier> Tiers;
    }
}