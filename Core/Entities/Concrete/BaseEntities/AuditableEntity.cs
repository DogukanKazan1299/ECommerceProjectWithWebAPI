using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete.BaseEntities
{
    public class AuditableEntity : BaseEntity, ICreatedEntity, IUpdatedEntity
    {
        public int CreatedUserId { get ; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
