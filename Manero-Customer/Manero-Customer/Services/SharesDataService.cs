using Manero_Customer.Data.Models;

namespace Manero_Customer.Services
{
    public class SharesDataService
    {
        private List<CategoryModel> categories = new List<CategoryModel>();

        public void SetCategorie(List<CategoryModel> models)
        {
            categories = models;
        }

        public List<CategoryModel> GetCategorie()
        {
            return categories; 
        }   
    }
}
