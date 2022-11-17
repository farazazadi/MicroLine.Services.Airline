using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.DbContextInitializer;

internal class AirlineDbContextInitializer : IAirlineDbContextInitializer
{
    private readonly AirlineDbContext _dbContext;
    private readonly IAirportReadonlyRepository _airportReadonlyRepository;
    private readonly IAircraftReadonlyRepository _aircraftReadonlyRepository;
    private readonly IFlightCrewReadonlyRepository _flightCrewReadonlyRepository;
    private readonly ICabinCrewReadonlyRepository _cabinCrewReadonlyRepository;
    private readonly IDateTime _dateTime;
    private readonly ILogger _logger;

    public AirlineDbContextInitializer(
        AirlineDbContext dbContext,
        IAirportReadonlyRepository airportReadonlyRepository,
        IAircraftReadonlyRepository aircraftReadonlyRepository,
        IFlightCrewReadonlyRepository flightCrewReadonlyRepository,
        ICabinCrewReadonlyRepository cabinCrewReadonlyRepository,
        IDateTime dateTime,
        ILogger<AirlineDbContextInitializer> logger
        )
    {
        _dbContext = dbContext;
        _airportReadonlyRepository = airportReadonlyRepository;
        _aircraftReadonlyRepository = aircraftReadonlyRepository;
        _flightCrewReadonlyRepository = flightCrewReadonlyRepository;
        _cabinCrewReadonlyRepository = cabinCrewReadonlyRepository;
        _dateTime = dateTime;
        _logger = logger;
    }

