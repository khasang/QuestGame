using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.ADO.NET
{
    class AdoNetProcedureExample
        {
            static void Main(string[] args)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (var connection = new SqlConnection(connectionString))
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
                    Console.WriteLine("Подключение открыто");

                    var procedureName = "GetQuestsByUserRole";
                    var command = new SqlCommand(procedureName, connection);

                    command.CommandType = CommandType.StoredProcedure;

                    //var param = new SqlParameter("roleName", "admin");

                    var param = new SqlParameter
                    {
                        ParameterName = "rolename",
                        Value = "admin"
                    };

                    command.Parameters.Add(param);
                    var result = command.ExecuteReader();

                    if (!result.HasRows)
                    {
                        Console.WriteLine("Нет данных!");
                        Console.ReadKey();

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

                        Console.WriteLine("id={0}\ntitle={1}\ndate={2}\nrate={3}\nactive={4}\nowner={5}\n\n", id, title, date, rate, active, owner);
                    }

                    Console.ReadKey();
                }
            }
        }
    }