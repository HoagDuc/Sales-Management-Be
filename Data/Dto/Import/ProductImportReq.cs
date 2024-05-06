namespace ptdn_net.Data.Dto.Import;

public class ProductImportReq
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ShortDescription { get; set; }

    public long? OriginId { get; set; }

    public long CategoryId { get; set; }

    public long? BrandId { get; set; }

    public long? VendorId { get; set; }

    public string? Description { get; set; }

    public float? Volume { get; set; }

    public long UnitId { get; set; }

    public short? Discount { get; set; }

    public decimal? RetailPrice { get; set; }

    public decimal? WholesalePrice { get; set; }

    public decimal? CostPrice { get; set; }

    public decimal? Vat { get; set; }

    public string? Barcode { get; set; }

    public bool IsActive { get; set; }
    
    public long? MinQuantity { get; set; }

    public long Quantity { get; set; }
}