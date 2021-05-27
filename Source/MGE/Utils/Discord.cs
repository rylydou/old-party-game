using System;
using DiscordRPC;

namespace MGE
{
	public static class Discord
	{
		public static DiscordRpcClient client;

		static string id;

		internal static void Init()
		{
			id = Environment.TickCount64.ToString();

			client = new DiscordRpcClient(Config.discordAppId);

			// client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

			client.OnConnectionEstablished += (sender, e) =>
			{
				Logger.Log($"Discord RPC connected");
			};

			client.OnConnectionFailed += (sender, e) =>
			{
				Logger.Log("Discord RPC disconnected");
				client.Dispose();
			};

			client.OnReady += (sender, e) =>
			{
				Logger.Log($"Discord RPC ready under {e.User.Username}");

				client.SetPresence(new RichPresence()
				{
					Timestamps = new Timestamps(DateTime.UtcNow)
				});
			};

			client.OnClose += (sender, e) =>
			{
				Logger.Log($"Discord RPC closed {e.Code}\n{e.Reason}");
				client.Dispose();
			};

			client.OnError += (sender, e) =>
			{
				Logger.LogError($"Discord RPC Error {e.Code}\n{e.Message}");
				client.Dispose();
			};

			client.Initialize();
		}

		public static void SetPresence(RichPresence presence)
		{
			client.SetPresence(presence);
		}

		public static RichPresence SetDetails(string details)
		{
			if (client.IsInitialized) return client.UpdateDetails(details);
			return null;
		}

		public static RichPresence SetState(string details)
		{
			if (client.IsInitialized) return client.UpdateState(details);
			return null;
		}

		public static RichPresence SetSmallIcon(string key, string tooltip = null)
		{
			if (client.IsInitialized) return client.UpdateSmallAsset(key, tooltip);
			return null;
		}

		public static RichPresence SetLargeIcon(string key, string tooltip = null)
		{
			if (client.IsInitialized) return client.UpdateLargeAsset(key, tooltip);
			return null;
		}

		public static void DeInit()
		{
			client.Dispose();
		}
	}
}