using System.Web.Mvc;
using FortuneCookie.PersonalizationEngine.EditorModels;
using FortuneCookie.PersonalizationEngine.Models;

namespace FortuneCookie.PersonalizationEngine
{
    public class CriteriaValidationModelBinder : DefaultModelBinder
    {
        const string ValidationPropertyName = "Criteria";

        /// <summary>
        /// Binds the model, and honours any validation attributes set on dyanmically inserted form elements, by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext "/>parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                return bindingContext.Model;

            var model = base.BindModel(controllerContext, bindingContext);
            
            var adminViewModel = model as AdminViewModel;
            if (adminViewModel == null)
                return model;

            var criteriaValue = bindingContext.ValueProvider.GetValue(ValidationPropertyName);
            adminViewModel.CriteriaModel.Criteria = criteriaValue != null ? criteriaValue.AttemptedValue : string.Empty;

            ModelMetadata modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => adminViewModel.CriteriaModel, typeof(ICriteriaModel));
            ModelValidator compositeValidator = ModelValidator.GetModelValidator(modelMetadata, controllerContext);

            foreach (ModelValidationResult result in compositeValidator.Validate(null))
                bindingContext.ModelState.AddModelError(ValidationPropertyName, result.Message);

            return model;
        }
    }
}