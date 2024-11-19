using api.Dtos;

namespace api.Mappers
{
    public static class BankAccountMappers
    {
        public static BankAccountDto ToBankAccountDto(this BankAccountDto bankAccountDto) 
        {
            return new BankAccountDto
            {
                AccountNumber = bankAccountDto.AccountNumber,
                Balance = bankAccountDto.Balance
            };
        }
    }
}
