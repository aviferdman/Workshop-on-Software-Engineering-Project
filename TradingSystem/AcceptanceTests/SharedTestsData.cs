using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests
{
    /// <summary>
    /// Manages all of the shared data in one place
    /// </summary>
    public static class SharedTestsData
    {
        public static readonly BuyerUserInfo User_Other = new BuyerUserInfo
        (
            "user123",
            "mypassword1",
            "0539876543",
            new BankAccount(branch: 1, accountNumber: 9),
            new Address
            {
                State = "Israel",
                City = "Ramat Gan",
                Street = "Bialik",
                ApartmentNum = "54",
                ZipCode = "584268",
            }
        );

        public static readonly BuyerUserInfo User_Buyer = new BuyerUserInfo
        (
            "buyer78",
            "buyer_pass78",
            "0500000000",
            new BankAccount(branch: 3, accountNumber: 10),
            new Address
            {
                State = "Israel",
                City = "Tel-Aviv",
                Street = "Rot Shield",
                ApartmentNum = "5d",
                ZipCode = "58426300",
            }
        );

        public static readonly BuyerUserInfo User_Buyer2 = new BuyerUserInfo
        (
            "buyer78",
            "buyer_pass78",
            "0500000000",
            new BankAccount(branch: 6, accountNumber: 7),
            new Address
            {
                State = "Israel",
                City = "Tel-Aviv",
                Street = "Rot Shield",
                ApartmentNum = "5d",
                ZipCode = "58426300",
            }
        );

        public static readonly UserInfo User_ShopOwner1 = new UserInfo
        (
            "shopowner78",
            "shopowner_pass78",
            "05200000001",
            new Address
            {
                State = "Israel",
                City = "Beer-Sheva",
                Street = "Rager",
                ApartmentNum = "100",
                ZipCode = "7563187",
            }
        );

        public static readonly UserInfo User_ShopOwner2 = new UserInfo
        (
            "nymn1",
            "go5minutes",
            "0540123456",
            new Address
            {
                State = "Sviden",
                City = "forsenGa",
                Street = "Kappa",
                ApartmentNum = "123",
                ZipCode = "123456",
            }
        );

        public static readonly ShopInfo Shop1 = new ShopInfo
        (
            name: "my shop 1",
            bankAccount: new BankAccount(branch: 2, accountNumber: 8),
            address: new Address
            {
                State = "U.S.",
                City = "Los Santos",
                Street = "The Hood",
                ApartmentNum = "8",
                ZipCode = "1547623",
            }
        );

        public static readonly ShopInfo Shop2 = new ShopInfo
        (
            name: "another shop",
            bankAccount: new BankAccount(branch: 1, accountNumber: 2),
            address: new Address
            {
                State = "Israel",
                City = "Ramat-Gan",
                Street = "HaYarden",
                ApartmentNum = "67",
                ZipCode = "53288742",
            }
        );
    }
}
