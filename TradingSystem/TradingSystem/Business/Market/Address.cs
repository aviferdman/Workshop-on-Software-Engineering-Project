namespace TradingSystem.Business.Market
{
    public class Address
    {
        private string _state;
        private string _city;
        private string _street;
        private string _apartmentNum;

        public Address(string state, string city, string street, string apartmentNum)
        {
            this._state = state;
            this._city = city;
            this._street = street;
            this._apartmentNum = apartmentNum;
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