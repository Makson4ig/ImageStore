using LinqToDB.Configuration;
using log4net;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace ImageStore.Authorization
{
    class ConnectionSettings
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionSettings));
        public class ConnectionStringSettings : IConnectionStringSettings
        {
            public string ConnectionString { get; set; }
            public string Name { get; set; }
            public string ProviderName { get; set; }
            public bool IsGlobal => true;
        }
        
        public class ImageStoreIternetSettings : ILinqToDBSettings
        {
            public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

            public string DefaultConfiguration => "ImageStore";
            public string DefaultDataProvider => "System.Data.SqlClient";

            public IEnumerable<IConnectionStringSettings> ConnectionStrings
            {
                get
                {
                    yield return
                        new ConnectionStringSettings
                        {
                            Name = "ImageStore",
                            ProviderName = "System.Data.SqlClient",
                            ConnectionString = @"Data Source=" + new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).DataSource.ToString() + ";" +
                            "Initial Catalog=" + new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).Database.ToString() + ";" +
                            "User ID=u0718435_Maxim;" +
                            "Password=Q1w2e3r4t5"
                        };
                }
            }
        }

        public class ImageStoreSettings : ILinqToDBSettings
        {
            public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

            public string DefaultConfiguration => "ImageStore";
            public string DefaultDataProvider => "System.Data.SqlClient";

            public IEnumerable<IConnectionStringSettings> ConnectionStrings
            {
                get
                {
                    yield return
                        new ConnectionStringSettings
                        {
                            Name = "ImageStore",
                            ProviderName = "System.Data.SqlClient",
                            ConnectionString = 
                            @"Data Source="+ new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).DataSource.ToString() + ";" +
                            "Database =" + new SqlConnection(ConfigurationManager.ConnectionStrings["ImageStore"].ConnectionString).Database.ToString() + ";" +
                            "User ID=DipAnonim2;" +
                            "Password=Q1w2e3r4"
                        };
                }
            }
        }

        
    }
}
