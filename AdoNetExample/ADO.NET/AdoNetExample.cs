using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;

namespace QuestGame.WebApi.ADO.NET
{
    class AdoNetExample
    {
        static void Main(string[] args)
        {
            var connecionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var connection = new SqlConnection(connecionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                Console.WriteLine("Подключено");

                var sqlExpression = "SELECT * FROM Quests";
                var command = new SqlCommand(sqlExpression, connection);
                var result = command.ExecuteReader();

                if (!result.HasRows)
                {
                    throw new Exception("Нет данных!");
                }

                while (result.Read())
                {
                    var id = result.GetInt32(0);
                    var title = result.GetString(1);
                    var date = result.GetDateTime(2);
                    var rate = result.GetInt32(3);
                    var active = result.GetBoolean(4);
                    var owner = result.GetString(5);

                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", id, title, date, rate, active, owner);
                }

                Console.ReadKey();
            }
        }
    }
}