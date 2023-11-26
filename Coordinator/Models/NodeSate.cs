using Coordinator.Enums;

namespace Coordinator.Models
{
    public record NodeSate(Guid TransactionId)
    {
        public Guid Id { get; set; }


        public Guid NodeId { get; set; }
        public Node Node { get; set; }


        // first step behavior state
        public ReadyType IsReady { get; set; }

        // second step bheavior state
        public TransactionState TransactionState { get; set; }
    }
}
