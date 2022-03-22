using API.Core.Entities;
using API.Infrastructure.Data.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Infrastructure.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductsImages { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderedProduct> OrderedProducts { get; set; } = null!;

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("DATE");

            base.ConfigureConventions(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductImageMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new OrderedProductMapping());
            modelBuilder.ApplyConfiguration(new OrderMapping());
            modelBuilder.ApplyConfiguration(new ApplicationUserMapping());

            SeedData(modelBuilder);
        }
        protected void SeedData(ModelBuilder modelBuilder)
        {
            //SeedRoles(modelBuilder);
            Guid administratorRoleId = SeedRoles(modelBuilder);
            SeedUser(modelBuilder, administratorRoleId);
            SeedProducts(modelBuilder);

        }

        protected Guid SeedRoles(ModelBuilder modelBuilder)
        {
            //Seeding a 'Administrator' and 'User' roles to AspNetRoles table
            Guid administratorRoleId = Guid.NewGuid(); //Save GUID to be used on seed employee.
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = administratorRoleId, Name = "Administrator", NormalizedName = "ADMINISTRATOR" });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" });

            return administratorRoleId;
        }

        protected void SeedUser(ModelBuilder modelBuilder, Guid administratorRoleId)
        {

            //Generate administrator user id.
            Guid adminUserId = Guid.NewGuid();

            //A hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<ApplicationUser?>();

            //Seeding the Employee to AspNetUsers table
            modelBuilder.Entity<ApplicationUser>(u =>
            {
                var a = u.HasData(new
                {
                    Id = adminUserId,
                    Name = "Admin name",
                    UserName = "adm@test.dev",
                    Email = "adm@test.dev",
                    BirthDate = DateTime.MinValue,
                    IsActive = true,
                    MemberSince = DateTime.MinValue,
                    PhoneNumberConfirmed = true,

                    NormalizedUserName = "ADMIN@TEST.DEV",
                    NormalizedEmail = "ADMIN@TEST.DEV",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "$ABC12345wasd$"),

                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,

                });
                u.OwnsOne(u => u.Address).HasData(new
                {
                    ApplicationUserId = adminUserId,
                    AddressLine1 = "Avenida Rio Branco 134",
                    AddressLine2 = "",
                    City = "Rio de Janeiro",
                    State = "RJ",
                    Neighborhood = "Centro",
                    ZipCode = "20040921",
                    Country = "Brasil",
                });
            });




            //Seeding the relation between our user and 'Administrator' role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                    new IdentityUserRole<Guid>
                    {
                        RoleId = administratorRoleId,
                        UserId = adminUserId,
                    }
                );
        }

        protected void SeedProducts(ModelBuilder modelBuilder)
        {
            var tecladoGamerTDagger = new Product(
                "Teclado Mecânico Gamer T-Dagger Bora, RGB, Switch Outemu Brown, PT - T-TGK315-BROWN",
                ("O T-Dagger Bora RGB traz tudo que um teclado mecânico deve ter: um RGB encantador, Switches de confiança, teclas que não desgastam e um prazer de uso absoluto!" +
                    Environment.NewLine +
                    "Inclui iluminação RGB individual por tecla que pode ser customizada tanto diretamente no teclado ou pelo software. O design elegante com as teclas flutuantes trazem estilo ao Bora sem comprometer a estética minimalista. O Bora também é de fácil transporte devido ao seu cabo removível."),
                (decimal)189.90,
                100,
                null);
            modelBuilder.Entity<Product>().HasData(tecladoGamerTDagger);

            var imageTecladoGamerTDagger = new ProductImage("https://i.imgur.com/Offz8C9.jpg", tecladoGamerTDagger.Id);
            modelBuilder.Entity<ProductImage>().HasData(imageTecladoGamerTDagger);


            var tecladoGamerHusky = new Product(
                "Teclado Mecânico Gamer Husky Gaming Blizzard, Branco, 60%, Switch Gateron Red, ABNT2, RGB - HGMO001",
                ("O Teclado Mecânico Gamer Husky Gaming Blizzard, 60%, RGB, Switch Gateron Red, ABNT2, Branco é perfeito para quem precisa de performance e precisão. Prepare-se para muitas horas de batalha intensa em seus gameplays!" +
                    Environment.NewLine +
                    "Switches com feedback audível e tátil"
                    + Environment.NewLine +
                    "Switch Gateron Red contam com um feedback audível e tátil.É versátil e aprimorado em relação a outros switches.Ele traz a tecnologia dos switches mecânicos e é um ícone para os jogos e longos períodos de digitação.Tem um clique único, suas teclas possuem um som \"clicky\" característico."),
                (decimal)229.90,
                30,
                null);
            modelBuilder.Entity<Product>().HasData(tecladoGamerHusky);

            var imageTecladoGamerHusky = new ProductImage("https://i.imgur.com/QIdu2KR.jpg", tecladoGamerHusky.Id);
            modelBuilder.Entity<ProductImage>().HasData(imageTecladoGamerHusky);


            var keycapsHyperXPudding = new Product(
                "Keycaps HyperX Pudding - HKCPXA-BK-BR",
                "As capas de tecla HyperX Pudding apresentam um estilo de duas camadas translúcidas e a fonte exclusiva HyperX, ambas projetadas para proporcionar brilho extra.",
                (decimal)129.90,
                200,
                null);
            modelBuilder.Entity<Product>().HasData(keycapsHyperXPudding);

            var imageKeycapsHyperXPudding = new ProductImage("https://i.imgur.com/Yd6SnMP.jpg", keycapsHyperXPudding.Id);
            modelBuilder.Entity<ProductImage>().HasData(imageKeycapsHyperXPudding);
        }


        /// <summary>
        /// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa.
        /// </summary>
        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            /// <summary>
            /// Creates a new instance of this converter.
            /// </summary>
            public DateOnlyConverter() : base(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d))
            { }
        }

        /// <summary>
        /// Converts <see cref="DateOnly?" /> to <see cref="DateTime?"/> and vice versa.
        /// </summary>
        public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
        {
            /// <summary>
            /// Creates a new instance of this converter.
            /// </summary>
            public NullableDateOnlyConverter() : base(
                d => d == null
                    ? null
                    : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
                d => d == null
                    ? null
                    : new DateOnly?(DateOnly.FromDateTime(d.Value)))
            { }
        }
    }
}
