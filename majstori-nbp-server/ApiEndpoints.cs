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
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string Emails = $"{Base}/emails";
        }

        public static class Klijenti
        {
            private const string Base = $"{VersionBase}/klijenti";

            public const string GetAll = Base;
            public const string GetById = $"{Base}/{{id}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{id}}";
            public const string Delete = $"{Base}/{{id}}";
            public const string Emails = $"{Base}/emails";
        }

        public static class Auth
        {
            private const string Base = $"{VersionBase}/auth";

            public const string RegisterMajstor = $"{Base}/register/majstor";
            public const string RegisterKlijent = $"{Base}/register/klijent";
            public const string Login = $"{Base}/login";
        }
    }
}