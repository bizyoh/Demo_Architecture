using Application.Interfaces.InvoiceService;
using Application.Models.DTO.InvoiceDto;
using Microsoft.AspNetCore.Mvc;


namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Invoicescontroller : ControllerBase
    {
        private IInvoiceService invoiceService;
        public Invoicescontroller(IInvoiceService _invoiceService)
        {
            invoiceService = _invoiceService;
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateInvoiceDto invoice)
        {
            if (invoiceService.Create(invoice))
            {
                return Ok();
            }
          return BadRequest();
        }


        [HttpGet("user/{id}")]
        public IActionResult GetAllInvoiceUserViewModelByUserId(int id)
        {
            var invoices = invoiceService.GetAllInvoiceUserViewModelByUserId(id);
            
            return Ok(invoices);
        }
        [HttpGet]
        public IActionResult GetAllInvoiceUserViewModel()
        {
            var invoices = invoiceService.GetAllInvoiceUserViewModel();

            return Ok(invoices);
        }



        [HttpGet("{id}/detail")]
        public IActionResult GetAllInvoiceUserDetailViewModel(int id)
        {
            var invoices = invoiceService.GetAllInvoiceDetailUserViewModel(id);
            return Ok(invoices);
        }
    }
}
