using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using FortuneCookie.PersonalizationEngine.ContentProviders;
using FortuneCookie.PersonalizationEngine.Models;
using FortuneCookie.PersonalizationEngine.Services;
using FortuneCookie.PersonalizationEngine.Validation;

namespace FortuneCookie.PersonalizationEngine
{
    /// <summary>
    /// An <see cref="EPiServer.Framework.IInitializableModule" /> enabling hooking into EPiServer events.
    /// </summary>
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ScanContentProviderAttributesInitializer : IInitializableModule
    {
        private ContentProviderModelValidator ContentModelValidator { get; set; }

        /// <summary>
        /// <para>Initializes the Module and attaches custom eventhandlers.</para>
        /// <para>Attaches a <see cref="System.EventHandler" /> for the EPiServer.Web.UrlSegment.
        /// CreatedUrlSegment event.</para>
        /// </summary>
        /// <param name="context">The current
        /// <see cref="EPiServer.Framework.Initialization.InitializationEngine"/></param>
        public void Initialize(InitializationEngine context)
        {
            SetContentModelValidator();
            ScanAssembliesForContentProviderAttributes();
            ValidateCriteriaEditorModels();
            SetInitialCacheDependencyKey();
            EnsureSavedContentProviderModels();
            AddCriteriaValidationModelBinder();
            AddDefaultPersonalizationEngineEvents();
        }

        private void SetContentModelValidator()
        {
            ContentModelValidator = new ContentProviderModelValidator();
        }

        private void ValidateCriteriaEditorModels()
        {
            ContentModelValidator.VerifyCriteriaEditorModels();
        }

        private void AddCriteriaValidationModelBinder()
        {
            ModelBinders.Binders.Add(typeof(AdminViewModel), new CriteriaValidationModelBinder());
        }

        private void EnsureSavedContentProviderModels()
        {
            ContentModelValidator.RemoveModelsWithUnknownContentProvider();
            ContentModelValidator.RemoveModelsWithUnknownVisitorGroup();
        }

        private void SetInitialCacheDependencyKey()
        {
            CachedContentProviderBase.InitialiseContentProviderCache();
        }

        private void ScanAssembliesForContentProviderAttributes()
        {
            new ContentProviderAttributeService().GetContentProviderList().ToList();
        }

        private void AddDefaultPersonalizationEngineEvents()
        {
            PersonalizationEngine.OnContentProviderGetContent += PersonalizationEngine_OnContentProviderGetContent;
        }

        private void PersonalizationEngine_OnContentProviderGetContent(ContentProviderEventArgs e)
        {
        }

        /// <summary>
        /// Perform pre-loading tasks.
        /// </summary>
        public void Preload(string[] parameters)
        {
        }

        /// <summary>
        /// <para>Uninitializes the Module.</para>
        /// <para>Detaches the <see cref="System.EventHandler" /> for the EPiServer.Web.UrlSegment.
        /// CreatedUrlSegment event added during Initialization.</para>
        /// </summary>
        /// <param name="context">The current
        /// <see cref="EPiServer.Framework.Initialization.InitializationEngine"/></param>
        public void Uninitialize(InitializationEngine context)
        {
            PersonalizationEngine.OnContentProviderGetContent -= PersonalizationEngine_OnContentProviderGetContent;    
        }
    }
}

