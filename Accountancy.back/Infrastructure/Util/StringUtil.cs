namespace Accountancy.Infrastructure.Util;

public static class StringUtil
{
    public static byte[] HexToByteArray(this string hex)
    {
        var bytes = new byte[hex.Length / 2];
        for (var i = 0; i < hex.Length; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }
}