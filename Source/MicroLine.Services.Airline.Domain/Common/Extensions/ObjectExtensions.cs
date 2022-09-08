namespace MicroLine.Services.Airline.Domain.Common.Extensions;

internal static class ObjectExtensions
{
    internal static Type GetRealType(this object obj)
    {
        const string efCoreProxyPrefix = "Castle.Proxies.";

        var type = obj.GetType();
        var typeString = type.ToString();

        if (typeString.Contains(efCoreProxyPrefix))
            return type.BaseType;

        return type;
    }
}