using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models.CreditCards;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.CreditCards
{
    public class CreditCardDataService : ICreditCardDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/creditcards";

        public CreditCardDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<CreditCardToRead> AddCreditCardAsync(CreditCardToWrite creditCard)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(creditCard), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                CreditCardToRead cc = await JsonSerializer.DeserializeAsync<CreditCardToRead>(await response.Content.ReadAsStreamAsync(), options);
                return cc;
            }

            toastService.ShowError($"Failed to add credit card. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<CreditCardToReadInList>> GetAllCreditCardsAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<CreditCardToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception)
            {
                // TODO: log exception
            }

            return null;
        }

        public async Task<CreditCardToRead> GetCreditCardAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<CreditCardToRead>($"{UriSegment}/{id}");
            }
            catch (Exception)
            {
                // TODO: log exception
            }
            return null;
        }

        public async Task UpdateCreditCardAsync(CreditCardToWrite creditCard, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(creditCard), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Credit Card saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Credit Card failed to update:  Id = {id}", "Save Failed");
        }
    }
}
