using System;

namespace LoLTournaments.Shared.Abstractions
{

    public interface ISharedTime
    {
        DateTime Current { get; }
    }

}