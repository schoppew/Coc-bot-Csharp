namespace CoC.Bot.Data
{
	public enum BuildingType
	{
		Other = 1,			// Any that don't fit
		Extractor = 2,		// Elixir Collectors, Gold Mines and Dark Elixir Drills
		Barrack = 3,		// Elixir Barracks
		DarkBarrack = 4,	// Dark Elixir Barracks
		ArmyCamp = 5		// Army Camps

		// TODO: add the other buildings as needed
	}
}