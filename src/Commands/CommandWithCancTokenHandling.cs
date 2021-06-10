using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WebApiCanTok.Commands
{
    public class CommandWithCancTokenHandling : IRequest<CommandWithCancTokenHandlingResult>
    {
        public bool HandleCancellationToken { get; }
        public bool PassCancellationTokenToOtherMethods { get; }

        public CommandWithCancTokenHandling(
            bool handleCancellationToken,
            bool passCancellationTokenToOtherMethods = false)
        {
            HandleCancellationToken = handleCancellationToken;
            PassCancellationTokenToOtherMethods = passCancellationTokenToOtherMethods;
        }
    }

    public record CommandWithCancTokenHandlingResult();

    public class
        CommandWithCancTokenHandlingHandler : IRequestHandler<CommandWithCancTokenHandling,
            CommandWithCancTokenHandlingResult>
    {
        private readonly ILogger<CommandWithCancTokenHandlingHandler> _logger;

        public CommandWithCancTokenHandlingHandler(ILogger<CommandWithCancTokenHandlingHandler> logger)
        {
            _logger = logger;
        }

        public async Task<CommandWithCancTokenHandlingResult> Handle(CommandWithCancTokenHandling request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Running command {(request.HandleCancellationToken ? "with" : "without")} cancellationToken checking");

            _logger.LogDebug("Validating request. It might take around 2 seconds...");
            await Task.Delay(2000, request.PassCancellationTokenToOtherMethods ? cancellationToken : default);
            _logger.LogDebug("Request validated.");
            
            //
            if (request.HandleCancellationToken)
            {
                _logger.LogDebug("Processing request if cancellationToken has not been cancelled...");
                cancellationToken.ThrowIfCancellationRequested();
            }
            

            // Imagine that you have some db select, data transforming or something long-running here
            var operationResult = await LongRunningOperation(request.PassCancellationTokenToOtherMethods ? cancellationToken : default);

            // This line is to verify if the request has been cancelled.
            if(request.HandleCancellationToken)
            {
                _logger.LogDebug("Operation finished, processing request if cancellationToken has not been cancelled...");
                cancellationToken.ThrowIfCancellationRequested();
            }

            // This line will only happen, if request has not been cancelled
            await PersistOperationResult(operationResult);

            return new CommandWithCancTokenHandlingResult();
        }

        private async Task<object> LongRunningOperation(CancellationToken cancellationToken)
        {
            for (var i = 0; i < 5; i++)
            {
                _logger.LogDebug($"Loop {i + 1} of {nameof(CommandWithCancTokenHandlingHandler)}");
                await Task.Delay(1000, cancellationToken);
            }

            return new object();
        }

        private async Task PersistOperationResult(object operationResult)
        {
            _logger.LogDebug($"Persisting operation result");
            return;
        }
    }
}