using System;
using System.Collections.Generic;
using System.Linq;
using WebAPIDemo.Controllers;
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;
using LibraryDemo.Models;

namespace WebAPIDemo.Repositories
{
    public class ShirtRepository
    {

        private readonly DataContext _context;


        public ShirtRepository(DataContext context)
        {
            _context = context;
        }
        // public static void Initialize(DataContext context)
        // {
        //     _context = context;
        // }


        /*private static List<Shirt> shirts = new List<Shirt>()
        {
            new Shirt{ ShirtId = 1, Brand = "My Brand", Color = "Blue", Gender = "Men", price = 30, Size = 10},
            new Shirt{ ShirtId = 2, Brand = "My Brand", Color = "Black", Gender = "Men", price = 35, Size = 12},
            new Shirt{ ShirtId = 3, Brand = "Your Brand", Color = "Pink", Gender = "WoMen", price = 28, Size = 8},
            new Shirt{ ShirtId = 4, Brand = "Your Brand", Color = "Yello", Gender = "WoMen", price = 30, Size = 9}
        };*/


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
            /*return _context.Shirts.FirstOrDefault(x =>
                x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
                x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
                x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
                x.Size == size);*/
            /*return _context.Shirts.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(brand) &&
                !string.IsNullOrWhiteSpace(x.Brand) &&
                x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(gender) &&
                !string.IsNullOrWhiteSpace(x.Gender) &&
                x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(color) &&
                !string.IsNullOrWhiteSpace(x.Color) &&
                x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
                size.HasValue &&
                x.Size.HasValue &&
                size.Value == x.Size.Value);*/

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
            _context.Shirts.Add(shirt);
            _context.SaveChanges();
        }

        public void UpdateShirt(Shirt shirt)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ShirtRepository has not been initialized. Call Initialize method first.");
            }
            /*var shirtToUpdate = shirts.First(x => x.ShirtId == shirt.ShirtId);
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.price = shirt.price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;*/

            // Detach any previously tracked entity with the same key
            var existingEntry = _context.ChangeTracker.Entries<Shirt>().FirstOrDefault(e => e.Entity.ShirtId == shirt.ShirtId);
            if (existingEntry != null)
            {
                existingEntry.State = EntityState.Detached;
            }

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
                // Inspect the state of the shirt entity
                var entry = _context.Entry(shirt);
                Console.WriteLine($"State before deletion: {entry.State}");

                _context.Shirts.Remove(shirt);

                // Inspect the state again after marking for deletion
                Console.WriteLine($"State after marking for deletion: {entry.State}");

                // Save changes to apply the deletion
                _context.SaveChanges();
            }
        }
    }
}
