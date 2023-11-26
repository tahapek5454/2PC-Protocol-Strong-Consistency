using Coordinator.Models;
using Coordinator.Models.Contexts;
using Coordinator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Coordinator.Services.Concrete
{
    public class TransactionService : ITransactionService
    {
        
        private readonly TwoPhaseCommitContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _stockHttpClient;
        private readonly HttpClient _orderHttpClient;
        private readonly HttpClient _paymentHttpClient;

        public TransactionService(TwoPhaseCommitContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

            _stockHttpClient = _httpClientFactory.CreateClient("Stock.API");
            _orderHttpClient = _httpClientFactory.CreateClient("Order.API");
            _paymentHttpClient = _httpClientFactory.CreateClient("Payment.API");
        }

        public async Task<Guid> CreateTransactionAsync()
        {
            Guid transactionId = Guid.NewGuid();

            var nodes = await _context.Nodes.ToListAsync();

            nodes.ForEach(node => node.NodeSates = new List<NodeSate>()
            {
                new(transactionId)
                {
                    IsReady = Enums.ReadyType.Pending,
                    TransactionState = Enums.TransactionState.Pending
                }
            });

            _context.SaveChanges();

            return transactionId;
        }

        public async Task PrepareServicesAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeSates.Include(x => x.Node)
                .Where(x => x.TransactionId == transactionId).ToListAsync();



            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.Name switch
                    {
                        "Order.API" => _orderHttpClient.GetAsync("ready"),
                        "Stock.API" => _stockHttpClient.GetAsync("ready"),
                        "Payment.API" => _paymentHttpClient.GetAsync("ready")
                    });

                    var result = bool.Parse(await response.Content.ReadAsStringAsync());

                    transactionNode.IsReady = result ? Enums.ReadyType.Ready : Enums.ReadyType.UnReady;
                }
                catch (Exception e)
                {

                    transactionNode.IsReady = Enums.ReadyType.UnReady;
                }
            }

            _context.SaveChanges();
        }

        public async Task<bool> CheckReadyServicesAsync(Guid transactionId)
        {
            var transactionNodeStates = await _context.NodeSates
                .Where(x => x.TransactionId == transactionId).ToListAsync();


            var response = transactionNodeStates.TrueForAll(x => x.IsReady == Enums.ReadyType.Ready);

            return response;
        }

        public async Task CommitAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeSates.Include(x => x.Node)
               .Where(x => x.TransactionId == transactionId).ToListAsync();


            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.Name switch
                    {
                        "Order.API" => _orderHttpClient.GetAsync("commit"),
                        "Stock.API" => _stockHttpClient.GetAsync("commit"),
                        "Payment.API" => _paymentHttpClient.GetAsync("commit")
                    });

                    var result = bool.Parse(await response.Content.ReadAsStringAsync());

                    transactionNode.TransactionState = result ? Enums.TransactionState.Done : Enums.TransactionState.Abort;
                }
                catch (Exception e)
                {

                    transactionNode.TransactionState = Enums.TransactionState.Abort;
                }
            }

            _context.SaveChanges();

        }

        public async Task<bool> CheckTransactionStateServicesAsync(Guid transactionId)
        {
            var transactionNodeStates = await _context.NodeSates
               .Where(x => x.TransactionId == transactionId).ToListAsync();


            var response = transactionNodeStates.TrueForAll(x => x.TransactionState == Enums.TransactionState.Done);

            return response;
        }


        public async Task RollbackAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeSates.Include(x => x.Node)
                .Where(x => x.TransactionId == transactionId).ToListAsync();


            foreach (var transactionNode in transactionNodes)
            {
                try
                {                
                    if(transactionNode.TransactionState == Enums.TransactionState.Done)
                    {
                        _ = await (transactionNode.Node.Name switch
                        {
                            "Order.API" => _orderHttpClient.GetAsync("rollback"),
                            "Stock.API" => _stockHttpClient.GetAsync("rollback"),
                            "Payment.API" => _paymentHttpClient.GetAsync("rollback")
                        });
                    }


                    transactionNode.TransactionState = Enums.TransactionState.Abort;
                }
                catch (Exception e)
                {

                    transactionNode.TransactionState = Enums.TransactionState.Abort;

                }
            }

            _context.SaveChanges();
        }
    }
}
