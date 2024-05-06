using AutoMapper;
using BT_MVC_Web.Exceptions;
using ClosedXML.Excel;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using TransactionType = ptdn_net.Const.Emun.TransactionType;

namespace ptdn_net.Services;

public class TransactionService : BaseService, ITransactionService
{
    private readonly IMapper _mapper;
    private readonly ITransactionTypeService _transactionTypeService;
    
    public TransactionService(ITransactionTypeService transactionTypeService,
        IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _transactionTypeService = transactionTypeService;
        _mapper = mapper;
    }
    
    public async Task<List<TransactionResp>> GetAll()
    {
        var entities = await UnitOfWork.TransactionRepo.GetAll();
        return _mapper.Map<List<TransactionResp>>(entities).OrderByDescending(x => x.Date).ToList();
    }

    public async Task<TransactionResp> GetById(Guid id)
    {
        var entity = await UnitOfWork.TransactionRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Transaction " + id + " not found");

        return _mapper.Map<TransactionResp>(entity);
    }

    public async Task<Guid> CreateBy(TransactionReq req)
    {
        await ValidCreateBy(req);
        
        var entity = _mapper.Map<Transaction>(req);

        await UnitOfWork.TransactionRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.TransactionId;
    }
    
    public async Task<Guid> Create(string code, long transactionTypeId, decimal? price, DateTime date)
    {
        var entity = new Transaction
        {
            Code = code,
            TransactionTypeId = transactionTypeId,
            Price = (decimal)price!,
            Date = date
        };

        await UnitOfWork.TransactionRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.TransactionId;
    }

    public async Task<Guid> UpdateBy(TransactionReq req, Guid id)
    {
        await ValidUpdateBy(id, req);
        
        var entity = await UnitOfWork.TransactionRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.TransactionRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.TransactionId;
    }
    
    private async Task ValidCreateBy(TransactionReq req)
    {
        await ValidExistsByCode(req);
        await _transactionTypeService.ValidNotExistsByTransactionTypeId(req.TransactionTypeId);
    }

    private async Task ValidUpdateBy(Guid id, TransactionReq req)
    {
        await ValidExistsByCode(req, id);
        await _transactionTypeService.ValidNotExistsByTransactionTypeId(req.TransactionTypeId);
    }
    
    private async Task ValidExistsByCode(TransactionReq req, Guid? id = null)
    {
        var existsBy = await UnitOfWork.TransactionRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task DeleteBy(Guid id)
    {
        var entity = await UnitOfWork.TransactionRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Transaction " + id + " not found");
        UnitOfWork.TransactionRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task<List<TransactionResp>> GetAllFromDate(DateTime ngayBatDau, DateTime ngayKetThuc)
    {
        var entities = await UnitOfWork.TransactionRepo.GetAllFromDate(ngayBatDau, ngayKetThuc);
        return _mapper.Map<List<TransactionResp>>(entities).OrderByDescending(x => x.Date).ToList();
    }

    public XLWorkbook Export(List<TransactionResp> transactionList, DateTime fromDate, DateTime toDate)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Danh sách phiếu thu");
        worksheet.Columns().AdjustToContents();

        var titleRow = worksheet.Range("A1:K1");
        titleRow.Merge();
        worksheet.Cell(1, 1).Value = $"Danh sách phiếu thu (từ {fromDate:dd/MM/yyyy} đến {toDate:dd/MM/yyyy})";
        titleRow.Style.Font.Bold = true;
        titleRow.Style.Font.FontColor = XLColor.Black;
        titleRow.Style.Font.FontSize = 30;

        var headerRow = worksheet.Range("A3:E3");
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRow.Style.Font.FontColor = XLColor.Black;

        worksheet.Cell(3, 1).Value = "STT";
        worksheet.Cell(3, 2).Value = "Mã phiếu";
        worksheet.Cell(3, 3).Value = "Loại giao dịch";
        worksheet.Cell(3, 4).Value = "Tổng tiền";
        worksheet.Cell(3, 5).Value = "Ngày";

        for (int col = 1; col <= 5; col++)
        {
            worksheet.Cell(3, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(3, col).Style.Border.OutsideBorderColor = XLColor.Black;
        }

        var stt = 1;
        var row = 4;
        foreach (var item in transactionList)
        {
            worksheet.Cell(row, 1).Value = stt++;
            worksheet.Cell(row, 2).Value = item.Code;
            worksheet.Cell(row, 3).Value = item.TransactionTypeId == (long)TransactionType.PhieuThu
                ? "Phiếu thu"
                : "Phiếu chi";
            worksheet.Cell(row, 4).Value = item.Price;
            worksheet.Cell(row, 5).Value = item.Date == null ? "" : item.Date.Value.ToString("dd/MM/yyyy");

            for (int col = 1; col <= 5; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.Black;
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        return workbook;
    }
}