﻿using Lab.DTO.Masters.Objects;
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
        Int64 Edit(DTOCasePaper _objDtoCasePaper); 
        Int64 Delete(DTOCasePaper _objDtoCasePaper);
        Int64 InvoiceSave(DTOCasePaper _objDtoCasePaper); 
        Int64 Approve(DTOCasePaper _objDtoCasePaper);
        int Approve(string TrnNos);
        List<DTOCasePaper> GetApprovalPendingList(string comid);
        Task<string> GetLastPatientIdForDate(string dateComboKey);
        Task<string> GetLastInvoiceNoAsync(string comid);
        DTOCasePaper GetExisting(Int64 code);
        Task<List<DTOCasePaper>> GetDateWiseAllAsync(string strStartDate, string strEndDate, string ComId);
        Task<int> GetCountByDate(string currentDate,string comid);
        Task<int> GetCountApprovePending(string comid);
    }
}
