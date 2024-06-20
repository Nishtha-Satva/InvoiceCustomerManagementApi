using DataAccessLayer.Model;
using DataAccessLayer.Services;
using InvoiceCustomerManagementApi.CommonJsonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace InvoiceCustomerManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceInterface invoiceInterface;
        private readonly IMongoCollection<Invoice> invoiceCollection;

        public InvoiceController(IInvoiceInterface invoiceInterface, IMongoCollection<Invoice> invoiceCollection)
        {
            this.invoiceInterface = invoiceInterface;
            this.invoiceCollection = invoiceCollection;
        }
        [HttpGet]
        [Route("getInvoices")]
        public async Task<ActionResult> GetInvoices()
        {
            var objCommonJson = new CommonResponse();

            try
            {
                var invoiceList = invoiceInterface.InvoiceListAsync();
                if (invoiceList != null)
                {
                    objCommonJson.Message = "Records found successfully";
                }
                else
                {
                    objCommonJson.Message = "Records not found!";
                }
                objCommonJson.ResponseStatus = 1;
                objCommonJson.Result = invoiceList;
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
        [Route("CreateInvoice")]
        public async Task<ActionResult> Post(Invoice invoice)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var existingItem = await invoiceInterface.FindInvoiceNumberAsync(invoice.InvoiceNumber);
                    if (existingItem != null)
                    {
                        objCommonJson.ResponseStatus = 0;
                        objCommonJson.Message = "Invoice number must be unique";
                        return Ok(objCommonJson);
                    }
                    await invoiceInterface.AddInvoice(invoice);
                    objCommonJson.ResponseStatus = 1;
                    objCommonJson.Message = "Invoice added successfully!";
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
        [Route("updateInvoice/{id}")]
        public async Task<ActionResult> UpdateInvoice(string id, Invoice invoice)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                var existingItem = invoiceInterface.GetInvoiceById(invoice.Id);
                if (existingItem == null)
                {
                    objCommonJson.ResponseStatus = 0;
                    objCommonJson.Message = "Invoice doesn't exist";
                    return Ok(objCommonJson);
                }
                await invoiceInterface.UpdateInvoiceAsync(id, invoice);
                objCommonJson.ResponseStatus = 1;
                objCommonJson.Message = "Record updated successfully!";
                objCommonJson.Result = invoice;
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

        [HttpPatch]
        [Route("{id}/Status")]
        public async Task<ActionResult> UpdateInvoiceStatus(string id, Invoice invoice)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                await invoiceInterface.UpdateStatusAsnyc(id, invoice);
                objCommonJson.ResponseStatus = 1;
                objCommonJson.Message = "Record updated successfully!";
                objCommonJson.Result = invoice.Status;
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
        [Route("DeleteInvoice/{id}")]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            var objCommonJson = new CommonResponse();
            try
            {
                var existingItem = invoiceInterface.GetInvoiceById(id);
                if (existingItem == null)
                {
                    objCommonJson.ResponseStatus = 0;
                    objCommonJson.Message = "Invoice doesn't exist";
                    return Ok(objCommonJson);
                }
                await invoiceInterface.DeleteInvoiceAsync(id);
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
