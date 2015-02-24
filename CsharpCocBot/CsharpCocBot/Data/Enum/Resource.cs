namespace CoC.Bot.Data
{
	using System.ComponentModel;

	/// <summary>
	/// The Resource
	/// </summary>
	public enum Resource
	{
		Gold = 1,

		Elixir = 2,

		[Description("Dark Elixir")]
		DarkElixir = 3,

		Gem = 4
	};
}