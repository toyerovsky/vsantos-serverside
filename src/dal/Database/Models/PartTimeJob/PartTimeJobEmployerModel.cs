using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRP.DAL.Database.Models.PartTimeJob
{
    public class PartTimeJobEmployerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }

        // foreign keys
        [ForeignKey("PartTimeJob")]
        public int PartTimeJobId { get; set; }

        // navigation properties
        public virtual PartTimeJobModel PartTimeJobModel { get; set; }
        public ICollection<PartTimeJobWorkerModel> Workers { get; set; }
    }
}