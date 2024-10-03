using System;
using System.Globalization;
using System.Threading.Tasks;
using Beamable.Common.Content;
using Beamable.Server;
using UnityEngine;

namespace Beamable.Microservices
{
    [Microservice("Service")]
    public class Service : Microservice
    {
        [ClientCallable]
        public async Task<bool> IsBattlepassValid(ContentRef<Battlepass> battlepass)
        {
            // Retrieve the BattlePass content by its ID
            var battlePass = await Services.Content.GetContent(battlepass);

            if (battlePass == null)
            {
                throw new Exception($"BattlePass with ID {battlepass.GetId()} not found");
            }

            // Get the current time in Unix timestamp
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Parse the EndDate string into a DateTimeOffset object
            if (DateTimeOffset.TryParseExact(battlePass.EndDate, "yyyy-MM-ddTHH:mm:ssZ", 
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var endDate))
            {
                // Convert the EndDate to Unix timestamp
                long endDateUnix = endDate.ToUnixTimeSeconds();
                
                // Now you can compare the Unix timestamps
                return currentTime <= endDateUnix;
            }
            else
            {
                throw new Exception("EndDate is not in a valid ISO 8601 format.");
            }
        }
    }
}