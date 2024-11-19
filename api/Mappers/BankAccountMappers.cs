using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class BankAccountMappers
    {
        public static BankAccountDto ToBankAccountDto(this BankAccount bankAccountDto) 
        {
            return new BankAccountDto
            {
                AccountNumber = bankAccountDto.AccountNumber,
                Balance = bankAccountDto.Balance
            };
        }
    }
}
