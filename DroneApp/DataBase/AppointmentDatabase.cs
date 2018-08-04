using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using DroneApp.AerodromePages;
namespace DroneApp.DataBase
{
    public class AppointmentDatabase
    {
        public SQLiteAsyncConnection database;

        public AppointmentDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Appointment>().Wait();
            database.CreateTableAsync<Aerodromes>().Wait();
        }

        public Task<List<Appointment>> GetItemsAsync()
        {
            return database.Table<Appointment>().ToListAsync();
        }

        public Task<List<Aerodromes>> GetAerodromesAsync()
        {
            return database.Table<Aerodromes>().ToListAsync();
        }

        public Task<List<Appointment>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Appointment>("SELECT * FROM [Appointment] WHERE [Done] = 0");
        }

        public Task<List<Appointment>> GetItemsDoneAsync()
        {
            return database.QueryAsync<Appointment>("SELECT * FROM [Appointment] WHERE [Done] = 1");
        }

        public Task<Appointment> GetItemAsync(int ID)
        {
            return database.Table<Appointment>().Where(i => i.ID == ID).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Appointment item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> SaveAerodromeAsync(Aerodromes aerodrome)
        {
            if (aerodrome.ID != 0)
            {
                return database.UpdateAsync(aerodrome);
            }
             else
            {
                return database.InsertAsync(aerodrome);
            }
        }

        public Task<int> DeleteAerodromeAsync(Aerodromes aerodrome)
        {
            return database.DeleteAsync(aerodrome);
        }

        public Task<int> DeleteItemAsync(Appointment item)
        {
            return database.DeleteAsync(item);
        }
    }
}
