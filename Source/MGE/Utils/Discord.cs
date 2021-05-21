using System;
using DiscordRPC;
using DiscordRPC.Logging;

namespace MGE
{
	public static class Discord
	{
		public static DiscordRpcClient client;

		static string id;

		public static void Init()
		{
			id = Environment.TickCount64.ToString();

			client = new DiscordRpcClient("845088612718739457");

			client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

			client.OnReady += (sender, e) =>
			{
				Logger.Log($"RPC connected under {e.User.Username}");
			};

			client.OnPresenceUpdate += (sender, e) =>
			{
				Logger.Log($"RPC presence updated to {e.Presence}");
			};

			client.Initialize();

			client.SetPresence(new RichPresence()
			{
				Timestamps = new Timestamps(DateTime.UtcNow)
			});
		}

		public static void SetPresence(RichPresence presence)
		{
			client.SetPresence(presence);
		}

		public static RichPresence SetDetails(string details) => client.UpdateDetails(details);
		public static RichPresence SetState(string details) => client.UpdateState(details);
		public static RichPresence SetSmallIcon(string key, string tooltip = null) => client.UpdateSmallAsset(key, tooltip);
		public static RichPresence SetLargeIcon(string key, string tooltip = null) => client.UpdateLargeAsset(key, tooltip);

		public static void Update()
		{
			// client.Invoke();
		}

		public static void DeInit()
		{
			client.Dispose();
		}
	}
}