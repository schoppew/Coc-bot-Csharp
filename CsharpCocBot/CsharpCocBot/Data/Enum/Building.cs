namespace CoC.Bot.Data
{
	using System.ComponentModel;

	/// <summary>
	/// The Building.
	/// </summary>
	public enum Building
	{
		[Description("Town Hall")]
		TownHall = 1,

		[Description("Clan Castle")]
		ClanCastle = 2,

		Laboratory = 3,
		[Description("Spell Factory")]
		SpellFactory = 4,

		[Description("Army Camp 1")]
		Camp1 = 5,
		[Description("Army Camp 2")]
		Camp2 = 6,
		[Description("Army Camp 3")]
		Camp3 = 7,
		[Description("Army Camp 4")]
		Camp4 = 8,

		[Description("Barrack 1")]
		Barrack1 = 10,
		[Description("Barrack 2")]
		Barrack2 = 11,
		[Description("Barrack 3")]
		Barrack3 = 12,
		[Description("Barrack 4")]
		Barrack4 = 13,

		[Description("Dark Barrack 1")]
		DarkBarrack1 = 16,
		[Description("Dark Barrack 2")]
		DarkBarrack2 = 17,

		[Description("Elixir Collector 1")]
		Elixir1 = 20,
		[Description("Elixir Collector 2")]
		Elixir2 = 21,
		[Description("Elixir Collector 3")]
		Elixir3 = 22,
		[Description("Elixir Collector 4")]
		Elixir4 = 23,
		[Description("Elixir Collector 5")]
		Elixir5 = 24,
		[Description("Elixir Collector 6")]
		Elixir6 = 25,
		[Description("Elixir Collector 7")]
		Elixir7 = 26,

		[Description("Gold Mine 1")]
		Gold1 = 30,
		[Description("Gold Mine 2")]
		Gold2 = 31,
		[Description("Gold Mine 3")]
		Gold3 = 32,
		[Description("Gold Mine 4")]
		Gold4 = 33,
		[Description("Gold Mine 5")]
		Gold5 = 34,
		[Description("Gold Mine 6")]
		Gold6 = 35,
		[Description("Gold Mine 7")]
		Gold7 = 36,

		[Description("Dark Elixir Drill 1")]
		Drill1 = 40,
		[Description("Dark Elixir Drill 2")]
		Drill2 = 41,
		[Description("Dark Elixir Drill 3")]
		Drill3 = 42

		// TODO: add the other buildings as needed, continue from value 50 (leave some space between building type just in case CoC adds more)
	}
}