using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.DataAbstraction;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Shell.Navigation;
using FortuneCookie.PersonalizationEngine.ContentProviders;
using FortuneCookie.PersonalizationEngine.Models;
using FortuneCookie.PersonalizationEngine.Services;
using FortuneCookie.PersonalizationEngine.Validation;

namespace FortuneCookie.PersonalizationEngine.Controllers
{
    [Authorize(Roles = "CmsAdmins, VisitorGroupAdmins")]
    public class AdminController : Controller
    {
        protected IContentProviderService ContentProviderService { get; set; }
        protected ContentProviderAttributeService AttributeService { get; set; }
        protected IVisitorGroupRepository VisitorGroupRepository { get; set; }
        
        public AdminController()
        {
            ContentProviderService = new DdsContentProviderService();
            AttributeService = new ContentProviderAttributeService();
            VisitorGroupRepository = new VisitorGroupStore(); 
            ValidateContentProviderModels();
        }

        [MenuItem("/global/cms/edit", Text = "Personalization Engine")]
        [HttpGet]
        public ActionResult Index()
        {
            return VisitorGroupRepository.List().Any()
                       ? View(CreateDefaultAdminViewModel())
                       : View("Setup");
        }

        [HttpPost]
        public ActionResult Index(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                VisitorGroupContentProviderModel persistanceModel = VisitorGroupContentProviderFactory.Create(model.ContentProviderTypeName, model.VisitorGroupId, model.CriteriaModel.Criteria);
                ContentProviderService.Add(persistanceModel);
                return RedirectToAction("Index");
            }

            AdminViewModel defaultModel = CreateDefaultAdminViewModel();
            TryUpdateModel(defaultModel, new[] { "ContentProviderTypeName", "VisitorGroupId", "CriteriaModel.Criteria" });

            return View("Index", defaultModel);
        }

        public ActionResult Delete(Guid id)
        {
            ContentProviderService.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult ReorderUp(Guid id)
        {
            ContentProviderService.Reorder(id, ReorderDirection.Up);
            return RedirectToAction("Index");
        }

        public ActionResult ReorderDown(Guid id)
        {
            ContentProviderService.Reorder(id, ReorderDirection.Down);
            return RedirectToAction("Index");
        }

        public ActionResult RefreshCache()
        {
            CachedContentProviderBase.InitialiseContentProviderCache();
            return RedirectToAction("Index");
        }

        public ActionResult Example(AdminExampleViewModel model)
        {
            AddSelectedGroupsToContext(model.SelectedVisitorGroupIds);

            model.LanguageBranchItems = PopulateLanguageBranchItems(model.SelectedLanguageBranchItem);
            model.VisitorGroupItems = PopulateVisitorGroupItems(model.SelectedVisitorGroupIds);
            model.PageCountItems = PopulatePageCountItems(model.SelectedPageCountItem);
            model.RecommendedContent = new PersonalizationEngine().GetRecommendedContent(ControllerContext.HttpContext.User, model.SelectedLanguageBranchItem, model.PageCount);

            return View("Example", model);
        }

        public ActionResult CriteriaEditorTemplate(string contentProviderType)
        {
            var criteriaEditorType = AttributeService.GetCriteriaEditorTypeFromAttribute(contentProviderType);

            var contentProviderInstance = Activator.CreateInstance(criteriaEditorType);
            if (contentProviderInstance == null)
                throw new FormatException(string.Format("Could not create instance of {0}", criteriaEditorType));

            return PartialView(contentProviderInstance);
        }

        private AdminViewModel CreateDefaultAdminViewModel()
        {
            IEnumerable<ContentProviderAttributeModel> contentProviders = AttributeService.GetContentProviderList();

            var model = new AdminViewModel
            {
                ContentProviders = ContentProviderService.GetAllModels(),
                ContentProviderList = new SelectList(contentProviders, "TypeName", "DisplayName", contentProviders.FirstOrDefault().TypeName),
                VisitorGroupList = new SelectList(VisitorGroupRepository.List(), "Id", "Name")
            };

            return model;
        }

        private IEnumerable<SelectListItem> PopulateVisitorGroupItems(IEnumerable<string> selectedItems)
        {
            return VisitorGroupRepository.List()
                .Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString(), Selected = selectedItems.Contains(g.Id.ToString()) });
        }

        private IEnumerable<SelectListItem> PopulateLanguageBranchItems(string selectedItem)
        {
            return LanguageBranch.ListEnabled().Select(language =>
                   new SelectListItem { Text = language.Culture.Name, Value = language.Culture.Name, Selected = selectedItem.Equals(language.Culture.Name) });
        }

        private IEnumerable<SelectListItem> PopulatePageCountItems(string selectedItem)
        {
            return new List<SelectListItem>
                       {
                           new SelectListItem { Text = "10 pages", Value = "10", Selected = selectedItem.Equals("10")},
                           new SelectListItem { Text = "20 pages", Value = "20", Selected = selectedItem.Equals("20")},
                           new SelectListItem { Text = "50 pages", Value = "50", Selected = selectedItem.Equals("50")}
                       };
        }

        private void AddSelectedGroupsToContext(IEnumerable<string> visitorGroupIds)
        {
           ControllerContext.HttpContext.Items.Add("ImpersonatedVisitorGroupsById", visitorGroupIds);
        }

        private void ValidateContentProviderModels()
        {
            var contentModelValidator = new ContentProviderModelValidator();
            contentModelValidator.RemoveModelsWithUnknownVisitorGroup();
        }
    }
}
