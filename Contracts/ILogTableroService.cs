using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ILogTableroService : IRepositoryBase<Log_Tablero>
    {
        Task LogEvent(LogTableroResponse logModel);
    }
}
