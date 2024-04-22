using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Core.Attributes;
using Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Discord
{
	/// <summary>
	/// A collection of the relevant channels associated in for a player's game in a company guild
	/// </summary>
	[RegisterValueConverter(typeof(JsonValueConverter<PlayerChannels>), typeof(JsonValueComparer<PlayerChannels>))]
	[Keyless]
	public class PlayerChannels
	{
		public PlayerChannels() { }


		/// <summary>
		/// The id of the text channel that relates to their public lobby channel. This is also the same channel where the threads are linked to
		/// </summary>
		[JsonProperty("lobby_channel_id")]
		[Column("company_status")]
		public ulong LobbyChannelId { get; set; }


		[JsonProperty("notify_thread")]
		[Column("notify_thread")]
		public ulong NotificationThreadId { get; set; }


		[JsonProperty("adventure_thread")]
		[Column("adventure_thread")]
		public ulong AdventureThreadId { get; set; }


		[JsonProperty("missions_thread")]
		[Column("missions_thread")]
		public ulong MissionsThreadId { get; set; }


		[JsonProperty("home_system_thread")]
		[Column("home_system_thread")]
		public ulong HomeSystemThreadId { get; set; }


		[JsonProperty("colonized_systems_thread")]
		[Column("colonized_systems_thread")]
		public ulong ColonizedSystemsThreadId { get; set; }


		[JsonProperty("outposts_thread")]
		[Column("outposts_thread")]
		public ulong OutpostsThreadId { get; set; }
	}


	/// <summary>
	/// A data representation of a player's complete data that is stored either to files or to a database.
	/// </summary>
	[Table("player_discord_data")]
	public class PlayerDiscordData
	{


		/// <summary>
		/// The Id of the user this object belongs to
		/// </summary>
		[JsonRequired]
		[Required]
		[JsonProperty("user_id")]
		[Column("user_id")]
		[Key]
		public ulong UserId { get; private set; }


		/// <summary>
		///The guild/company this player currently belongs to. If 0, this player does not have a company and cannot play
		/// </summary>

		[JsonProperty("guild_id")]
		[Column("guild_id")]
		public ulong GuildId { get; set; }


		/// <summary>
		/// The number of available company permits that this player has available. Each permit creates a company
		/// </summary>
		[JsonProperty("company_permits")]
		[Column("company_permits")]
		public int CompanyPermits { get; set; } = 1;


		/// <summary>
		/// Represents how much premium currency this user has
		/// </summary>
		[JsonProperty("premium_currency")]
		[Column("premium_currency")]
		public int PremiumCurrency { get; set; } = 0;


		/// <summary>
		/// A data representation of this player's channels
		/// </summary>
		[JsonProperty("channels")]
		[Column("channels", TypeName = "jsonb")]
		public PlayerChannels Channels { get; set; } = new();





		/// <summary>
		/// Creates a new player discord data object
		/// </summary>
		/// <param name="userId">This user's id</param>
		public PlayerDiscordData(ulong userId)
		{
			UserId = userId;
		}
	}
}
