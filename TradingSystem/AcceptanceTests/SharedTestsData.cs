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
            new CreditCard
            (
                cardNumber: "1111111111111111",
                month: "02",
                year: "24",
                holderName: "User Dinner",
                cvv: "123",
                holderId: "147852369"
            ),
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
            new CreditCard
            (
                cardNumber: "7984652132140000",
                month: "08",
                year: "32",
                holderName: "Buyer Chicken",
                cvv: "452",
                holderId: "123478965"
            ),
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
            "buyer_2",
            "buyer_2",
            "05002222222",
            new CreditCard
            (
                cardNumber: "4444333322221111",
                month: "12",
                year: "25",
                holderName: "Buyer Buyer",
                cvv: "456",
                holderId: "1597532486"
            ),
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
            new CreditCard
            (
                cardNumber: "8888888899999999",
                month: "10",
                year: "22",
                holderName: "Owner Balben",
                cvv: "888",
                holderId: "753214983"
            ),
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
            new CreditCard
            (
                cardNumber: "7777888899990000",
                month: "05",
                year: "27",
                holderName: "Bob alice",
                cvv: "444",
                holderId: "300158710"
            ),
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
