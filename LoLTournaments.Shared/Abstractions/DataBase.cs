using System;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Shared.Abstractions
{
    public abstract class DataBase : IIdentity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public override string ToString()
        {
            return this.ReflectionFormat();
        }

        public override bool Equals(object obj)
        {
            return obj is DataBase other && Equals(other);
        }

        protected virtual bool Equals(DataBase other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}