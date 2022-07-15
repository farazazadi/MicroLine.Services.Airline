using MicroLine.Services.Airline.Domain.Aircraft.Exceptions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Aircraft;

public sealed class AircraftModel : ValueObject
{
    private readonly string _model;

    private AircraftModel(string aircraftModel) => _model = aircraftModel;

    public static AircraftModel Create(string aircraftModel)
    {
        Validate(aircraftModel);

        return new AircraftModel(aircraftModel.Trim());
    }

    private static void Validate(string aircraftModel)
    {
        if (aircraftModel.IsNullOrEmpty())
            throw new InvalidAircraftModelException("Aircraft model could not be null or empty!");

        if(aircraftModel.Trim().Length is < 3 or > 15)
            throw new InvalidAircraftModelException("Aircraft model's length could not be greater than 15 or less than 3 characters!");
    }


    public static implicit operator string(AircraftModel aircraftModel) => aircraftModel.ToString();
    public static implicit operator AircraftModel(string aircraftModel) => Create(aircraftModel);

    public override string ToString() => _model;

}