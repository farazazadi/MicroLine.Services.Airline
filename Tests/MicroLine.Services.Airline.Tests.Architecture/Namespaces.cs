using System.Diagnostics;
using System.Runtime.CompilerServices;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Attributes;

namespace MicroLine.Services.Airline.Tests.Architecture;

public static class Namespaces
{
    private static readonly string CompilerServices = typeof(CustomConstantAttribute).Namespace;
    private static readonly string DebuggerStepThroughAttribute = typeof(DebuggerStepThroughAttribute).Namespace;


    public const string Domain = "MicroLine.Services.Airline.Domain";

    public static readonly string DomainCommonNamespace = typeof(ValueObject).Namespace;

    public static readonly string DomainAttributesNamespace = $"{typeof(IgnoreMemberAttribute).Namespace}";

    public static readonly string DomainCommonAndItsInnerNamespacesRegexPattern = $@"^{DomainCommonNamespace.NormalizeForRegex()}\.*\b";
    public static readonly string SystemAndItsInnerNamespacesRegexPattern = $@"^System\.*\b";

    public static readonly string DomainExcludingCommonNamespaceRegexPattern = $@"^(?!.*{DomainCommonNamespace.NormalizeForRegex()}).*$";

    public static readonly string DomainAllowedAttributesNamespacesRegexPattern = $"{DomainAttributesNamespace.NormalizeForRegex()}|{CompilerServices.NormalizeForRegex()}|{DebuggerStepThroughAttribute.NormalizeForRegex()}";


    public const string Application = "MicroLine.Services.Airline.Application";
    public const string Infrastructure = "MicroLine.Services.Airline.Infrastructure"; 
    public const string WebApi = "MicroLine.Services.Airline.WebApi";


    private static string NormalizeForRegex(this string text) => text?.Replace(".", @"\.");

}