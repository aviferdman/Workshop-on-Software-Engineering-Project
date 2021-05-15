namespace TradingSystem.Business.Market
{
    public class Address
    {
        private string _state;
        private string _city;
        private string _street;
        private string _apartmentNum;
        private string _zip;

        public Address(string state, string city, string street, string apartmentNum, string zip)
        {
            this._state = state;
            this._city = city;
            this._street = street;
            this._apartmentNum = apartmentNum;
            this.Zip = zip;
        }

        public string State { get => _state; set => _state = value; }
        public string City { get => _city; set => _city = value; }
        public string Street { get => _street; set => _street = value; }
        public string ApartmentNum { get => _apartmentNum; set => _apartmentNum = value; }
        public string Zip { get => _zip; set => _zip = value; }

        public override string ToString()
        {
            return State + "\\" + City + "\\" + Street + "\\" + ApartmentNum + "\\" + Zip;
        }
    }
}