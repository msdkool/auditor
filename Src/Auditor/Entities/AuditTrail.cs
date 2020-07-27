using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auditor.Entities
{
    public class AuditTrail
    {
        public AuditTrail()
        {
            AuditTrailDetail = new HashSet<AuditTrailDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuditTrailId { get; set; }
        public string Action { get; set; }
        public string ActionEntity { get; set; }
        public string ActionBy { get; set; }
        public DateTime? AuditDate { get; set; }

        public virtual ICollection<AuditTrailDetail> AuditTrailDetail { get; set; }
    }
}
