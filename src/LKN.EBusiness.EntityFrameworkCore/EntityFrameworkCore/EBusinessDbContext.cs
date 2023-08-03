using LKN.EBusiness.Orders;
using LKN.EBusiness.Products;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LKN.EBusiness.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
[IgnoreMultiTenancy]
public class EBusinessDbContext :
    AbpDbContext<EBusinessDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion
    public DbSet<Order> Orders { get; set; } // 配置订单领域(以领域为单位)

    public DbSet<Product> Products { get; set; } // 配置商品(以领域为单位)

    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    public EBusinessDbContext(DbContextOptions<EBusinessDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Product>(b =>
        {
            b.ConfigureByConvention();
            b.HasMany(u => u.ProductImages).WithOne().HasForeignKey(ur => ur.ProductId).IsRequired();
        });

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(EBusinessConsts.DbTablePrefix + "YourEntities", EBusinessConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        /*
        builder.Entity<Product>(b => {
            b.ToTable(EBusinessConsts.DbTablePrefix+"Products",EBusinessConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property<Guid>("Id")
                       .ValueGeneratedOnAdd()
                       .HasColumnType("char(36)");

            b.Property<string>("ProductCode")
                .HasColumnType("longtext");

            b.Property<string>("ProductDescription")
                .HasColumnType("longtext");

            b.Property<decimal>("ProductPrice")
                .HasColumnType("decimal(65,2)");

            b.Property<int>("ProductSold")
                .HasColumnType("int");

            b.Property<int>("ProductSort")
                .HasColumnType("int");

            b.Property<string>("ProductStatus")
              
                .HasColumnType("longtext");

            b.Property<int>("ProductStock")
                .HasColumnType("int");

            b.Property<string>("ProductTitle")
                .HasColumnType("longtext");

            b.Property<string>("ProductUrl")
                .HasColumnType("longtext");

            b.Property<decimal>("ProductVirtualprice")
                .HasColumnType("decimal(65,3)");

            b.HasKey("Id");
        });
        builder.Entity<ProductImage>(b =>{
            b.ToTable(EBusinessConsts.DbTablePrefix + "ProductImage", EBusinessConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property<Guid>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("char(36)");

            b.Property<int>("ImageSort")
                .HasColumnType("int");

            b.Property<string>("ImageStatus")
                .HasColumnType("longtext");

            b.Property<string>("ImageUrl")
                .HasColumnType("longtext");

            b.Property<Guid>("ProductId")
                .HasColumnType("char(36)");

            b.HasKey("Id");

            b.HasIndex("ProductId");
        });
        */
    }
}
