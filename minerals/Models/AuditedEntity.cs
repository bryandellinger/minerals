using System;

namespace Models
{
    public class AuditedEntity
    {
        public DateTime? CreateDate { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

    }
}