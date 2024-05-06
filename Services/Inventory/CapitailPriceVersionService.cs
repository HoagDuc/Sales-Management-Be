using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class CapitailPriceVersionService : BaseService, ICapitailPriceVersionService
{
    public CapitailPriceVersionService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task CreateBy(short? quantity, decimal? price, long inventoryId, DateTime? dateTime = null)
    {
        var entity = new CapitailPriceVersion
        {
            Quantity = quantity,
            CapitalPrice = price,
            InventoryId = inventoryId,
            ReceiptDate = dateTime ?? DateTime.Now
        };
        await UnitOfWork.CapitalPriceVersionRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task<List<CapitailPriceVersion>> GetByInventoryId(long inventoryId)
    {
        return await UnitOfWork.CapitalPriceVersionRepo.GetByInventoryId(inventoryId);
    }

    public async Task<CapitailPriceResp> GetTotalPrice(long inventoryId)
    {
        var list = await UnitOfWork.CapitalPriceVersionRepo.GetByInventoryId(inventoryId);

        var total = list.Sum(x => x.CapitalPrice * x.Quantity);
        var Quantity = list.Sum(x => x.Quantity);

        return new CapitailPriceResp
        {
            TotalPrice = total,
            Quantity = Quantity
        };
    }

    public async Task OutInventory(long entityInventoryId, short? quantity)
    {
        var entities = await UnitOfWork.CapitalPriceVersionRepo.GetByInventoryId(entityInventoryId);

        if (entities == null || !entities.Any())
        {
            return;
        }

        foreach (var item in entities)
        {
            if (!quantity.HasValue || !(quantity > 0)) continue;
            if (item.Quantity >= quantity)
            {
                item.Quantity -= (short)quantity;
                quantity = 0; // Hết quantity để trừ
            }
            else
            {
                quantity -= (short?)item.Quantity;
                item.Quantity = 0; // Đặt lại quantity của item về 0
            }

            UnitOfWork.CapitalPriceVersionRepo.Update(item);
        }

        await UnitOfWork.CompleteAsync();
    }
}