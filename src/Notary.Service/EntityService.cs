using log4net;
using Notary.Contract;
using Notary.Interface.Repository;
using Notary.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Service
{
    public abstract class EntityService<TC> : IEntityService<TC>
        where TC : Entity
    {
        public IRepository<TC> Repository { get; private set; }

        public ILog Logger { get; }

        public EntityService(IRepository<TC> repository, ILog log)
        {
            Repository = repository;
            Logger = log;
        }

        public async Task DeleteAsync(string slug, string updatedBySlug)
        {
            await Repository.DeleteAsync(slug, updatedBySlug);
        }

        public async Task<List<TC>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public async Task<TC> GetAsync(string slug)
        {
            return await Repository.GetAsync(slug);
        }

        public async Task SaveAsync(TC entity, string updatedBySlug)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await Repository.SaveAsync(entity);
        }
    }
}
