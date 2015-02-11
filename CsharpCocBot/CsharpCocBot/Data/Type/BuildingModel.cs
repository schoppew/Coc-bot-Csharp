namespace CoC.Bot.Data
{
	/// <summary>
	/// Base class for all building
	/// Contents: 
	///		Name
	///		ColorList to locate it in the village
	///		Position
	/// </summary>
	public class BuildingModel
	{
		public string Name { get; set; }

		public ClickablePoint Location { get; set; }

		public ColorSet Colors { get; set; }
	}
}