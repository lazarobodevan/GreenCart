using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models {
    public class ConsumerFavProducer {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Consumer")]
        public Guid ConsumerId { get; set; }
        public Consumer Consumer { get; set; }

        [ForeignKey("Producer")]
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }

        public ConsumerFavProducer() { }


    }
}
