using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptOut)]
	public class EditorSettings
	{
		public Color textColor = new Color("#EEE");
		public Color accent = new Color("#FF7504");
		public Color bgColor = new Color("#0A0A0A");
		public Color panelBGColor = new Color(0.05f, 0.9f);
	}
}