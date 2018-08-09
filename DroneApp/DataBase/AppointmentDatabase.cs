using System.Collections.Generic;
using System.Threading.Tasks;
using DroneApp.TKMap.CustomPins;
using SQLite;

namespace DroneApp.DataBase
{
    public class AppointmentDatabase
    {
        //Create SQL connection 
        public SQLiteAsyncConnection database;

        public AppointmentDatabase(string dbPath)
        {
            //Establish file path w/n database
            database = new SQLiteAsyncConnection(dbPath);
            //Create appointments table 
            database.CreateTableAsync<Appointment>().Wait();
            //Create aerodromes table 
            database.CreateTableAsync<Aerodromes>().Wait();
            //Create Pin Table 
            database.CreateTableAsync<Pins>().Wait();
            //Create Circle Table
            database.CreateTableAsync<Circles>().Wait();
        }
        //Get, set/save, and delete functions
        //All require async commands to utilize 
        public Task<List<Aerodromes>> GetAerodromesAsync()
        {
            return database.Table<Aerodromes>().ToListAsync();
        }
        public Task<List<Appointment>> GetItemsAsync()
        {
            return database.Table<Appointment>().ToListAsync();
        }       
        public Task<List<Appointment>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Appointment>("SELECT * FROM [Appointment] WHERE [Done] = 0");
        }
        public Task<List<Appointment>> GetItemsDoneAsync()
        {
            return database.QueryAsync<Appointment>("SELECT * FROM [Appointment] WHERE [Done] = 1");
        }
        public Task<Appointment> GetItemAsync(int id) => database.Table<Appointment>().Where(i => i.ID == id).FirstOrDefaultAsync();
  
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
        public Task<int> SaveAerodromesAsync(Aerodromes aerodromes)
        {
            if (aerodromes.ID != 0)
            {
                return database.UpdateAsync(aerodromes);
            }
            else
            {
                return database.InsertAsync(aerodromes);
            }
        }
        public Task<int> DeleteItemAsync(Appointment item)
        {
            return database.DeleteAsync(item);
        }
        public Task<int> DeleteAerodromeAsync(Aerodromes aerodromes)
        {
            return database.DeleteAsync(aerodromes);
        }
    }
}