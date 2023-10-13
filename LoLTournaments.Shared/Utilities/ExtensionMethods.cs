namespace LoLTournaments.Shared.Utilities
{

    public static class ExtensionMethods
    {
        public static bool HasAnyFlags(this Enum value, Enum flags)
        {
            return value != null
                   && flags != null
                   && (Convert.ToInt32(value) & Convert.ToInt32(flags)) != 0;
        }

        public static bool HasAllFlags(this Enum value, Enum flags)
        {
            return value != null
                   && flags != null
                   && (Convert.ToInt32(value) & Convert.ToInt32(flags)) == Convert.ToInt32(flags);
        }
    }

}