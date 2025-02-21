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
        Int64 Create(DTOTest _objTestDetails);
        Task<List<DTOTest>> GetTestsAsync(string searchtext);
    }
}
