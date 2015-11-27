namespace bytePassion.Lib.Communication.State
{

    public interface IGlobalStateWriteOnly<in T>
    {
        T Value { set; }
    }
}