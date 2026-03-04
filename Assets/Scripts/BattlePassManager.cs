using System;
using System.Collections.Generic;
using System.Globalization;
using Beamable;
using Beamable.Common;
using Beamable.Common.Content;
using Beamable.Server.Clients;
using UnityEngine;

namespace BattlePass
{
    public class BattlePassManager : MonoBehaviour
    {
        [SerializeField] private ContentRef<Battlepass> battlepassRef;
        
        private const string BattlePassContentKey = "items.battlepass";
        private Battlepass _battlePass;
        private BeamContext _beamContext;
        private BattlePassServiceClient _service;

        private async void Start()
        {
            // Get the Beamable context
            _beamContext = await BeamContext.Default.Instance;
            Debug.Log("Player gamertag: " + _beamContext.PlayerId);
            _service = _beamContext.Microservices.BattlePassService();

            // Fetch the Battlepass content
            await battlepassRef.Resolve()
                .Then(content =>
                {
                    _battlePass = content;
                    Debug.Log($"Fetched Battlepass: {_battlePass.Name}");
                })
                .Error(ex =>
                {
                    Debug.LogError("Failed to fetch the Battlepass content.");
                });
            
            await DisplayBattlePassDetails();
            
        }

        private async Promise DisplayBattlePassDetails()
        {
            Debug.Log($"Battlepass: {_battlePass.Name}");
            foreach (var tier in _battlePass.Tiers)
            {
                Debug.Log($"Tier {tier.Level}");
                foreach (var reward in tier.Rewards)
                {
                    Debug.Log($"Reward: {reward.RewardName}, Quantity: {reward.Quantity}");
                }
            }
            // Parse and display the end date
            if (DateTime.TryParseExact(_battlePass.EndDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime endDate))
            {
                Debug.Log($"Battlepass End Date: {endDate}");
            }
            else
            {
                Debug.LogWarning("Failed to parse the Battlepass End Date.");
            }

            var isValid = await _service.BattlePassService_IsBattlepassValid(battlepassRef);
            Debug.Log(isValid);
        }
        
        private async Promise AddBattlePassToInventory()
        {
            _beamContext = await BeamContext.Default.Instance;

            var inventory = _beamContext.Inventory;

            // Use Update to add the Battlepass to the player's inventory
            await inventory.Update(builder => builder.AddItem(BattlePassContentKey, new Dictionary<string, string>
            {
                { "name", _battlePass.Name },  // You can add any properties related to the battle pass here
                { "endDate", _battlePass.EndDate }     // Example property for the battle pass duration (in days)
            }));

            Debug.Log("BattlePass added to inventory!");
        }

        public async void GetBattlePassButton()
        {
            await AddBattlePassToInventory();
        }
    }
}