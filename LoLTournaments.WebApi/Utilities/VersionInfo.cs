using System;
using System.Reflection;

namespace LoLTournaments.WebApi.Utilities
{

    public static class VersionInfo
    {
        public const string APIVersion = "v1";

        public static string SolutionName = Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly string APP_INSTANCE_ID = $"Server-{Guid.NewGuid().ToString()}";
    }

}