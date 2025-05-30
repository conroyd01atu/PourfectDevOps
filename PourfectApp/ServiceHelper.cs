using PourfectApp.Services;

namespace PourfectApp
{
    public static class ServiceHelper
    {
        private static DatabaseService _database;

        public static DatabaseService Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new DatabaseService();
                }
                return _database;
            }
        }
    }
}