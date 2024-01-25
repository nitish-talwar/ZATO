using Microsoft.EntityFrameworkCore;
using ZATO.Contracts;
using ZATO.Exceptions;
using ZATO.Models;

namespace ZATO.Repository;

//The following GenericRepository class Implement the IGenericRepository Interface
//And Here T is going to be a class
//While Creating an Instance of the GenericRepository type, we need to specify the Class Name
//That is we need to specify the actual class name of the type T
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
        //The following variable is going to hold the EmployeeDBContext instance
        private ZATOStoreContext _context = null;
        
        //The following Variable is going to hold the DbSet Entity
        private DbSet<T> table = null;
        //Using the Parameterless Constructor, 
        //we are initializing the context object and table variable
        public GenericRepository()
        {
            this._context = new ZATOStoreContext();
            //Whatever class name we specify while creating the instance of GenericRepository
            //That class name will be stored in the table variable
            table = _context.Set<T>();
        }
        //Using the Parameterized Constructor, 
        //we are initializing the context object and table variable
        public GenericRepository(ZATOStoreContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        //This method will return all the Records from the table
        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }
        
        public async Task<T> GetByIdAsync(int id)
        {
            T existing = table.Find(id);
                
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id value. It must be greater than zero.", nameof(id));
            }

            T entity = await table.FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.");
                // EntityNotFoundException is a custom exception you might define to handle entity not found scenarios
            }
            return entity;
        }
        
        public async Task AddAsync(T obj)
        {
            await table.AddAsync(obj);
        }

        public async Task UpdateAsync(T obj)
        {
            //First attach the object to the table
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            //First, fetch the record from the table
            T existing =  table.Find(id);
            //This will mark the Entity State as Deleted
            table.Remove(existing);
            await _context.SaveChangesAsync();
        }
}