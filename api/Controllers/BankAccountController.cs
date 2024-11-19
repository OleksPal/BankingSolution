using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bankAccounts = await _bankAccountService.GetAll();

            return Ok(bankAccounts);
        }

        [HttpGet("{number}")]
        public async Task<IActionResult> GetBankAccount(string number)
        {
            var bankAccount = await _bankAccountService.GetBankAccountByNumber(number);

            if (bankAccount is null)
                return NotFound();

            return Ok(bankAccount);
        }
    }
}
