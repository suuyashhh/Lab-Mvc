using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Interfaces
{
    public interface IMstDoctor
    {
        List<DTODoctor> GetAll();
        Int64 Create(DTODoctor _ObjDoctor);
        //Task<List<DTODoctor>> GetTestsAsync(string searchtext);
        // Dont Need          Task<string> GetLastTestIdForDate(string datePart);
        List<DTODoctor> GetDoctorList();
        Task<string> GetLastTestIdForFixedParts(string fixedPart, string fixedPartSec);
    }
}
