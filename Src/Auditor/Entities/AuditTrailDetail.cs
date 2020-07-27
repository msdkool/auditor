using System.ComponentModel.DataAnnotations.Schema;

namespace Auditor.Entities
{
    public class AuditTrailDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuditTrailDetailId { get; set; }
        public long AuditTrailID { get; set; }
        public string ColumnName { get; set; }
        public string OldRecord { get; set; }
        public string NewRecord { get; set; }

        public virtual AuditTrail AuditTrail { get; set; }
    }
}