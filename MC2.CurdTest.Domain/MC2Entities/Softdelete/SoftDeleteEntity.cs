using MC2.CurdTest.Domain.MC2Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC2.CurdTest.Domain.MC2Entities.Softdelete
{
    public abstract class SoftDeleteEntity : ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
