using System.IO;
using ProtoBuf;

namespace Blog.Infrastructure.Serialization;

public sealed class ProtoBufSerializer
{
    public static byte[] ClassToByteArray<T>(T value) where T : class
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, value);
        return stream.ToArray();
    }

    public static T ByteArrayToClass<T>(byte[] data) where T : class
    {
        using var stream = new MemoryStream(data);
        return Serializer.Deserialize<T>(stream);
    }
}