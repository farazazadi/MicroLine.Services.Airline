namespace MicroLine.Services.Airline.Tests.Architecture.GeneralRules;

public class NamingConventionRules
{
    private static GivenMethodMembersThat Methods() => 
        MethodMembers()
            .That()
            .AreNoConstructors()
            .And();


    [Fact]
    internal void GetterMethodsShouldNotReturnVoid()
    {
        Methods()
            .HaveName("Get[A-Z].*", true)
            .Should()
            .NotHaveReturnType(typeof(void))
            .Because("of respecting naming convention")
            .Check(Assemblies.Architecture);
    }


    [Fact]
    public void IsAndHasMethodsShouldReturnBooleans()
    {
        Methods()
            .HaveName("Is[A-Z].*", true)
            .Or()
            .HaveName("Has[A-Z].*", true)
            .Should()
            .HaveReturnType(typeof(bool))
            .Because("of respecting naming convention")
            .Check(Assemblies.Architecture);
    }


    [Fact]
    public void SetterMethodsShouldReturnVoid()
    {
        Methods()
            .HaveName("Set[A-Z].*", true)
            .Should()
            .HaveReturnType(typeof(void))
            .Because("of respecting CQS principle and naming convention")
            .Check(Assemblies.Architecture);
    }


    [Fact]
    public void InterfaceNameShouldStartWithAnI()
    {
        Interfaces()
            .Should()
            .HaveNameStartingWith("I")
            .Because("of respecting naming conventions")
            .Check(Assemblies.Architecture);
    }
}