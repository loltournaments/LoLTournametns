using System;

namespace LoLTournaments.Shared.Models
{

    public struct SessionTimer
    {
        public DateTime End { get; set; }
        public TimerState State { get; set; }
    }

}