using System;
using System.Globalization;
using Beamable.Common;
using Beamable.Common.Content;
using Beamable.Server;

namespace Beamable.BattlePassService
{
	public partial class BattlePassService : Microservice
	{
		[ClientCallable]
		public async Promise<bool> IsBattlepassValid(ContentRef<Battlepass> battlePassContent)
		{
			// Retrieve the BattlePass content by its ID
			var battlePass = await Services.Content.GetContent(battlePassContent);

			if (battlePass == null)
			{
				throw new Exception($"BattlePass with ID {battlePassContent.GetId()} not found");
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
