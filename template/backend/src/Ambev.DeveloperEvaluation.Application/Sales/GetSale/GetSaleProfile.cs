using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.Status == Domain.Enums.SaleStatus.Cancelled));
            
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}
