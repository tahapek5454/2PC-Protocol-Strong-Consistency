using Microsoft.EntityFrameworkCore;

namespace Coordinator.Models.Contexts
{
    public class TwoPhaseCommitContext: DbContext
    {
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeSate> NodeSates { get; set; }

        public TwoPhaseCommitContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().HasData(
                    new Node("Order.API") {Id = Guid.Parse("be27ac8e-64ac-425f-a976-474bdef99cc4") },
                    new Node("Stock.API") {Id = Guid.Parse("204d4ccb-da5b-4445-a88b-2686ed09b2cd") },
                    new Node("Payment.API") {Id = Guid.Parse("462c2add-12ce-4ab6-aba1-f0b0c5354541") }
                );
        }
    }
}
