using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests
{
    /// <summary>
    /// Manages all of the shared data in one place
    /// </summary>
    public static class SharedTestsData
    {
        public static readonly UserInfo User_Buyer = new UserInfo
        (
            "buyer78",
            "buyer_pass78",
            "0500000000",
            new Address
            {
                State = "Israel",
                City = "Tel-Aviv",
                Street = "Rot Shield",
                ApartmentNum = "5d",
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
                ApartmentNum = "123"
            }
        );

        public static readonly UserInfo User_Other = new UserInfo
        (
            "user123",
            "mypassword1",
            "0539876543",
            new Address
            {
                State = "Israel",
                City = "Ramat Gan",
                Street = "Bialik",
                ApartmentNum = "54",
            }
        );

        public const string SHOP_NAME = "my shop 1";
        public const string SHOP_NAME_2 = "another shop";
    }
}
