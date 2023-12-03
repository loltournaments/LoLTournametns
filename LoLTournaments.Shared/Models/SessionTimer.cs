using System;

namespace LoLTournaments.Shared.Models
{

    public class SessionTimer
    {
        public virtual DateTime End { get; set; }
        public TimerState State { get; set; }
    }

}