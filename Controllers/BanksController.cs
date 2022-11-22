using banks.Models;
using banks.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace banks.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BanksController : ControllerBase
    {
        private readonly IBankServices bankServices;

        public BanksController(IBankServices bankServices)
        {
            this.bankServices = bankServices;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Bank>>>> Get()
        {
            var response = new ApiResponse<List<Bank>>();
            try
            {
                await this.bankServices.LoadHtmlDocument();
                response.Data = this.bankServices.GetBank();
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("Detail")]
        public async Task<ActionResult<ApiResponse<BankDetail>>> GetDetail(string URL)
        {
            var response = new ApiResponse<BankDetail>();
            try
            {
                await this.bankServices.LoadHtmlDocument(URL);
                response.Data = this.bankServices.GetBankDetail();
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
