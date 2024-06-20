using DataAccessLayer.Services;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using InvoiceCustomerManagementApi.CommonJsonResponse;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceCustomerManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerInterface customerInterface;
        private readonly IMongoCollection<DynamicJsonResponse> customerCollection;

        public CustomerController(ICustomerInterface customerInterface, IMongoCollection<DynamicJsonResponse> customerCollection)
        {
            this.customerInterface = customerInterface;
            this.customerCollection = customerCollection;
        }

        [HttpGet]
        [Route("getcustomers")]
        public async Task<ActionResult> GetCustomers()
        {
            var objCommonJson = new CommonResponse();

            try
            {
                var customerList = customerInterface.CustomerListAsync();
                if (customerList != null)
                {
                    objCommonJson.Message = "Records found successfully";
                }
                else
                {
                    objCommonJson.Message = "Records not found!";
                }
                objCommonJson.ResponseStatus = 1;
                objCommonJson.Result = customerList;
            }
            catch (Exception ex)
            {
                objCommonJson.ResponseStatus = 0;
                objCommonJson.Message = ex.Message;

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    objCommonJson.Message = ex.InnerException.Message;
                }
            }
            return Ok(objCommonJson);
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<ActionResult> Post(Customer customer)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                if (ModelState.IsValid)
                {

                    await customerInterface.AddCustomer(customer);
                    objCommonJson.ResponseStatus = 1;
                    objCommonJson.Message = "Record added successfully!";
                    objCommonJson.Result = customer;
                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Any())
                                           .ToDictionary(
                                                kvp => kvp.Key,
                                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                                            );
                    objCommonJson.ResponseStatus = 0;
                    objCommonJson.Message = "Validation failed. Please check the errors.";
                    objCommonJson.Result = errors;
                }
            }
            catch (Exception ex)
            {

                objCommonJson.ResponseStatus = 0;
                objCommonJson.Message = ex.Message;

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    objCommonJson.Message = ex.InnerException.Message;
                }
            }
            return Ok(objCommonJson);
        }

        [HttpPut]
        [Route("updateCustomer/{id}")]
        public async Task<ActionResult> UpdateCustomer(string id, Customer customer)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                await customerInterface.UpdateCustomer(id, customer);
                objCommonJson.ResponseStatus = 1;
                objCommonJson.Message = "Record updated successfully!";
                objCommonJson.Result = customer;
            }
            catch (Exception ex)
            {

                objCommonJson.ResponseStatus = 0;
                objCommonJson.Message = ex.Message;

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    objCommonJson.Message = ex.InnerException.Message;
                }
            }
            return Ok(objCommonJson);
        }

        [HttpDelete]
        [Route("DeleteCustomer/{id}")]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                await customerInterface.DeleteCustomer(id);
                objCommonJson.ResponseStatus = 1;
                objCommonJson.Message = "Record deleted successfully!";
            }
            catch (Exception ex)
            {

                objCommonJson.ResponseStatus = 0;
                objCommonJson.Message = ex.Message;

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    objCommonJson.Message = ex.InnerException.Message;
                }
            }
            return Ok(objCommonJson);
        }
    }
}
