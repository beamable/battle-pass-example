using Beamable.Server;
using System.Threading.Tasks;

namespace Beamable.BattlePassService
{
	public class Program
	{
		/// <summary>
		/// The entry point for the <see cref="BattlePassService"/> service.
		/// </summary>
		public static async Task Main()
		{
			await BeamServer
				.Create()
				.IncludeRoutes<BattlePassService>(routePrefix: "")
				.RunForever();
		}
	}
}
