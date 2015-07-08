
namespace bytePassion.Lib.Clonable {

	public interface IGenericClonable<out T> 
	{
		T Clone ();
	}
}
