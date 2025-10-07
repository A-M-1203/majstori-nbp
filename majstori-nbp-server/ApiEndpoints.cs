public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class V1
    {
        private const string VersionBase = $"{ApiBase}/v1";

        public static class Majstori
        {
            private const string Base = $"{VersionBase}/majstori";

            public const string GetAll = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
        }
    }
}