using System;

namespace Sat.Recruitment.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}