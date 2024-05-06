using ptdn_net.Data.Entity;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Dto.Product;

public class ProductResp
{
    public long ProductId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string? ShortDescription { get; set; }

    public long? OriginId { get; set; }

    public long CategoryId { get; set; }

    public long? BrandId { get; set; }

    public long? VendorId { get; set; }

    public string? Description { get; set; }

    public float? Volume { get; set; }

    public long UnitId { get; set; }

    public short? Discount { get; set; }

    public decimal? GiaNiemYet { get; set; }

    public decimal? ChietKhauNhap { get; set; }

    public decimal? CostPrice { get; set; }

    public decimal? Vat { get; set; }

    public string Barcode { get; set; }

    public bool IsActive { get; set; }

    public Brand? Brand { get; set; }

    public Category Category { get; set; }

    public ICollection<Image> Images { get; set; }

    public Origin? Origin { get; set; }

    public Unit? Unit { get; set; }

    public Vendor? Vendor { get; set; }

    public ICollection<Inventory>? Inventory { get; set; }
    
    public ProductResp(Entity.ProductEntity.Product entity)
    {
        Inventory = entity.Inventories;
        ProductId = entity.ProductId;
        Code = entity.Code;
        Name = entity.Name;
        ShortDescription = entity.ShortDescription;
        OriginId = entity.OriginId;
        CategoryId = entity.CategoryId;
        BrandId = entity.BrandId;
        VendorId = entity.VendorId;
        Description = entity.Description;
        Volume = entity.Volume;
        UnitId = entity.UnitId;
        Discount = entity.Discount;
        GiaNiemYet = entity.GiaNiemYet;
        ChietKhauNhap = entity.ChietKhauNhap;
        CostPrice = entity.CostPrice;
        Vat = entity.Vat;
        Barcode = entity.Barcode;
        IsActive = entity.IsActive;
        Images = entity.Images;
        Brand = entity.Brand;
        Origin = entity.Origin;
        Unit = entity.Unit;
        Vendor = entity.Vendor;
        Category = entity.Category;
    }
}