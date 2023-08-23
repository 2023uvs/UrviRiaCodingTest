using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace RESTServerSampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private List<Customer> customers = new List<Customer>();
        private string jsonFilePath = "customers.json";

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {

            _logger = logger;
        }
        //Adding Customers
        [HttpPost]
        public List<Customer> AddCustomers(List<Customer> customerList)
        {
            try
            {
                // Find the index to insert to maintain sorting
                foreach (var newcustomer in customerList)
                {
                    ValidateAndAddCustomer(newcustomer);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           return customers;
        }
        private void ValidateAndAddCustomer(Customer newCustomer)
        {
            if (newCustomer == null || string.IsNullOrWhiteSpace(newCustomer.FirstName) || string.IsNullOrWhiteSpace(newCustomer.LastName) || newCustomer.Age <= 18 || newCustomer.Id <= 0 ||
                customers.Any(c => c.Id == newCustomer.Id))
            {
                throw new Exception("Invalid customer data");
            }
            //Reading from file.
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            List<Customer> loadedCustomers = JsonConvert.DeserializeObject<List<Customer>>(jsonContent);

            //Inserting at correct index
            int insertIndex;
            if (loadedCustomers != null && loadedCustomers.Count > 0)
            {
                insertIndex = FindInsertIndex(loadedCustomers, newCustomer);
                loadedCustomers.Insert(insertIndex, newCustomer);
            }
            else
            {
                loadedCustomers = new List<Customer>();
                loadedCustomers.Insert(0, newCustomer);
            }
            
            //Writing into the file
            customers = loadedCustomers;
            System.IO.File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(loadedCustomers, Formatting.Indented));

        }

        private int FindInsertIndex(List<Customer> customers, Customer newCustomer)
        {
            int index = 0;

            if (customers != null && customers.Count > 0)
            {
                foreach (var customer in customers)
                {
                    //Comparing last names
                    int lastNameComparison = string.Compare(customer.LastName, newCustomer.LastName, StringComparison.OrdinalIgnoreCase);
                    if (lastNameComparison > 0 || (lastNameComparison == 0 && string.Compare(customer.FirstName, newCustomer.FirstName, StringComparison.OrdinalIgnoreCase) > 0))
                    {
                        return index;
                    }
                    index++;
                }
            }
            return index;
        }

        //Getting Customers
        [HttpGet(Name = "GetCustomers")]
        public List<Customer> GetCustomers()
        {
            LoadCustomersFromFile();
            return customers;
        }


        private void LoadCustomersFromFile()
        {
            // Load customers from file if available
            if (System.IO.File.Exists(jsonFilePath))
            {
                string json = System.IO.File.ReadAllText(jsonFilePath);
                customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            }
        }
    }
}


