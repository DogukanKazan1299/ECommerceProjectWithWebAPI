using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Abstract
{
    public interface IUpdatedEntity
    {
        int? UpdateUserId { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}
