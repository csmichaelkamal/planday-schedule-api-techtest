using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planday.Schedule.Queries
{
    public interface IGetAllShiftsQuery
    {
        Task<IReadOnlyCollection<Shift>> QueryAsync();
    }    
}

