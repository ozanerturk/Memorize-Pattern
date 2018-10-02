using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCacheExample
{

    class Program
    {

        static void Main(string[] args)
        {

            var res = Search(name: "ozan");
            res = Search(number: 4);
            res = Search(name: "a", minAge: 22);
            res = Search(number: 4);
            res = Search(minAge: 28);
            Console.ReadLine();
        }



        public static List<Student> Search(string name = "", int number = 0, int minAge = 0)
        {

            IQueryable<Student> result = DBContext.Students.AsQueryable();

            if (name != "")
            {
                result = result.Where(x => x.Name.ToUpperInvariant().Contains(name.ToUpperInvariant()));
            }
            if (number != 0)
            {
                result = result.Where(x => x.Number == number);
            }
            if (minAge != 0)
            {
                result = result.Where(x => x.Age >= minAge);
            }

            return result.Cacheble().ToList();

        }


       
    }


}
