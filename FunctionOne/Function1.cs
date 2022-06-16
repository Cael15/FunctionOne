using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace FunctionOne
{
    public class Function1
    {
        [Function("Funcion2ColaAzure")]
        public void Run([ServiceBusTrigger("micola")]string myQueueItem, FunctionContext context)
        {
            var logger = context.GetLogger("Function1");
            logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var client = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientEntity>(myQueueItem);
            if (IsValidClient(client))
            {
                CreateClient(client);
                logger.LogInformation($"Client with email: {client.Email} recien crado");
            }
            else
            {
                logger.LogInformation($"Client with email: {client.Email} already exist. Client was not created");
            }
        }

        private static bool IsValidClient(ClientEntity client)
        {
            var clientFound = GetClientByEmail(client.Email);
            if (clientFound == null) return true;
            return false;
        }

        private static ClientEntity GetClientByEmail(string email)
        {
            var client = new ClientEntity();
            try
            {
                SqlConnectionStringBuilder connectionStringBuilder = new();
                connectionStringBuilder.DataSource = "colas-dataserver-1.database.windows.net";
                connectionStringBuilder.InitialCatalog = "colas-database";
                connectionStringBuilder.IntegratedSecurity = false;
                connectionStringBuilder.UserID = "DanielDelOrt";
                connectionStringBuilder.Password = "C@eli15042019";
                var cs = connectionStringBuilder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(cs))
                {
                    connection.Open();
                    var query = $"Select * from client where email = '{email}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = command.ExecuteReader();
                    if (!reader.HasRows) return null;

                    while (reader.Read())
                    {
                        client = new ClientEntity()
                        {
                            Id = (string)reader["id"],
                            IsActive = (bool)reader["isActive"],
                            Age = (int)reader["age"],
                            Name = (string)reader["name"],
                            Gender = (string)reader["gender"],
                            Company = (string)reader["company"],
                            Email = (string)reader["email"],
                            Phone = (string)reader["phone"],
                            Address = (string)reader["address"]
                        };
                    }

                }
            }
            catch (Exception e)
            {
                //logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            }

            return client;
        }
        private static void CreateClient(ClientEntity client)
        {
            try
            {
                var result = client.IsActive == true ? 1 : 0;

                SqlConnectionStringBuilder connectionStringBuilder = new();
                connectionStringBuilder.DataSource = "colas-dataserver-1.database.windows.net";
                connectionStringBuilder.InitialCatalog = "colas-database";
                connectionStringBuilder.IntegratedSecurity = false;
                connectionStringBuilder.UserID = "DanielDelOrt";
                connectionStringBuilder.Password = "C@eli15042019";
                var cs = connectionStringBuilder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(cs))
                {
                    connection.Open();
                    var query = $"INSERT INTO [dbo].[client] ([isActive] ,[age] ,[name] ,[gender] ,[company] ,[email],[phone] ,[address]) VALUES({result}, {client.Age} , '{client.Name}', '{client.Gender}', '{client.Company}', '{client.Email}', '{client.Phone}', '{client.Address}')";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                //logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            }
        }
    }
 
}
