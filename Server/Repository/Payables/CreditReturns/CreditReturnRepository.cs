using MenomineePlayWASM.Client.Helpers;
using MenomineePlayWASM.Shared.Entities.Payables.CreditReturns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
{
    public class CreditReturnRepository : ICreditReturnRepository
    {
        ////private readonly IHttpService httpService;
        //private string url = "api/returns";

        ////public CreditReturnRepository(IHttpService httpService)
        ////{
        ////    this.httpService = httpService;
        ////}

        //public async Task<int> CreateReturnAsync(CreditReturn creditReturn)
        //{
        //    var response = await httpService.Post<CreditReturn, int>(url, creditReturn);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }

        //    return response.Response;
        //}

        //public async Task<CreditReturn> GetReturnAsync(int id)
        //{
        //    return await httpService.GetHelper<CreditReturn>($"{url}/{id}");
        //}

        //public async Task<List<CreditReturn>> GetReturnsAsync()
        //{
        //    return await httpService.GetHelper<List<CreditReturn>>(url);
        //}

        //public async Task UpdateReturnAsync(CreditReturn creditReturn)
        //{
        //    var response = await httpService.Put(url, creditReturn);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}
    }
}
