﻿using System;
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

        public int TrnNo { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string ConNumber { get; set; }
        public string Address { get; set; }
        public string DoctorRef { get; set; }
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

        public static async Task<Int64> Create(CasePaper _ObjCsPaper)
        {
            try
            {
                Int64 result = 0;
                _dalCasePaper = new DALCasePaper();

                // Generate Unique Patient ID as Int64
                string datePart = DateTime.Now.ToString("yyyyMMdd"); // Get YYYYMMDD format
                Int64 newPatientId = await GeneratePatientId(datePart);

                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = newPatientId, // Store as Int64
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    GENDER = _ObjCsPaper.Gender,
                    CON_NUMBER = _ObjCsPaper.ConNumber,
                    DOCTOR_REF = _ObjCsPaper.DoctorRef
                };

                result = await Task.Run(() => { return _dalCasePaper.Create(_objDtoCasePaper); });
                return result;
            }
            catch
            {
                return 0;
            }
        }


        // Function to Generate Unique Patient ID
        private static async Task<long> GeneratePatientId(string datePart)
        {
            _dalCasePaper = new DALCasePaper();

            // Define the fixed part "02"
            string fixedPart = "02";

            // Get the last inserted Patient ID for today
            string lastId = await _dalCasePaper.GetLastPatientIdForDate(datePart);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(datePart + fixedPart))
            {
                int lastNumber = int.Parse(lastId.Substring(10)); // Extract the last 3 digits
                nextNumber = lastNumber + 1;
            }

            // Convert to Int64
            long newPatientId = long.Parse(datePart + fixedPart + nextNumber.ToString("D3"));

            return newPatientId;
        }




    }
}
