using System;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Common
{

    public class SharedTime : ISharedTime
    {
        public virtual DateTime Current => DateTime.Now;
    }

}