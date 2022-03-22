using API.Core.Entities;
using API.Core.ValueObjects;
using API.ViewModels;
using AutoMapper;


namespace API.Mappings
{
    public class DomainToModelMappingProfile : Profile
    {
        public DomainToModelMappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ConvertUsing(from => new ProductViewModel(from.Id, from.Name, from.Description, from.Price, (from.InStock > 0), ((from.Images != null && from.Images.Any()) ? from.Images.First().Name : ""), from.CategoryId));


            CreateMap<Address, AddressViewModel>();
            //.ConvertUsing(from =>
            //    new AddressViewModel(from.AddressLine1, from.AddressLine2, from.City,
            //from.State, from.Neighborhood, from.ZipCode, from.Country));

            /*CreateMap<ApplicationUser, UserViewModel>()
                .ConvertUsing((from, to, opt) =>
                    new UserViewModel(from.Id, from.Name, opt.Mapper.Map<Address, AddressViewModel>(from.Address),
                        from.BirthDate, from.IsActive, from.MemberSince));*/

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address));
            //.ForMember(dest => dest.BirthDate, opts => opts.MapFrom(src => src.BirthDate.ToString("dd/MM/yyyy")))
            //.ForMember(dest => dest.MemberSince, opts => opts.MapFrom(src => src.MemberSince.ToString("dd/MM/yyyy")));

            CreateMap<OrderedProduct, OrderedProductViewModel>()
                .ConvertUsing((from, source, opts) =>
                {
                    var product = opts.Mapper.Map<Product, ProductViewModel>(from.Product);
                    return new OrderedProductViewModel(from.Id, from.OrderId, from.ProductId, from.Quantity, from.UnitPrice, opts.Mapper.Map<Product, ProductViewModel>(from.Product));
                });
            //    .ForMember(dest => dest.Product, opts => opts.MapFrom(src => src.Product));
            //.ConvertUsing((from, source, opts) => new OrderedProductViewModel(from.Id, from.OrderId, from.ProductId, from.Quantity, from.UnitPrice, opts.Mapper.Map<Product, ProductViewModel>(from.Product)));

            CreateMap<Order, OrderViewModel>()
        //.ConvertUsing((from, source, opts) => new OrderViewModel(from.Id, from.UserId, from.Total, from.TotalPaid, from.Delivered, opts.Mapper.Map<Address, AddressViewModel>(from.DeliveryAddress), from.CreatedAt, opts.Mapper.Map<IEnumerable<OrderedProduct>, IEnumerable<OrderedProductViewModel>>(from.OrderedProducts)));

        .ForMember(dest => dest.DeliveryAddress, opts => opts.MapFrom((src, a, b) => src.DeliveryAddress))
        .ForMember(dest => dest.OrderedProducts, opts => opts.MapFrom((src, a, b) => src.OrderedProducts));

        }
    }
}
