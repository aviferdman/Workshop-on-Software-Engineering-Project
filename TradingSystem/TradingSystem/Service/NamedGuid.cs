using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Service
{
    public class NamedGuid
    {
        private Guid _id;
        private string _name;

        public NamedGuid(Guid id, string name)
        {
            this._id = id;
            this._name = name;
        }

        public Guid Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public override bool Equals(object obj)
        {
            return obj is NamedGuid guid &&
                   _id.Equals(guid._id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }
    }
}
