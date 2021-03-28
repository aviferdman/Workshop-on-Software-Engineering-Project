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

        public string State { get => _state; set => _state = value; }
        public string City { get => _city; set => _city = value; }
        public string Street { get => _street; set => _street = value; }
        public string ApartmentNum { get => _apartmentNum; set => _apartmentNum = value; }

        public override string ToString()
        {
            return State + "\\" + City + "\\" + Street + "\\" + ApartmentNum;
        }
    }
}