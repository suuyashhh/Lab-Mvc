using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DTO.Masters.Interfaces
{
    public interface IMstCasePaper
    {
        List<DTOCasePaper> GetAll();
        Int64 Create(DTOCasePaper _objDtoCasePaper);
        Task<string> GetLastPatientIdForDate(string datePart);
        DTOCasePaper GetExisting(Int64 code);
    }
}
