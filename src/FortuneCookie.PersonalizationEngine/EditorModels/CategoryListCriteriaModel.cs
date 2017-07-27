using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.DataAbstraction;

namespace FortuneCookie.PersonalizationEngine.EditorModels
{
    public class CategoryListCriteriaModel : ICriteriaModel
    {
        public string Criteria { get; set; }

        public IEnumerable<SelectListItem> CategorySelectListItems
        {
            get
            {
                var rootCategory = Category.GetRoot();
                return from Category category in rootCategory.Categories
                       select new SelectListItem() { Text = category.LocalizedDescription, Value = category.Name };
            }
        }
    }
}