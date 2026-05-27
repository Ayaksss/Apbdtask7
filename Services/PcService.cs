namespace Apbdtask7.Services;
using Apbdtask7.DTOs;
using Apbdtask7.Repositories;

public class PcService : IPcService
{
    private readonly IPcRepository _dataAccess;

    public PcService(IPcRepository pcRepository)
    {
        _dataAccess = pcRepository;
    }

    public Task<IEnumerable<PcDto>> GetAllPcsAsync()
    {
        return _dataAccess.GetAllPcsAsync();
    }

    public Task<PcComponentsResponse?> GetPcComponentsAsync(int id)
    {
        return _dataAccess.GetPcComponentsAsync(id);
    }

    public Task<PcDto> CreatePcAsync(CreatePcRequest request)
    {
        return _dataAccess.CreatePcAsync(request);
    }

    public Task<bool> UpdatePcAsync(int id, UpdatePcRequest request)
    {
        return _dataAccess.UpdatePcAsync(id, request);
    }

    public Task<bool> DeletePcAsync(int id)
    {
        return _dataAccess.DeletePcAsync(id);
    }
}