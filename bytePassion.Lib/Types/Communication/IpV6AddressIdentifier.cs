using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.Types.Communication
{
    public class IpV6AddressIdentifier : AddressIdentifier
    {
        public IpV6AddressIdentifier(byte part1, byte part2, byte part3, byte part4, byte part5, byte part6, IpPort port)
            : base(AddressIdentifierType.IpV4)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
            Part4 = part4;
            Part5 = part5;
            Part6 = part6;
            Port = port;
        }

        public byte Part1 { get; }
        public byte Part2 { get; }
        public byte Part3 { get; }
        public byte Part4 { get; }
        public byte Part5 { get; }
        public byte Part6 { get; }

        public IpPort Port { get; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, (a1, a2) => a1.Part1 == a2.Part1 &&
                                                a1.Part2 == a2.Part2 &&
                                                a1.Part3 == a2.Part3 &&
                                                a1.Part4 == a2.Part4 &&
                                                a1.Part5 == a2.Part5 &&
                                                a1.Part6 == a2.Part6 &&
                                                a1.Port == a2.Port);
        }

        public override int GetHashCode()
        {
            return Part1.GetHashCode() ^
                   Part2.GetHashCode() ^
                   Part3.GetHashCode() ^
                   Part4.GetHashCode() ^
                   Part5.GetHashCode() ^
                   Part6.GetHashCode()^
                   Port.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Part1}.{Part2}.{Part3}.{Part4}.{Part5}.{Part6}:{Port}";
        }

        public static bool operator ==(IpV6AddressIdentifier a1, IpV6AddressIdentifier a2) => EqualsExtension.EqualsForEqualityOperator(a1, a2);
        public static bool operator !=(IpV6AddressIdentifier a1, IpV6AddressIdentifier a2) => !(a1 == a2);
    }
}