namespace Apbdtask7.Repositories;

using Apbdtask7.Data;
using Apbdtask7.DTOs;
using Apbdtask7.Models;
using Microsoft.EntityFrameworkCore;

public class PcRepository : IPcRepository
{
    private readonly AppDbContext _db;

    public PcRepository(AppDbContext context)
    {
        _db = context;
    }

    public async Task<IEnumerable<PcDto>> GetAllPcsAsync()
    {
        return await _db.PCs
            .Select(p => new PcDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock
            }).ToListAsync();
    }

    public async Task<PcComponentsResponse?> GetPcComponentsAsync(int id)
    {
        var computerRecord = await _db.PCs.Include(p => p.PCComponents)
            .ThenInclude(pcc => pcc.Component)
            .ThenInclude(c => c.ComponentManufacturer)
            .Include(p => p.PCComponents)
            .ThenInclude(pcc => pcc.Component)
            .ThenInclude(c => c.ComponentType)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (computerRecord is null)
            return null;

        return new PcComponentsResponse
        {
            Id = computerRecord.Id,
            Name = computerRecord.Name,
            Components = computerRecord.PCComponents.Select(pcc => new ComponentInPcResponse
            {
                Code = pcc.ComponentCode.Trim(),
                Name = pcc.Component.Name,
                Description = pcc.Component.Description,
                Quantity = pcc.Amount,
                Manufacturer = pcc.Component.ComponentManufacturer.FullName,
                Type = pcc.Component.ComponentType.Name
            }).ToList()
        };
    }

    public async Task<PcDto> CreatePcAsync(CreatePcRequest request)
    {
        var newComputer = new PC
        {
            Name = request.Name,
            Weight = request.Weight,
            Warranty = request.Warranty,
            CreatedAt = request.CreatedAt,
            Stock = request.Stock
        };

        _db.PCs.Add(newComputer);
        await _db.SaveChangesAsync();

        return new PcDto
        {
            Id = newComputer.Id,
            Name = newComputer.Name,
            Weight = newComputer.Weight,
            Warranty = newComputer.Warranty,
            CreatedAt = newComputer.CreatedAt,
            Stock = newComputer.Stock
        };
    }

    public async Task<bool> UpdatePcAsync(int id, UpdatePcRequest request)
    {
        var computerRecord = await _db.PCs.FindAsync(id);
        
        if (computerRecord is null)
            return false;

        computerRecord.Name = request.Name;
        computerRecord.Weight = request.Weight;
        computerRecord.Warranty = request.Warranty;
        computerRecord.CreatedAt = request.CreatedAt;
        computerRecord.Stock = request.Stock;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePcAsync(int id)
    {
        var computerRecord = await _db.PCs.FindAsync(id);
        
        if (computerRecord is null)
            return false;

        _db.PCs.Remove(computerRecord);
        await _db.SaveChangesAsync();
        return true;
    }
}