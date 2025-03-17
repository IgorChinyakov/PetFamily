using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Shared
{
    public abstract class SoftDeletableEntity : Entity<Guid>
    {
        public bool IsDeleted { get; protected set; } 
        public DateTime? DeletionDate { get; protected set; }

        protected SoftDeletableEntity() { }

        protected SoftDeletableEntity(Guid id) : base(id) { }

        public virtual void Delete()
        {
            if(!IsDeleted)
            {
                IsDeleted = true;
                DeletionDate = DateTime.UtcNow;
            }
        }

        public virtual void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                DeletionDate = null;
            }
        }
    }
}
