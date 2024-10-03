using System.Collections.Generic;
using Beamable.Common.Content;

[ContentType("battlepass")]
    public class Battlepass : ContentObject
    {
        public string Name;
        public string EndDate; 
        public List<Tier> Tiers;
    }