using _.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace _.Domain.Entities.Catalog
{
    public class Location: AuditableEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Time { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString();
        public string Speed { get; set; }
        public double Latitude { get; set; }
        public double Long { get; set; }
        public string GpsCourse { get; set; }
        public string SpeedSignalStatus { get; set; }
        public string EngineON { get; set; }
        public int SpeedGovId { get; set; }
        [ForeignKey("SpeedGovId")]
        public virtual SpeedGovernor SpeedGovernor { get; set; }

    public override string ToString() => this.Latitude.ToString() + this.Long.ToString();
  }
}
