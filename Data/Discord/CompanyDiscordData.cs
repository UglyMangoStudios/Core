using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SpaceCore.Attributes;
using SpaceCore.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceCore.Data.Discord
{
	/// <summary>
	/// Simple enumeration that describes the company's guild's current status
	/// </summary>
	public enum CompanyStatus
	{
		IncompleteAndNotEstablished,
		CompleteAndNotEstablished,

		IncompleteAndEstablished,
		CompleteAndEstablished
	}

	/// <summary>
	/// A collection of the relevant channels associated in a company
	/// </summary>
	[RegisterValueConverter(typeof(JsonValueConverter<CompanyChannels>), typeof(JsonValueComparer<CompanyChannels>))]
	[Keyless]
	public class CompanyChannels()
	{

		[JsonProperty("company_center_category_id")]
		[Column("company_center_category_id")]
		public ulong CompanyCenterCategory { get; set; } = 0;


		[JsonProperty("player_games_category")]
		[Column("company_center_category_id")]
		public ulong PlayerGamesCategory { get; set; } = 0;


		[JsonProperty("announce_channel_id")]
		[Column("announce_channel_id")]
		public ulong AnnouncementChannelId { get; set; } = 0;


		[JsonProperty("company_console_channel_id")]
		[Column("company_console_channel_id")]
		public ulong CompanyConsoleChannelId { get; set; } = 0;


		[JsonProperty("admin_console_channel_id")]
		[Column("admin_console_channel_id")]
		public ulong AdminConsoleChannelId { get; set; } = 0;


		[JsonProperty("company_data_channel_id")]
		[Column("company_data_channel_id")]
		public ulong CompanyDataChannelId { get; set; } = 0;	
	}

	/// <summary>
	/// Data that is stored within the NoSQL database. This class is the representation of Discord-related data for each company
	/// (e.g.each Discord guild). 
	/// <br/><br/>
	/// Guild roles and channels as well as company name, description, etc. are stored here.All members are public.
	/// </summary>
	[Table("company_discord_data")]
	public class CompanyDiscordData
	{
		[JsonProperty("guild_id")]
		[Column("guild_id")]
		[JsonRequired]
		[Required]
		[Key]
		public ulong GuildId { get; private set; }


		[JsonProperty("company_status")]
		[Column("company_status")]
		public CompanyStatus CompanyStatus { get; set; } = 0;


		[JsonProperty("name")]
		[Column("name")]
		public string Name { get; set; } = "";


		[JsonProperty("shorthand")]
		[Column("shorthand")]
		public string Shorthand { get; set; } = "";


		[JsonProperty("description")]
		[Column("description")]
		public string Description { get; set; } = "";


		[JsonProperty("channels")]
		[Column("channels", TypeName = "jsonb")]
		public CompanyChannels Channels { get; set; } = new();


		[JsonProperty("reg_players")]
		[Column("reg_players")]
		public List<ulong> RegisteredPlayers { get; set; } = new();


		[JsonProperty("reg_admins")]
		[Column("reg_admins")]
		public List<ulong> RegisteredAdmins { get; set; } = new();


		[JsonProperty("executive_id")]
		[Column("executive_id")]
		public ulong ExecutiveId { get; set; } = 0;


		[JsonProperty("owner_id")]
		[Column("owner_id")]
		public ulong OwnerId { get; set; } = 0;


		public CompanyDiscordData(ulong guildId)
		{
			GuildId = guildId;
		}


		public bool IsEstablished() =>
			CompanyStatus == CompanyStatus.IncompleteAndEstablished || 
			CompanyStatus == CompanyStatus.CompleteAndEstablished;

		public bool IsComplete() =>
			CompanyStatus == CompanyStatus.CompleteAndNotEstablished ||
			CompanyStatus == CompanyStatus.CompleteAndEstablished;
		
	}
}
