namespace MicroLine.Services.Airline.Application.Common.DataTransferObjects;

public record EntityAuditingDetailsDto(
    string CreatedBy,
    DateTime CreatedAtUtc,
    string LastModifiedBy,
    DateTime? LastModifiedAtUtc
);