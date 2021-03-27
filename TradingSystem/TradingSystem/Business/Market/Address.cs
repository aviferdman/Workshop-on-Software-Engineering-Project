namespace TradingSystem.Business.Market
{
    class Address
    {
        private string _state;
        private string _city;
        private string _street;
        private string _apartmentNum;

        public Address()
        {

        }

        public override string ToString()
        {
            return _state + "\\" + _city + "\\" + _street + "\\" + _apartmentNum;
        }
    }
}