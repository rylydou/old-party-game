namespace MGE
{
	public interface ISerializable
	{
		void OnBeforeSerilize();
		void OnAfterSerilize();

		void OnAfterDeserilize();
	}
}