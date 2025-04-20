using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Interfaces
{
    public interface IMstDoctor
    {
        List<DTODoctor> GetAll(string comid);
        Int64 Create(DTODoctor _ObjDoctor);
        //Task<List<DTODoctor>> GetTestsAsync(string searchtext);
        Int64 Edit(DTODoctor _objDtoDoctor);
        Int64 Delete(DTODoctor _objDtoDoctor);

        List<DTODoctor> GetDoctorList(string comid);
        Task<string> GetLastDoctorIdForFixedParts(string fixedPart, string fixedPartSec);

        DTODoctor GetExisting(Int64 code);
    }
}
