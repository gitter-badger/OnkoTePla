namespace bytePassion.Lib.Types.Communication
{
    public abstract class AddressIdentifier
    {
        protected AddressIdentifier(AddressIdentifierType type)
        {
            Type = type;
        }

        public AddressIdentifierType Type { get; }       
    }
}