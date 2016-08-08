using QuestGame.Domain.DBInitializers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        private Random rnd = new Random();

        protected override void Seed(ApplicationDbContext dbContext)
        {
            // Здесь инициализируем БД

            DBInitialization initialization = new DBInitialization(dbContext);

            // Здесь добавляем созданные нами объекты, наследованные от InitializationDB, для инициализации БД
            // Пример DBInitilizers.InitUserAdmin
            initialization.Add(new InitUserAdmin());  // Добавил Ruslan

            initialization.Initialization();
        }
    }
}
