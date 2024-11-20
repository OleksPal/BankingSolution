using api.Dtos;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
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
            BankAccountDto bankAccount;

            try
            {
                bankAccount = await _bankAccountService.GetBankAccountByNumber(number);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok(bankAccount);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankAccount(decimal initialBalance)
        {
            BankAccountDto newBankAccountDto;

            try
            {
                newBankAccountDto = await _bankAccountService.CreateAccount(initialBalance);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok(newBankAccountDto);
        }

        [HttpPut]
        public async Task<IActionResult> Deposit(string number, decimal amount)
        {
            BankAccountDto recipientAccount;

            try
            {
                recipientAccount = await _bankAccountService.Deposit(number, amount);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok(recipientAccount);
        }

        [HttpPut]
        public async Task<IActionResult> Withdraw(string number, decimal amount)
        {
            BankAccountDto recipientAccount;

            try
            {
                recipientAccount = await _bankAccountService.Withdraw(number, amount);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok(recipientAccount);
        }

        [HttpPut]
        public async Task<IActionResult> Transfer(string sender, string recipient, decimal amount)
        {
            BankAccountDto recipientAccount;

            try
            {
                recipientAccount = await _bankAccountService.Transfer(sender, recipient, amount);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok(recipientAccount);
        }
    }
}
