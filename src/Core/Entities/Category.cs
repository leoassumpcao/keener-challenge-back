namespace API.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; protected set; }
        public virtual IEnumerable<Product> Products { get; protected set; } = null!;

        public Category(string name)
        {
            Name = name;
        }
    }
}
