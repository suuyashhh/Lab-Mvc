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
        
        List<DTODoctor> GetDoctorList();
        Task<string> GetLastDoctorIdForFixedParts(string fixedPart, string fixedPartSec);
    }
}
