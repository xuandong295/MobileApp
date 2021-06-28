using MVVM_LoginPage.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_LoginPage.Services
{
    interface ILedService
    {
        Task<bool> CreateLed(LedModel led);
        Task<bool> DeleteLed(int id);
        Task<bool> RefreshLed(int id, LedModel led);
        Task<List<LedModel>> GetLedData();
    }
}
