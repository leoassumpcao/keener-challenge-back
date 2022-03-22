namespace API.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; protected set; }
        public IEnumerable<ProductViewModel> Products { get; protected set; }

        public CategoryViewModel(string name, IEnumerable<ProductViewModel> products)
        {
            Name = name;
            Products = products;
        }
    }
}
