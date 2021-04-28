namespace AcceptanceTests.AppInterface.Data
{
    public struct BankAccount
    {
        public BankAccount(int branch, int accountNumber)
        {
            Branch = branch;
            AccountNumber = accountNumber;
        }

        public int Branch { get; set; }
        public int AccountNumber { get; set; }

        public override string ToString()
        {
            return @$"BankMethod:\{AccountNumber}\{Branch}";
        }
    }
}
