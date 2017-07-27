using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using FortuneCookie.PersonalizationEngine.EditorModels;
using FortuneCookie.PersonalizationEngine.Services;

namespace FortuneCookie.PersonalizationEngine.Models
{
    public class AdminViewModel
    {
        public IEnumerable<VisitorGroupContentProviderModel> ContentProviders { get; set; }
        public SelectList VisitorGroupList { get; set; }
        public SelectList ContentProviderList { get; set; }

        [DisplayName("Visitor Group")]
        public Guid VisitorGroupId { get; set; }

        [DisplayName("Content Provider")]
        public string ContentProviderTypeName { get; set; }

        private ICriteriaModel _criteriaModel;
        public ICriteriaModel CriteriaModel
        {
            get
            {
                if (_criteriaModel == null)
                {
                    if (string.IsNullOrEmpty(ContentProviderTypeName))
                        return new DefaultCriteriaModel();

                    var modelCriteriaName = new ContentProviderAttributeService().GetCriteriaEditorTypeFromAttribute(ContentProviderTypeName);
                    _criteriaModel = Activator.CreateInstance(modelCriteriaName) as ICriteriaModel;
                }
                return _criteriaModel;
            }
        }

        public string FormPanelVisibility(bool modelIsValid)
        {
            return modelIsValid ? "display:none;" : "display:block;";
        }
    }
}