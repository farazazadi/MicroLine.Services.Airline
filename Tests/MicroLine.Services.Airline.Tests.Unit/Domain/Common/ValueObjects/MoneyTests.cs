
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class MoneyTests
{

    public static TheoryData<decimal, Money.CurrencyType> ValidMoneyList = new()
    {
        {10.50m, Money.CurrencyType.UnitedStatesDollar},
        {20.25m, Money.CurrencyType.PoundSterling},
        {5, Money.CurrencyType.Euro},
        {50.00m, Money.CurrencyType.CanadianDollar},
        {50.00m, Money.CurrencyType.AustralianDollar}
    };

    [Theory, MemberData(nameof(ValidMoneyList))]
    public void Money_ShouldCreateAsExpected_WhenItCreatesFromValidInput(decimal amount, Money.CurrencyType currency)
    {
        // Given
        // When
        var money = Money.Of(amount, currency);

        // Then
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void Money_ShouldThrowInvalidMoneyException_WhenItCreatesFromNegativeInput()
    {
        // Given
        // When
        var func = () => Money.Of(-1, Money.CurrencyType.CanadianDollar);

        // Then
        func.Should().ThrowExactly<InvalidMoneyException>()
            .And.Code.Should().Be(nameof(InvalidMoneyException));
    }


    public static TheoryData<string> ArithmeticOperators = new()
    {
        "+",
        "-",
        "*",
        "/"
    };

    [Theory, MemberData(nameof(ArithmeticOperators))]
    public void ArithmeticOperationsOfMoneys_ShouldHaveExpectedResult(string operatorType)
    {
        // Given
        var currency = Money.CurrencyType.PoundSterling;
        var leftAmount = 10.90m;
        var rightAmount = 3.75m;

        var leftMoney = Money.Of(leftAmount, currency);
        var rightMoney = Money.Of(rightAmount, currency);

        var expectedResult = operatorType switch
        {
            "+" => leftAmount + rightAmount,
            "-" => leftAmount - rightAmount,
            "*" => leftAmount * rightAmount,
            "/" => leftAmount / rightAmount,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };


        // When
        var resultMoney = operatorType switch
        {
            "+" => leftMoney + rightMoney,
            "-" => leftMoney - rightMoney,
            "*" => leftMoney * rightMoney,
            "/" => leftMoney / rightMoney,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };

        // Then
        resultMoney.Amount.Should().Be(expectedResult);
        resultMoney.Currency.Should().Be(currency);

    }



    public static TheoryData<Money.CurrencyType, Money.CurrencyType, string> ArithmeticOperationsWithDifferentCurrenciesData = new()
    {
        {Money.CurrencyType.CanadianDollar, Money.CurrencyType.Euro, "+"},
        {Money.CurrencyType.PoundSterling, Money.CurrencyType.Euro, "-"},
        {Money.CurrencyType.AustralianDollar, Money.CurrencyType.Euro, "*"},
        {Money.CurrencyType.UnitedStatesDollar, Money.CurrencyType.Euro, "/"}
    };

    [Theory, MemberData(nameof(ArithmeticOperationsWithDifferentCurrenciesData))]
    public void ArithmeticOperationsOfMoneys_ShouldThrowInvalidMoneyOperationException_WhenCurrencyTypesInLeftAndRightOperandsAreDifferent(
        Money.CurrencyType leftCurrencyType, Money.CurrencyType rightCurrencyType, string operatorType)
    {
        // Given
        var leftMoney = Money.Of(15.50m, leftCurrencyType);
        var rightMoney = Money.Of(5.75m, rightCurrencyType);

        // When
        var func = () => operatorType switch
        {
            "+" => leftMoney + rightMoney,
            "-" => leftMoney - rightMoney,
            "*" => leftMoney * rightMoney,
            "/" => leftMoney / rightMoney,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };

        // Then
        func.Should().ThrowExactly<InvalidMoneyOperationException>()
            .And.Code.Should().Be(nameof(InvalidMoneyOperationException));

    }


    public static TheoryData<string> ComparisonOperators = new()
    {
        ">",
        ">=",
        "<",
        "<=",
        "==",
        "!="
    };

    [Theory, MemberData(nameof(ComparisonOperators))]
    public void ComparingTwoMoneys_ShouldHaveExpectedResult(string operatorType)
    {
        // Given
        var currency = Money.CurrencyType.Euro;
        var leftAmount = 10.50m;
        var rightAmount = 3.75m;

        var leftMoney = Money.Of(leftAmount, currency);
        var rightMoney = Money.Of(rightAmount, currency);

        var expectedResult = operatorType switch
        {
            ">" => leftAmount > rightAmount,
            ">=" => leftAmount >= rightAmount,
            "<" => leftAmount < rightAmount,
            "<=" => leftAmount <= rightAmount,
            "==" => leftAmount == rightAmount,
            "!=" => leftAmount != rightAmount,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };


        // When
        var result = operatorType switch
        {
            ">" => leftMoney > rightMoney,
            ">=" => leftMoney >= rightMoney,
            "<" => leftMoney < rightMoney,
            "<=" => leftMoney <= rightMoney,
            "==" => leftMoney == rightMoney,
            "!=" => leftMoney != rightMoney,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };

        // Then
        result.Should().Be(expectedResult);

    }


    public static TheoryData<Money.CurrencyType, Money.CurrencyType, string> ComparisonOperationsWithDifferentCurrenciesData = new()
    {
        {Money.CurrencyType.CanadianDollar, Money.CurrencyType.Euro, ">"},
        {Money.CurrencyType.PoundSterling, Money.CurrencyType.Euro, ">="},
        {Money.CurrencyType.AustralianDollar, Money.CurrencyType.Euro, "<"},
        {Money.CurrencyType.UnitedStatesDollar, Money.CurrencyType.Euro, "<="}
    };

    [Theory, MemberData(nameof(ComparisonOperationsWithDifferentCurrenciesData))]
    public void ComparisonOperationsOfMoneys_ShouldThrowInvalidMoneyOperationException_WhenCurrencyTypesInLeftAndRightOperandsAreDifferent(
        Money.CurrencyType leftCurrencyType, Money.CurrencyType rightCurrencyType, string operatorType)
    {
        // Given
        var leftMoney = Money.Of(15.50m, leftCurrencyType);
        var rightMoney = Money.Of(5.75m, rightCurrencyType);

        // When
        var func = () => operatorType switch
        {
            ">" => leftMoney > rightMoney,
            ">=" => leftMoney >= rightMoney,
            "<" => leftMoney < rightMoney,
            "<=" => leftMoney <= rightMoney,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };

        // Then
        func.Should().ThrowExactly<InvalidMoneyOperationException>()
            .And.Code.Should().Be(nameof(InvalidMoneyOperationException));

    }
}
