using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Repositories;
using DataLibrary.DTOs;

namespace ConsoleAppCSharpDemoDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            bool keepGoing = true;
            while(keepGoing)
            {
                Console.WriteLine("Would you like to:  [1] Look up books by author or [2] Look up authors and their books by country?");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("Please enter an author to find their books:");
                    string author = Console.ReadLine();

                    BookRepository bookRepository = new BookRepository();

                    List<string> books = bookRepository.GetBooksByAuthor(author);

                    Console.WriteLine("The author has written these books:");
                    foreach (string b in books)
                    {
                        Console.WriteLine(b);
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a country to find its authors and their books:");
                    string country = Console.ReadLine();

                    BookRepository bookRepository = new BookRepository();

                    //This line shows us getting the data using SqlDataReader and Object:
                    ////List<AuthorDTO> authorDTOs = bookRepository.GetAuthorsAndBooksByResidesCountry(country);
                    
                    //This line shows us getting the data using SqlDataAdapter and DataTable:
                    List<AuthorDTO> authorDTOs = bookRepository.GetTableOfAuthorsAndBooksByResidesCountry(country);

                    Console.WriteLine($"The following authors live in {country}, and have written these books:");
                    foreach (AuthorDTO am in authorDTOs)
                    {
                        Console.WriteLine($"Author: {am.AuthorName}, Book: {am.BookName}, Country: {am.ResidesCountry}.");
                    }
                }
                Console.WriteLine("Keep going? Yes / No");
                keepGoing = (Console.ReadLine().ToLower() == "yes") ? true : false;
            }

            Console.WriteLine("Press any key to end.");
            Console.ReadKey();
        }
    }
}
