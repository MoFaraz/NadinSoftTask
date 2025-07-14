namespace NadinSoft.Api.Models.Product;

public record ChangeAvailabilityApiModel(
    Guid Id,
    bool Availability
);