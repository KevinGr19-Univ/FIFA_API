namespace FIFA_API
{
    public static class API
    {
        private static WebApplication _app;
        public static WebApplication App
        {
            get => _app;
            set
            {
                if (_app is null) _app = value;
            }
        }


    }
}
