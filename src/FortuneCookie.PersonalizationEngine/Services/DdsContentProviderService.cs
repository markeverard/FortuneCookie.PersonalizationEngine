using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;
using FortuneCookie.PersonalizationEngine.Models;

namespace FortuneCookie.PersonalizationEngine.Services
{
    public class DdsContentProviderService : IContentProviderService
    {
        protected DynamicDataStore GetStore { get { return typeof(VisitorGroupContentProviderModel).GetStore(); } }

        public VisitorGroupContentProviderModel GetModel(Guid id)
        {
            return GetStore.Load<VisitorGroupContentProviderModel>(id);
        }

        public IEnumerable<VisitorGroupContentProviderModel> GetAllModels()
        {
            return GetStore.LoadAll<VisitorGroupContentProviderModel>().OrderBy(m => m.SortOrder);
        }

        public void Add(VisitorGroupContentProviderModel model)
        {
            GetStore.Save(model);
        }

        public void Update(VisitorGroupContentProviderModel model)
        {
            GetStore.Save(model);
        }

        public void Delete(Guid id)
        {
            VisitorGroupContentProviderModel modelTodelete = GetModel(id);
            if (modelTodelete == null)
                return;

            IEnumerable<VisitorGroupContentProviderModel> models = GetAllModels().Where(m => m.SortOrder > modelTodelete.SortOrder);
            foreach (var contentProviderModel in models)
            {
                contentProviderModel.SortOrder -= 1;
                Update(contentProviderModel);
            }
            
            GetStore.Delete(id);
        }

        public void Reorder(Guid id, ReorderDirection direction)
        {
            var modelToMove = GetModel(id);
            if (modelToMove == null)
                throw new NullReferenceException(string.Format("No ContentProvider model found with id = {0}", id));

            if (direction == ReorderDirection.Up)
                MoveModelUp(modelToMove);

            if (direction == ReorderDirection.Down)
                MoveModelDown(modelToMove); 
            
            return;
        }

        private void MoveModelUp(VisitorGroupContentProviderModel modelToMove)
        {
            if (modelToMove.SortOrder == 1)
                return;

            IEnumerable<VisitorGroupContentProviderModel> models = GetAllModels();
            modelToMove.SortOrder -= 1;

            VisitorGroupContentProviderModel modelToMoveDown = models.Where(m => m.SortOrder == modelToMove.SortOrder).SingleOrDefault();
            
            if (modelToMoveDown == null)
                throw new NullReferenceException(string.Format("No ContentProvider model found with SortOrder = {0}", modelToMove.SortOrder));
            
            modelToMoveDown.SortOrder += 1;

            Update(modelToMove);
            Update(modelToMoveDown);
        }

        private void MoveModelDown(VisitorGroupContentProviderModel modelToMove)
        {
            if (modelToMove.SortOrder == GetMaxSortOrder())
                return;

            IEnumerable<VisitorGroupContentProviderModel> models = GetAllModels();
            modelToMove.SortOrder += 1;

            VisitorGroupContentProviderModel modelToMoveUp = models.Where(m => m.SortOrder == modelToMove.SortOrder).SingleOrDefault();
            
            if (modelToMoveUp == null)
                throw new NullReferenceException(string.Format("No ContentProvider model found with SortOrder = {0}", modelToMove.SortOrder));
            
            modelToMoveUp.SortOrder -= 1;

            Update(modelToMove);
            Update(modelToMoveUp);
        }

        public int GetMaxSortOrder()
        {
            var items = GetStore.LoadAll<VisitorGroupContentProviderModel>().ToList();
            return items.Count == 0 ? 0 : items.Max(m => m.SortOrder);
        }
    }
}