namespace Notary
{
    public enum KeyUsageFlags
    {
        ServerSigning = 0x01,
        ClientSigning = 0x02,
        CodeSigning = 0x04,
        OcspSigning = 0x08,
        EmailProtection = 0x10,
        TimeStamping = 0x20
    }
}