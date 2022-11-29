namespace MicroLine.Services.Airline.Domain.Common;
public class Result
{
    private enum ReasonType
    {
        Failure,
        Success
    }

    private readonly List<(ReasonType Type, string Reason)> _results = new ();

    public IReadOnlyList<string> FailureReasons =>
        _results.Where(r => r.Type == ReasonType.Failure)
            .Select(r => r.Reason)
            .ToList();

    public bool IsSuccess => _results.All(r => r.Type == ReasonType.Success);


    public Result()
    {
    }

    public static Result Fail(string reason)
    {
        var result = new Result ();
        result._results.Add ((ReasonType.Failure ,reason));
        return result;
    }

    public static Result Success(string reason)
    {
        var result = new Result();
        result._results.Add((ReasonType.Success, reason));
        return result;
    }


    public Result WithFailure(string reason)
    {
        _results.Add((ReasonType.Failure, reason));
        return this;
    }

    public Result WithSuccess(string reason)
    {
        _results.Add((ReasonType.Success, reason));
        return this;
    }


    public Result Concat(Result other)
    {
        if(other is null)
            throw new ArgumentNullException (nameof(other));

        foreach (var result in other._results)
            _results.Add(result);

        return this;
    }
}