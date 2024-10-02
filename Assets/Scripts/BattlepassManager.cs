using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common.Content;
using Beamable.Server.Clients;
using UnityEngine;

namespace DefaultNamespace
{
    public class BattlepassManager : MonoBehaviour
    {
        [SerializeField] private ContentRef<Battlepass> _battlepassRef;
        private Battlepass _battlepass;
        private BeamContext _beamContext;
        private ServiceClient _service;

        private async void Start()
        {
            // Get the Beamable context
            _beamContext = await BeamContext.Default.Instance;
            Debug.Log(_beamContext.PlayerId);
            _service = new ServiceClient();

            // Fetch the Battlepass content
            await _battlepassRef.Resolve()
                .Then(content =>
                {
                    _battlepass = content;
                    Debug.Log($"Fetched Battlepass: {_battlepass.Name}");
                    DisplayBattlepassDetails();
                })
                .Error(ex =>
                {
                    Debug.LogError("Failed to fetch the Battlepass content.");
                });
        }

        private async void DisplayBattlepassDetails()
        {
            Debug.Log($"Battlepass: {_battlepass.Name}");
            foreach (var tier in _battlepass.Tiers)
            {
                Debug.Log($"Tier {tier.Level}");
                foreach (var reward in tier.Rewards)
                {
                    Debug.Log($"Reward: {reward.RewardName}, Quantity: {reward.Quantity}");
                }
            }
            // Parse and display the end date
            if (DateTime.TryParseExact(_battlepass.EndDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime endDate))
            {
                Debug.Log($"Battlepass End Date: {endDate}");
            }
            else
            {
                Debug.LogWarning("Failed to parse the Battlepass End Date.");
            }

            var isValid = await _service.IsBattlepassValid(_battlepassRef);
            Debug.Log(isValid);
        }
        
        private async Task AddBattlepassToInventory()
        {
            _beamContext = await BeamContext.Default.Instance;

            var inventory = _beamContext.Inventory;

            // Use Update to add the Battlepass to the player's inventory
            await inventory.Update(builder => builder.AddItem("items.battlepass", new Dictionary<string, string>
            {
                { "name", _battlepass.Name },  // You can add any properties related to the battle pass here
                { "endDate", _battlepass.EndDate }     // Example property for the battle pass duration (in days)
            }));

            Debug.Log("Battlepass added to inventory!");
        }

        public async void GetBattlepassButton()
        {
            await AddBattlepassToInventory();
        }
    }
}