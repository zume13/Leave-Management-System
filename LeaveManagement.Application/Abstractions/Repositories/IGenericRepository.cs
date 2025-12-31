namespace LeaveManagement.Domain.Commons.Contracts
{
    public interface IGenericRepository<TValue> where TValue : class
    {
        Task<IReadOnlyList<TValue>> GetAll();
        Task<TValue?> GetById(Guid id);
        Task Add(TValue value);
        Task Update(TValue value);
        Task Delete(Guid Id);
    }
}
