using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Tests.Common.Extensions;

public static class EntitiesExtensions
{

    public static string GenerateFormattedString<TEntity>(
        this IEnumerable<TEntity> entities,
        string messagePattern,
        params Func<TEntity, object>[] memberSelectors
    ) where TEntity : Entity
    {
        IEnumerable<string> formattedMessages =
            entities.Select(entity =>
            {
                return string.Format(messagePattern,
                    memberSelectors.Select(selector => selector(entity)).ToArray());
            });

        var generateExceptionMessage = string.Join(Environment.NewLine, formattedMessages);

        return generateExceptionMessage;
    }

}