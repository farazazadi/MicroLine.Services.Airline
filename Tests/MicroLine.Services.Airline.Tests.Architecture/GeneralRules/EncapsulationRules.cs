namespace MicroLine.Services.Airline.Tests.Architecture.GeneralRules;

public class EncapsulationRules
{
    [Fact]
    public void TypesShouldNotHavePublicOrInternalFields()
    {
        FieldMembers()
            .That().ArePublic().Or().AreInternal()
            .And().AreNotDeclaredIn(Types().That().AreEnums())
            .Should()
            .NotExist()
            .Because("of encapsulation violation")
            .Check(Assemblies.Architecture);
    }


    [Fact]
    public void PropertiesShouldNotHavePublicSetter()
    {
        PropertyMembers()
            .That().ArePublic().Or().AreInternal()
            .Should()
            .NotHavePublicSetter()
            .Because("of encapsulation violation")
            .Check(Assemblies.Architecture);
    }


}