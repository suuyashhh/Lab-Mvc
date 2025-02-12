using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab.DTO.Masters.Objects;
using Lab.DTO.Masters.Interfaces;
using System.Threading.Tasks;
using Lab.DALDapper.Implimantation.Masters;

namespace Lab.Businesss.Masters
{
    public class CasePaper
    {
        public static IMstCasePaper _dalCasePaper;

        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Doctor { get; set; }
        public string Date { get; set; }

        public static CasePaper New()
        {
            try
            {
                return new CasePaper();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Failed. " + ex.Message);
            }
        }

        public static async Task<int> Create(CasePaper _ObjCsPaper)
        {
            try
            {
                int result = 0;
                _dalCasePaper = new DALCasePaper();
                
                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    PATIENT_ID = _ObjCsPaper.PatientId,
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    GENDER = _ObjCsPaper.Gender,
                    CONTACT = _ObjCsPaper.Contact,
                    ADDRESS = _ObjCsPaper.Address,
                    DOCTOR = _ObjCsPaper.Doctor,
                    DATE = _ObjCsPaper.Date
                };

                result = await Task.Run(() => { return _dalCasePaper.Create(_objDtoCasePaper); });
                return result;
            }
            catch
            {
                return 0;
            }
        }

    }
}
