namespace MicroLine.Services.Airline.Tests.Architecture.Domain;

public class DomainRules
{
    private static GivenTypesConjunction DomainTypes() =>
        Types()
        .That()
        .ResideInAssembly(Assemblies.Domain);

    private static GivenMethodMembersConjunction DomainMethods() =>
        MethodMembers()
        .That()
        .AreDeclaredIn(DomainTypes());

    private static GivenPropertyMembersConjunction DomainProperties() =>
        PropertyMembers()
        .That()
        .AreDeclaredIn(DomainTypes());

    private static GivenFieldMembersConjunction DomainFields() =>
        FieldMembers()
        .That()
        .AreDeclaredIn(DomainTypes());



    [Fact]
    public void DomainLayerShouldNotDependOnAnyLayer()
    {
        DomainTypes()
            .Should()
            .OnlyDependOnTypesThat()
            .ResideInAssembly(Assemblies.Domain)
            .Because("of Domain model isolation")
            .Check(Assemblies.Architecture);
    }


    [Fact]
    public void DomainTypesShouldNotHaveAnyUnauthorizedAttribute()
    {
        DomainTypes()
            .And()
            .DoNotResideInNamespace(Namespaces.SystemAndItsInnerNamespacesRegexPattern, true)
            .And()
            .DoNotResideInNamespace(Namespaces.DomainCommonAndItsInnerNamespacesRegexPattern, true)
            .Should()
            .NotHaveAnyAttributesThat()
            .DoNotResideInNamespace(Namespaces.DomainAllowedAttributesNamespacesRegexPattern, true)
            .Because("of Domain model isolation")
            .Check(Assemblies.Architecture);
    }



    [Fact]
    public void DomainPropertiesShouldNotHaveAnyUnauthorizedAttribute()
    {
        DomainProperties()
            .Should()
            .NotHaveAnyAttributesThat()
            .DoNotResideInNamespace(Namespaces.DomainAllowedAttributesNamespacesRegexPattern, true)
            .Because("of Domain model isolation")
            .Check(Assemblies.Architecture);
    }



    [Fact]
    public void DomainFieldsShouldNotHaveAnyUnauthorizedAttribute()
    {
        DomainFields()
            .Should()
            .NotHaveAnyAttributesThat()
            .DoNotResideInNamespace(Namespaces.DomainAllowedAttributesNamespacesRegexPattern, true)
            .Because("of Domain model isolation")
            .Check(Assemblies.Architecture);
    }



    [Fact]
    public void DomainMethodsShouldNotHaveAnyUnauthorizedAttribute()
    {
        DomainMethods()
            .Should()
            .NotHaveAnyAttributesThat()
            .DoNotResideInNamespace(Namespaces.DomainAllowedAttributesNamespacesRegexPattern, true)
            .Because("of Domain model isolation")
            .Check(Assemblies.Architecture);
    }

}
