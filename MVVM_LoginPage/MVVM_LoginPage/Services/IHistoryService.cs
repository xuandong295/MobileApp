using MVVM_LoginPage.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_LoginPage.Services
{
    interface IHistoryService
    {
        Task<bool> CreateHistory(HistoryModel led);
        Task<bool> DeleteHistory(int id);
        Task<bool> RefreshHistory(int id, HistoryModel led);
        Task<List<HistoryModel>> GetHistoryData();
    }
}
