using System.ComponentModel.DataAnnotations;

namespace FortuneCookie.PersonalizationEngine.EditorModels
{
    public class TextBoxCriteriaModel : ICriteriaModel
    {
        [Required(ErrorMessage = "You must enter a Criteria value")]
        public string Criteria { get; set; }
    }
}