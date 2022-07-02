using System.Reflection;
using ArchUnitNET.Loader;
using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Tests.Architecture;

public static class Assemblies
{
    public static readonly Assembly Domain = typeof(ValueObject).Assembly;

    //public const string Domain = "MicroLine.Services.Airline.Domain";
    //public const string Application = "MicroLine.Services.Airline.Application";
    //public const string Infrastructure = "MicroLine.Services.Airline.Infrastructure"; 
    //public const string WebApi = "MicroLine.Services.Airline.WebApi";


    public static readonly Assembly[] AllSourceAssemblies = { Domain };


    public static readonly ArchUnitNET.Domain.Architecture Architecture =
        new ArchLoader().LoadAssemblies(AllSourceAssemblies)
            .Build();
}