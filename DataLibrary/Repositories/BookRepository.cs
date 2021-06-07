using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using DataLibrary.DataAccessLayer;
using DataLibrary.DTOs;

namespace DataLibrary.Repositories
{
    public class BookRepository
    {
        public List<string> GetBooksByAuthor(string AuthorName)
        {
            //Create a new instance of our DAL, passing in the connection string
            DataAccess dal = new DataAccess(ConfigurationManager.ConnectionStrings["BookInformationConnection"].ConnectionString);

            //Create a Dictionary (key, value pairs) of paramaters and values for our stored proc to use in our DAL
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@AuthorName", AuthorName);

            //Call our DAL method to get a list of books by author
            List<string> _booksbyAuthor = dal.ReadDataViaStoredProcedure<string>("spGetBookByAuthor", paramDictionary);

            return _booksbyAuthor;
        }

        public List<AuthorDTO> GetAuthorsAndBooksByResidesCountry(string ResidesCountry)
        {
            List<AuthorDTO> authorDTOList = new List<AuthorDTO>();

            //Create a new instance of our DAL, passing in the connection string
            DataAccess dal = new DataAccess(ConfigurationManager.ConnectionStrings["BookInformationConnection"].ConnectionString);

            //Create a Dictionary (key, value pairs) of paramaters and values for our stored proc to use in our DAL
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@Country", ResidesCountry);

            //Call our DAL method to get a list of books by author
            List<Object> _authorObjects = dal.PopulateObjectViaStoredProcedure("spGetAuthorsAndBooksByCountry", paramDictionary);

            //Now loop through our objects and put them in our model            
            foreach (Object[] obj in _authorObjects)
            {
                AuthorDTO authorDTO = new AuthorDTO();
                authorDTO.AuthorName = obj[0].ToString();
                authorDTO.BookName = obj[1].ToString();
                authorDTO.ResidesCountry = obj[2].ToString();

                authorDTOList.Add(authorDTO);
            }
          
            return authorDTOList;

        }

        public List<AuthorDTO> GetTableOfAuthorsAndBooksByResidesCountry(string ResidesCountry)
        {
            List<AuthorDTO> authorDTOList = new List<AuthorDTO>();

            //Create a new instance of our DAL, passing in the connection string
            DataAccess dal = new DataAccess(ConfigurationManager.ConnectionStrings["BookInformationConnection"].ConnectionString);

            //Create a Dictionary (key, value pairs) of paramaters and values for our stored proc to use in our DAL
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@Country", ResidesCountry);

            //Call our DAL method to get a list of books by author           
            DataTable authorTable = dal.PopulateDataTableViaStoredProcedure("spGetAuthorsAndBooksByCountry", paramDictionary);

            //Loop through our data table and populate the author model - notice with the data table we can use column names!
            foreach (DataRow row in authorTable.Rows)
            {
                AuthorDTO authorDTO = new AuthorDTO();
                authorDTO.AuthorName = row["AuthorName"].ToString();
                authorDTO.BookName = row["BookName"].ToString();
                authorDTO.ResidesCountry = row["ResidesCountry"].ToString();

                authorDTOList.Add(authorDTO);
            }
            return authorDTOList;

        }
    }
}
