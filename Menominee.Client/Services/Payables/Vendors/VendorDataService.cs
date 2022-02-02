﻿using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.Vendors
{
    public class VendorDataService : IVendorDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/payables/vendors";

        public VendorDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<VendorToRead> AddVendor(VendorToWrite vendor)
        {
            var content = new StringContent(JsonSerializer.Serialize(vendor), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{vendor.Name} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<VendorToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"{vendor.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<VendorToReadInList>> GetAllVendors()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<VendorToReadInList>>($"{UriSegment}/listing");
                //return await httpClient.GetFromJsonAsync<IReadOnlyList<VendorToReadInList>>("GetVendorsListAsync");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
                //Console.WriteLine($"BaseAddress: {httpClient.BaseAddress}");
            }

            return null;
        }

        public async Task<VendorToRead> GetVendor(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<VendorToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
            return null;
        }

        public async Task UpdateVendor(VendorToWrite vendor, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(vendor), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{vendor.Name} updated successfully", "Saved");
                return;
            }

            toastService.ShowError($"{vendor.Name} failed to update", "Save Failed");
        }
    }
}