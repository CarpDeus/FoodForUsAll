using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Integration.Tests
{
    public class DbConnectionTest
    {
        public DbConnectionTest()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            _foodForUsAllConnectionString = root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        [Test]
        public void OpenFoodForUsAllDatabaseConnection()
        {
            //Arrange
            SqlConnection conn = new SqlConnection(_foodForUsAllConnectionString);

            //Act
            conn.Open();

            //Assert
            Assert.That(conn.State == ConnectionState.Open);
            conn.Close();
        }

        #region private

        readonly string _foodForUsAllConnectionString;

        #endregion
    }
}