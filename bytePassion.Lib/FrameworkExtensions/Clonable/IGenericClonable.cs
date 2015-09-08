
namespace bytePassion.Lib.FrameworkExtensions.Clonable {

	public interface IGenericClonable<out T> 
	{
		T Clone ();
	}
}
