using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using SpyStore.Models.Entities;
using SpyStore.Service.Tests.TestClasses.Base;
using Xunit;


namespace SpyStore.Service.Tests.TestClasses
{
    [Collection("Service Testing")]
    public class CustomerControllerTests: BaseTestClass
    {
        public CustomerControllerTests()
        {
            RootAddress = "api/customer";
        }

        [Fact]
        public async void ShouldGetAllCustomers()
        {
            Console.WriteLine(RootAddress);
            Console.WriteLine(ServiceAddress);
            // Get All Customers
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonReponse = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(jsonReponse);
                Assert.Single(customers);

            }
        }

        [Fact]
        public async void ShouldGetOneCustomer()
        {
            // Get One customer (0)
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/0");
                Assert.True(response.IsSuccessStatusCode);
                var jsonReponse = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(jsonReponse);
                Assert.Equal("Super Spy", customer.FullName);

            }
        }

    }
}
