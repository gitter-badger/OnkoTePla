using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.Types.Communication
{
    public class IpV4AddressIdentifier : AddressIdentifier
    {
        public IpV4AddressIdentifier(byte part1, byte part2, byte part3, byte part4, IpPort port)
            : base(AddressIdentifierType.IpV4)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
            Part4 = part4;
            Port = port;
        }

        public byte Part1 { get; }
        public byte Part2 { get; }
        public byte Part3 { get; }
        public byte Part4 { get; }

        public IpPort Port { get; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, (a1, a2) => a1.Part1 == a2.Part1 &&
                                                a1.Part2 == a2.Part2 &&
                                                a1.Part3 == a2.Part3 &&
                                                a1.Part4 == a2.Part4 &&
                                                a1.Port == a2.Port);
        }

        public override int GetHashCode()
        {
            return Part1.GetHashCode() ^
                   Part2.GetHashCode() ^
                   Part3.GetHashCode() ^
                   Part4.GetHashCode() ^
                   Port.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Part1}.{Part2}.{Part3}.{Part4}:{Port}";
        }

        public static bool operator ==(IpV4AddressIdentifier a1, IpV4AddressIdentifier a2) => EqualsExtension.EqualsForEqualityOperator(a1, a2);
        public static bool operator !=(IpV4AddressIdentifier a1, IpV4AddressIdentifier a2) => !(a1 == a2);
    }
}