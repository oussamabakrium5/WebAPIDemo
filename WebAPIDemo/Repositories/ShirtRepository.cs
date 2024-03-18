using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;
using LibraryDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIDemo.Repositories
{
    public class ShirtRepository
    {

        private readonly DataContext _context;


        public ShirtRepository(DataContext context)
        {
            _context = context;
        }


        public List<Shirt> GetShirts()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }

            return _context.Shirts.ToList();
        }

        public bool ShirtExists(int id)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }

            return _context.Shirts.Any(x => x.ShirtId == id);
        }

        public Shirt? GetShirtById(int id)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }
            //return shirts.FirstOrDefault(x => x.ShirtId == id);
            return _context.Shirts.FirstOrDefault(x => x.ShirtId == id);
        }

        public Shirt? GetShirtBuProperties(string? brand, string? gender, string? color, int? size)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }

            return _context.Shirts.FirstOrDefault(x =>
        !string.IsNullOrWhiteSpace(brand) &&
        !string.IsNullOrWhiteSpace(x.Brand) &&
        x.Brand.ToUpper() == brand.ToUpper() && // Convert both sides to upper case
        !string.IsNullOrWhiteSpace(gender) &&
        !string.IsNullOrWhiteSpace(x.Gender) &&
        x.Gender.ToUpper() == gender.ToUpper() && // Convert both sides to upper case
        !string.IsNullOrWhiteSpace(color) &&
        !string.IsNullOrWhiteSpace(x.Color) &&
        x.Color.ToUpper() == color.ToUpper() && // Convert both sides to upper case
        size.HasValue &&
        x.Size.HasValue &&
        size.Value == x.Size.Value);
        }

        public void AddShirt(Shirt shirt)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }
            /*int maxId = shirts.Max(x => x.ShirtId);
            shirt.ShirtId = maxId +1 ;
            shirts.Add(shirt);*/

			shirt.rank = _context.Shirts.Max(shirt => shirt.rank) + 1;


			_context.Shirts.Add(shirt);
            _context.SaveChanges();
        }

        public void UpdateShirt(Shirt shirt)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }
            // Detach any previously tracked entity with the same key
            var existingEntry = _context.ChangeTracker.Entries<Shirt>().FirstOrDefault(e => e.Entity.ShirtId == shirt.ShirtId);
            if (existingEntry != null)
            {
                existingEntry.State = EntityState.Detached;
            }

            shirt.rank = _context.Shirts.Where(x => x.ShirtId == shirt.ShirtId).Select(x => x.rank).Single();

			// Attach and mark the new entity as modified
			_context.Shirts.Attach(shirt);
            _context.Entry(shirt).State = EntityState.Modified;

            // Save changes to apply the updates
            _context.SaveChanges();
        }

        public void DeleteShirt(int shirtId)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }
            /*var shirt = GetShirtById(shirtId);
            if(shirt != null)
            {
                shirts.Remove(shirt);
            }*/

            var shirt = GetShirtById(shirtId);
            
            if (shirt != null)
            {
				int rank = shirt.rank;
				// Inspect the state of the shirt entity
				var entry = _context.Entry(shirt);
                Console.WriteLine($"State before deletion: {entry.State}");

                _context.Shirts.Remove(shirt);

                // Inspect the state again after marking for deletion
                Console.WriteLine($"State after marking for deletion: {entry.State}");

				// Retrieve shirts with rank greater than 'rank'
				var shirtsToUpdate = _context.Shirts.Where(x => x.rank > rank).ToList();
				// Decrement rank for each shirt
				foreach (var shi in shirtsToUpdate)
				{
					shi.rank--;
				}

				// Save changes to apply the deletion
				_context.SaveChanges();
            }
        }

		public void PatchShirtRank(int shirtId, int newRank)
		{
			if (_context == null)
			{
				throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
			}

			var shirt = _context.Shirts.FirstOrDefault(x => x.ShirtId == shirtId);

			shirt.rank = newRank;

			_context.SaveChanges();
		}

	}
}
