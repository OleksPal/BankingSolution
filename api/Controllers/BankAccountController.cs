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

        [HttpGet]
        public async Task<IActionResult> GetBankAccount(string number)
        {
            var bankAccount = await _bankAccountService.GetBankAccountByNumber(number);

            if (bankAccount is null)
                return NotFound();

            return Ok(bankAccount);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankAccount(decimal initialBalance)
        {
            var newBankAccount = await _bankAccountService.CreateAccount(initialBalance);

            if (newBankAccount is null)
                return BadRequest();

            return Ok(newBankAccount);
        }

        [HttpPut]
        public async Task<IActionResult> Deposit(string number, decimal amount)
        {
            var recipientAccount = await _bankAccountService.Deposit(number, amount);

            if (recipientAccount is null)
                return BadRequest();

            return Ok(recipientAccount);
        }

        [HttpPut]
        public async Task<IActionResult> Withdraw(string number, decimal amount)
        {
            var recipientAccount = await _bankAccountService.Withdraw(number, amount);

            if (recipientAccount is null)
                return BadRequest();

            return Ok(recipientAccount);
        }

        [HttpPut]
        public async Task<IActionResult> Transfer(string sender, string recipient, decimal amount)
        {
            var recipientAccount = await _bankAccountService.Transfer(sender, recipient, amount);

            if (recipientAccount is null)
                return BadRequest();

            return Ok(recipientAccount);
        }
    }
}