    public async Task MigrateAsync(CancellationToken token = default)
    {
        var dbContextName = nameof(AirlineDbContext);

        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(token);
        var numberOfPendingMigrations = pendingMigrations.Count();

        _logger.LogInformation("{DbContext}'s Migration process started with {NumberOfMigrations} pending migrations."
            , dbContextName, numberOfPendingMigrations);

        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(token);

            if (!canConnect)
            {
                var databaseCreator = _dbContext.GetService<IRelationalDatabaseCreator>();

                var databaseExists = await databaseCreator.ExistsAsync(token);

                if (!databaseExists)
                    await databaseCreator.CreateAsync(token);
            }


            var executionStrategy = _dbContext.Database.CreateExecutionStrategy();


            await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
                await _dbContext.Database.MigrateAsync(token);
                await transaction.CommitAsync(token);
            });


            _logger.LogInformation("{DbContext}'s Migration process has been finished.", dbContextName);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "{DbContext}'s Migration process failed.", dbContextName);
            throw;
        }

    }


    public async Task SeedAsync(CancellationToken token = default)
    {
        var dbContextName = nameof(AirlineDbContext);

        try
        {
            _logger.LogInformation("{DbContext}'s seeding process started.", dbContextName);

            var flightsCount = await _dbContext.Flights.CountAsync(token);

            if (flightsCount > 0)
            {
                _logger.LogWarning("Due to the already existing data, seeding the database is not possible!");
                return;
            }

            await TrySeedAsync(token);

            _logger.LogInformation("{DbContext}'s seeding process has been finished.", dbContextName);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "{DbContext}'s seeding process failed.", dbContextName);
            throw;
        }
    }


    private async Task TrySeedAsync(CancellationToken token)
    {
        #region Airports

        var mehrabadAirport = await Airport.CreateAsync(
            IcaoCode.Create("OIII"),
            IataCode.Create("THR"),
            AirportName.Create("Mehrabad International Airport"),
            BaseUtcOffset.Create(3, 30),
            AirportLocation.Create(Continent.Asia, "Iran", "Tehran", "Tehran",
                35.68920135498047, 51.31340026855469),

            _airportReadonlyRepository,
            token
        );

        var istanbulAirport = await Airport.CreateAsync(
            IcaoCode.Create("LTFM"),
            IataCode.Create("IST"),
            AirportName.Create("Istanbul Airport"),
            BaseUtcOffset.Create(3, 0),
            AirportLocation.Create(Continent.Asia, "Turkey", "Istanbul", "Istanbul",
                41.275278, 28.751944),

            _airportReadonlyRepository,
            token
        );


        var torontoPearsonAirport = await Airport.CreateAsync(
            IcaoCode.Create("CYYZ"),
            IataCode.Create("YYZ"),
            AirportName.Create("Toronto Pearson International Airport"),
            BaseUtcOffset.Create(-5, 0),
            AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", "Toronto",
                43.6772003174, -79.63059997559999),

            _airportReadonlyRepository,
            token
        );


        var dublinAirport = await Airport.CreateAsync(
            IcaoCode.Create("EIDW"),
            IataCode.Create("DUB"),
            AirportName.Create("Dublin Airport"),
            BaseUtcOffset.Create(0, 0),
            AirportLocation.Create(Continent.Europe, "Ireland", "Dublin", "Collinstown",
                53.421299, -6.27007),

            _airportReadonlyRepository,
            token
        );

        var airports = new List<Airport>
        {
            mehrabadAirport, istanbulAirport,
            torontoPearsonAirport, torontoPearsonAirport, dublinAirport
        };

        #endregion

        await _dbContext.Airports.AddRangeAsync(airports, token);


        #region Aircrafts

        var airbusA320 = await Aircraft.CreateAsync(
            AircraftManufacturer.Airbus,
            AircraftModel.Create("A320"),
            Date.Create(2015, 05, 01),
            PassengerSeatingCapacity.Create(180, 45, 0),
            Speed.Create(828, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour),
            Speed.Create(871, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour),
            AircraftRegistrationCode.Create("EP-FSA"),

            _aircraftReadonlyRepository,
            token
            );

        var boeing777 = await Aircraft.CreateAsync(
            AircraftManufacturer.Boeing,
            AircraftModel.Create("777-300ER"),
            Date.Create(2010, 03, 30),
            PassengerSeatingCapacity.Create(300, 96, 0),
            Speed.Create(892, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour),
            Speed.Create(924, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour),
            AircraftRegistrationCode.Create("EP-FSB"),
            _aircraftReadonlyRepository,
            token
            );

        var aircrafts = new List<Aircraft> { airbusA320, boeing777 };

        #endregion

        await _dbContext.Aircrafts.AddRangeAsync(aircrafts, token);


        #region flightCrewMembers

        var pilot1 = await FlightCrew.CreateAsync(
            FlightCrewType.Pilot,
            Gender.Male,
            FullName.Create("Lonnie", "Watsica"),
            Date.Create(1974, 12, 27),
            NationalId.Create("ZQH46RS3VM7QC"),
            PassportNumber.Create("LR0CKY3W"),
            Email.Create("lonnie.watsica@test.com"),
            ContactNumber.Create("+111111111111"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );

        var pilot2 = await FlightCrew.CreateAsync(
            FlightCrewType.Pilot,
            Gender.Female,
            FullName.Create("Ella", "Daugherty"),
            Date.Create(1989, 5, 10),
            NationalId.Create("BV0OBTHSL8ZGCNR"),
            PassportNumber.Create("SNI0BCPA"),
            Email.Create("Ella.Daugherty@test.com"),
            ContactNumber.Create("+111111111112"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var coPilot1 = await FlightCrew.CreateAsync(
            FlightCrewType.CoPilot,
            Gender.Female,
            FullName.Create("Karen", "Ryan"),
            Date.Create(1985, 1, 13),
            NationalId.Create("EL16N749PRH1"),
            PassportNumber.Create("GR3V3S"),
            Email.Create("Karen.Ryan@test.com"),
            ContactNumber.Create("+111111111113"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var coPilot2 = await FlightCrew.CreateAsync(
            FlightCrewType.CoPilot,
            Gender.Male,
            FullName.Create("Wilbert", "Toy"),
            Date.Create(1979, 8, 19),
            NationalId.Create("LD86OHWK8U53EDB4PYZ"),
            PassportNumber.Create("TQMJG1I"),
            Email.Create("Wilbert.Toy@test.com"),
            ContactNumber.Create("+111111111114"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var flightEngineer1 = await FlightCrew.CreateAsync(
            FlightCrewType.FlightEngineer,
            Gender.Female,
            FullName.Create("Jessica", "Mitchell"),
            Date.Create(1991, 10, 12),
            NationalId.Create("NRNDZ0H0QM"),
            PassportNumber.Create("KMEA3W"),
            Email.Create("Jessica.Mitchell@test.com"),
            ContactNumber.Create("+111111111115"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var flightEngineer2 = await FlightCrew.CreateAsync(
            FlightCrewType.FlightEngineer,
            Gender.Male,
            FullName.Create("Martin", "Hirthe"),
            Date.Create(1972, 6, 20),
            NationalId.Create("0RIVCG8DB7375"),
            PassportNumber.Create("BG8FW1FU"),
            Email.Create("Martin.Hirthe@test.com"),
            ContactNumber.Create("+111111111116"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var navigator1 = await FlightCrew.CreateAsync(
            FlightCrewType.Navigator,
            Gender.Male,
            FullName.Create("Emanuel", "Leffler"),
            Date.Create(1993, 7, 11),
            NationalId.Create("TZ27IQ436EEGU6UQI8IR"),
            PassportNumber.Create("XVWSGSVP"),
            Email.Create("Emanuel.Leffler@test.com"),
            ContactNumber.Create("+111111111117"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var navigator2 = await FlightCrew.CreateAsync(
            FlightCrewType.Navigator,
            Gender.Female,
            FullName.Create("Josefina", "Russel"),
            Date.Create(1973, 6, 13),
            NationalId.Create("RFNB7AW8YA2R"),
            PassportNumber.Create("K0JUXDT7"),
            Email.Create("Josefina.Russel@test.com"),
            ContactNumber.Create("+111111111118"),
            Address.Create("test", "test", "test", "test", "test"),
            _flightCrewReadonlyRepository,
            token
        );


        var flightCrewMembers = new List<FlightCrew>
            {pilot1, pilot2, coPilot1, coPilot2, flightEngineer1, flightEngineer2, navigator1, navigator2};

        #endregion

        await _dbContext.FlightCrews.AddRangeAsync(flightCrewMembers, token);


        #region CabinCrewMembers

        var purser1 = await CabinCrew.CreateAsync(
            CabinCrewType.Purser,
            Gender.Female,
            FullName.Create("Jana", "Maggio"),
            Date.Create(1979, 7, 25),
            NationalId.Create("MRI28VC5S"),
            PassportNumber.Create("K1F88AC"),
            Email.Create("jana.maggio@test.com"),
            ContactNumber.Create("+111111111121"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var purser2 = await CabinCrew.CreateAsync(
            CabinCrewType.Purser,
            Gender.Female,
            FullName.Create("Joan", "Lang"),
            Date.Create(1973, 12, 18),
            NationalId.Create("0S0ECPYT39TJPTGMZ"),
            PassportNumber.Create("AC9WPM03"),
            Email.Create("Joan.Lang@test.com"),
            ContactNumber.Create("+111111111122"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var purser3 = await CabinCrew.CreateAsync(
            CabinCrewType.Purser,
            Gender.Male,
            FullName.Create("Johnathan", "Mayert"),
            Date.Create(1979, 1, 1),
            NationalId.Create("KHBZ3NBJ7SSCIC2NHIY"),
            PassportNumber.Create("WZPL7FVXM"),
            Email.Create("Johnathan.Mayert@test.com"),
            ContactNumber.Create("+111111111123"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var flightAttendant1 = await CabinCrew.CreateAsync(
            CabinCrewType.FlightAttendant,
            Gender.Female,
            FullName.Create("Jackie", "Russel"),
            Date.Create(1979, 9, 15),
            NationalId.Create("V7GFC76S5GSQWNL0B"),
            PassportNumber.Create("ZIR8XAM1"),
            Email.Create("Jackie.Russel@test.com"),
            ContactNumber.Create("+111111111124"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var flightAttendant2 = await CabinCrew.CreateAsync(
            CabinCrewType.FlightAttendant,
            Gender.Female,
            FullName.Create("Roberta", "Dach"),
            Date.Create(1986, 10, 17),
            NationalId.Create("65SBHCBXN7IC"),
            PassportNumber.Create("LJ6NI4V"),
            Email.Create("Roberta.Dach@test.com"),
            ContactNumber.Create("+111111111125"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var flightAttendant3 = await CabinCrew.CreateAsync(
            CabinCrewType.FlightAttendant,
            Gender.Male,
            FullName.Create("Kurt", "Feest"),
            Date.Create(1990, 5, 16),
            NationalId.Create("XN2HKC0FIIVEY"),
            PassportNumber.Create("8LI2P3JY9"),
            Email.Create("Kurt.Feest@test.com"),
            ContactNumber.Create("+111111111126"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var flightAttendant4 = await CabinCrew.CreateAsync(
            CabinCrewType.FlightAttendant,
            Gender.Male,
            FullName.Create("Charlie", "Schulist"),
            Date.Create(1995, 4, 19),
            NationalId.Create("0Y7Q5HNCTR"),
            PassportNumber.Create("CX9P4Z4"),
            Email.Create("Charlie.Schulist@test.com"),
            ContactNumber.Create("+111111111127"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var flightAttendant5 = await CabinCrew.CreateAsync(
            CabinCrewType.FlightAttendant,
            Gender.Female,
            FullName.Create("Faith", "Mitchell"),
            Date.Create(1978, 11, 23),
            NationalId.Create("BH55FIOWKZZQB3K2"),
            PassportNumber.Create("QNYV4JZQ"),
            Email.Create("Faith.Mitchell@test.com"),
            ContactNumber.Create("+111111111128"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var flightAttendant6 = await CabinCrew.CreateAsync(
            CabinCrewType.FlightAttendant,
            Gender.Female,
            FullName.Create("Blanca", "Schiller"),
            Date.Create(1981, 2, 28),
            NationalId.Create("U0RFEXA7LVECP"),
            PassportNumber.Create("WXS4NPI2O"),
            Email.Create("Blanca.Schiller@test.com"),
            ContactNumber.Create("+111111111129"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var chef1 = await CabinCrew.CreateAsync(
            CabinCrewType.Chef,
            Gender.Female,
            FullName.Create("Terri", "Thompson"),
            Date.Create(1969, 1, 10),
            NationalId.Create("7CTDDL0TH8MM0XPB"),
            PassportNumber.Create("A79SN7"),
            Email.Create("Terri.Terri@test.com"),
            ContactNumber.Create("+111111111130"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var chef2 = await CabinCrew.CreateAsync(
            CabinCrewType.Chef,
            Gender.Male,
            FullName.Create("Fredrick", "Okuneva"),
            Date.Create(1993, 9, 25),
            NationalId.Create("9WAZNWQS3CKXTKR7EH6"),
            PassportNumber.Create("FDZ5ET"),
            Email.Create("Fredrick.Okuneva@test.com"),
            ContactNumber.Create("+111111111131"),
            Address.Create("test", "test", "test", "test", "test"),
            _cabinCrewReadonlyRepository,
            token
        );


        var cabinCrewMembers = new List<CabinCrew>
        {
            purser1, purser2, purser3,
            flightAttendant1, flightAttendant2, flightAttendant3, flightAttendant4, flightAttendant5, flightAttendant6,
            chef1, chef2
        };

        #endregion

        await _dbContext.CabinCrews.AddRangeAsync(cabinCrewMembers, token);


        #region Flights

        var flightPricingPolicies = new List<IFlightPricingPolicy> { WeekDayFlightPricingPolicy.Create() };

        var flight1Price = FlightPrice.Create(
            Money.Of(200, Money.CurrencyType.UnitedStatesDollar),
            Money.Of(300, Money.CurrencyType.UnitedStatesDollar),
            Money.Of(0, Money.CurrencyType.UnitedStatesDollar)
        );

        var flight1FlightCrewMembers = new List<FlightCrew> { pilot1, coPilot1, flightEngineer1 };

        var flight1CabinCrewMembers = new List<CabinCrew>
            {purser1, flightAttendant1, flightAttendant2, flightAttendant3, chef1};

        var flight1 = await Flight.ScheduleNewFlightAsync(
            null,
            flightPricingPolicies,
            FlightNumber.Create("AJ50"),
            mehrabadAirport,
            dublinAirport,
            airbusA320,
            _dateTime.UtcNow.AddDays(10),
            flight1Price,
            flight1FlightCrewMembers,
            flight1CabinCrewMembers
        );


        var flight2Price = FlightPrice.Create(
            Money.Of(1000, Money.CurrencyType.UnitedStatesDollar),
            Money.Of(1450, Money.CurrencyType.UnitedStatesDollar),
            Money.Of(0, Money.CurrencyType.UnitedStatesDollar)
        );

        var flight2FlightCrewMembers = new List<FlightCrew> { pilot1, pilot2, coPilot2, flightEngineer2, navigator1 };

        var flight2CabinCrewMembers = new List<CabinCrew>
        {
            purser2,
            flightAttendant1, flightAttendant2, flightAttendant3, flightAttendant4, flightAttendant5, flightAttendant6,
            chef1
        };

        var flight2 = await Flight.ScheduleNewFlightAsync(
            null,
            flightPricingPolicies,
            FlightNumber.Create("CA35"),
            istanbulAirport,
            torontoPearsonAirport,
            boeing777,
            _dateTime.UtcNow.AddDays(20),
            flight2Price,
            flight2FlightCrewMembers,
            flight2CabinCrewMembers
        );


        var flights = new List<Flight> { flight1, flight2 };

        #endregion

        await _dbContext.Flights.AddRangeAsync(flights, token);


        await _dbContext.SaveChangesAsync(token);
    }
}