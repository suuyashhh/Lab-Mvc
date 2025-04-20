using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Interfaces
{
    public interface IMstEmployee
    {
        List<DTOEmployee> GetAll(string comid);
        Int64 Create(DTOEmployee _ObjEmployee);
        //Task<List<DTODoctor>> GetTestsAsync(string searchtext);
        Int64 Edit(DTOEmployee _objDtoEmployee);
        Int64 Delete(DTOEmployee _objDtoEmployee);

        List<DTOEmployee> GetEmployeeList(string comid);
        Task<string> GetLastEmployeeIdForFixedParts(string fixedPart, string fixedPartSec);

        DTOEmployee GetExisting(Int64 code);
    }
}
