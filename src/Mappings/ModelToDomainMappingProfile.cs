using API.Core.Entities;
using API.Core.ValueObjects;
using API.ViewModels;
using AutoMapper;


namespace API.Mappings
{
    public class ModelToDomainMappingProfile : Profile
    {
        public ModelToDomainMappingProfile()
        {
            CreateMap<ProductViewModel, Product>().ConvertUsing(from =>
                new Product(from.Name, from.Description, from.Price, 0, from.CategoryId));

            CreateMap<AddressViewModel, Address>().ConvertUsing(from =>
                new Address(from.AddressLine1, from.AddressLine2, from.City, from.State,
                    from.Neighborhood, from.ZipCode, from.Country));
        }
    }
}
