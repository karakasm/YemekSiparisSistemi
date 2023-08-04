using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace YemekSiparisSistemi.Models;

public partial class FoodOrderSystemDbContext : DbContext
{
    public FoodOrderSystemDbContext()
    {
    }

    public FoodOrderSystemDbContext(DbContextOptions<FoodOrderSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuProduct> MenuProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_address");

            entity.ToTable("address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApartmentName)
                .HasMaxLength(20)
                .HasColumnName("apartment_name");
            entity.Property(e => e.ApartmentNumber).HasColumnName("apartment_number");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Floor)
                .HasMaxLength(15)
                .HasColumnName("floor");
            entity.Property(e => e.Neighbourhood)
                .HasMaxLength(30)
                .HasColumnName("neighbourhood");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(5)
                .HasColumnName("postal_code");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.Street)
                .HasMaxLength(30)
                .HasColumnName("street");
            entity.Property(e => e.Tag)
                .HasMaxLength(15)
                .HasColumnName("tag");

            entity.HasOne(d => d.District).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_address_district");

            entity.HasOne(d => d.Province).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_address_province");

            entity.HasMany(d => d.Users).WithMany(p => p.Adres)
                .UsingEntity<Dictionary<string, object>>(
                    "AddressUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_addressUser_user"),
                    l => l.HasOne<Address>().WithMany()
                        .HasForeignKey("AdresId")
                        .HasConstraintName("fk_addressUser_address"),
                    j =>
                    {
                        j.HasKey("AdresId", "UserId").HasName("pk_address_user");
                        j.ToTable("address_user");
                        j.IndexerProperty<int>("AdresId").HasColumnName("adres_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                    });
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_card");

            entity.ToTable("card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardName)
                .HasMaxLength(20)
                .HasColumnName("card_name");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .HasColumnName("card_number");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("date")
                .HasColumnName("expiration_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Cards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_card_user");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_category");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(20)
                .HasColumnName("category_name");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");

            entity.HasOne(d => d.Company).WithMany(p => p.Categories)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_category_company");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_comment");

            entity.ToTable("comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment1)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_comment_company");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_comment_user");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_company");

            entity.ToTable("company");

            entity.HasIndex(e => e.Email, "UQ__company__AB6E616437FB4CE1").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__company__B43B145F5650532F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .HasColumnName("company_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Logo)
                .HasColumnType("image")
                .HasColumnName("logo");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phone");

            entity.HasOne(d => d.Address).WithMany(p => p.Companies)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_company_address");
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_courier");

            entity.ToTable("courier");

            entity.HasIndex(e => e.Phone, "UQ__courier__B43B145FA4C1909B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Surname)
                .HasMaxLength(30)
                .HasColumnName("surname");

            entity.HasOne(d => d.Company).WithMany(p => p.Couriers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_courier_company");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_delivery");

            entity.ToTable("delivery");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.ArrivalDate).HasColumnName("arrival_date");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CourierId).HasColumnName("courier_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Address).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("fk_delivery_address");

            entity.HasOne(d => d.Company).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("fk_delivery_company");

            entity.HasOne(d => d.Courier).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.CourierId)
                .HasConstraintName("fk_delivery_courier");

            entity.HasOne(d => d.Order).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_delivery_order");

            entity.HasOne(d => d.Status).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("fk_delivery_deliveryStatus");
        });

        modelBuilder.Entity<DeliveryStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_deliveryStatus");

            entity.ToTable("delivery_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_district");

            entity.ToTable("district");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DistrictName)
                .HasMaxLength(30)
                .HasColumnName("district_name");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_menu");

            entity.ToTable("menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Detail)
                .HasColumnType("text")
                .HasColumnName("detail");
            entity.Property(e => e.Image)
                .HasColumnType("image")
                .HasColumnName("image");
            entity.Property(e => e.MenuName)
                .HasMaxLength(30)
                .HasColumnName("menu_name");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("smallmoney")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Company).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("fk_menu_company");
        });

        modelBuilder.Entity<MenuProduct>(entity =>
        {
            entity.HasKey(e => new { e.MenuId, e.ProductId }).HasName("pk_menuProduct");

            entity.ToTable("menu_product");

            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("smallmoney")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuProducts)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("fk_menuProduct_menu");

            entity.HasOne(d => d.Product).WithMany(p => p.MenuProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_menuProduct_product");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_order");

            entity.ToTable("order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("created_at");
            entity.Property(e => e.PaymentType).HasColumnName("payment_type");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("smallmoney")
                .HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_address");

            entity.HasOne(d => d.Company).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_company");

            entity.HasOne(d => d.PaymentTypeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_paymentType");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_user");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("pk_orderProduct");

            entity.ToTable("order_product");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_orderProduct_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderProduct_product");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("pk_payment");

            entity.ToTable("payment");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.Date)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("date");
            entity.Property(e => e.PaymentType).HasColumnName("payment_type");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("smallmoney")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Order).WithOne(p => p.Payment)
                .HasForeignKey<Payment>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_order");

            entity.HasOne(d => d.PaymentTypeNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentType)
                .HasConstraintName("fk_payment_paymentType");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_user");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_paymentType");

            entity.ToTable("payment_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PaymentName)
                .HasMaxLength(20)
                .HasColumnName("payment_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_product");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.ProductImage)
                .HasColumnType("image")
                .HasColumnName("product_image");
            entity.Property(e => e.ProductName)
                .HasMaxLength(30)
                .HasColumnName("product_name");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("smallmoney")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_product_category");

            entity.HasOne(d => d.Company).WithMany(p => p.Products)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("fk_product_company");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_province");

            entity.ToTable("province");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProvinceName)
                .HasMaxLength(30)
                .HasColumnName("province_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_role");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UQ__user__AB6E616439597CA0").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__user__B43B145FADB83BDB").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(30)
                .HasColumnName("surname");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
