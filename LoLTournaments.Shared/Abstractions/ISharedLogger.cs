namespace LoLTournaments.Shared.Abstractions
{

    public interface ISharedLogger
    {
        void Error(object value);
        void Warning(object value);
        void Log(object value);
    }

}