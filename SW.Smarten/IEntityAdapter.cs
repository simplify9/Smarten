using Marten;
using System.Threading.Tasks;

namespace SW.Smarten
{
    public interface IEntityAdapter
    {
        Task<object> GetById(string id);
        Task<object> Search(string search);
        Task BulkSave(string bulkData);

    }
}
