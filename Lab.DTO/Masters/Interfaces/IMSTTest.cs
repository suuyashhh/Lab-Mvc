using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lab.DTO.Masters.Interfaces
{
    public interface IMSTTest
    {
        List<DTOTest> GetAll(string comid);
        Int64 Create(DTOTest _objDtoTest);
        Int64 Edit(DTOTest _objDtoTest);
        Int64 Delete(DTOTest _objDtoTest);
        Task<List<DTOTest>> GetTestsAsync(string searchtext, string comid);
        Task<string> GetLastTestIdForFixedParts(string fixedPart, string fixedPartSec);
        DTOTest GetExisting(Int64 code);
    }
}
