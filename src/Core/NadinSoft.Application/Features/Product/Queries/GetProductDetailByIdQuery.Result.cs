using AutoMapper;
using NadinSoft.Application.Common.MappingConfiguration;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductDetailByIdQueryResult(
    Guid Id,
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProducedDate,
    Guid OwnerId,
    string OwnerUsername,
    DateTime CreatedDate,
    DateTime ModifiedDate) : ICreateApplicationMapper<ProductEntity>
{
    public void Map(Profile profile)
    {
        profile.CreateMap<ProductEntity, GetProductDetailByIdQueryResult>()
            .ForCtorParam(nameof(Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(Name), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(ManufacturePhone), opt => opt.MapFrom(src => src.ManufacturePhone))
            .ForCtorParam(nameof(ManufactureEmail), opt => opt.MapFrom(src => src.ManufactureEmail))
            .ForCtorParam(nameof(ProducedDate), opt => opt.MapFrom(src => src.ProduceDate))
            .ForCtorParam(nameof(OwnerId), opt => opt.MapFrom(src => src.UserId))
            .ForCtorParam(nameof(OwnerUsername), opt=> opt.MapFrom(src => src.User.UserName))
            .ForCtorParam(nameof(CreatedDate), opt => opt.MapFrom(src => src.CreatedDate))
            .ForCtorParam(nameof(ModifiedDate), opt => opt.MapFrom(src => src.ModifiedDate))
            .ReverseMap();
    }
}