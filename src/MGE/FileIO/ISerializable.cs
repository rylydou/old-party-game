namespace MGE.FileIO
{
	public interface ISerializable
	{
		void OnBeforeSerilize();
		void OnAfterSerilize();

		void OnBeforeDeserilize();
		void OnAfterDeserilize();
	}
}