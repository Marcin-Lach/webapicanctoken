using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCanTok.Commands;

namespace WebApiCanTok.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CancelTokenController : ControllerBase
    {
        private readonly ILogger<CancelTokenController> _logger;
        private readonly IMediator _mediator;

        public CancelTokenController(
            ILogger<CancelTokenController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("checktoken")]
        public async Task<IActionResult> WithCancellationTokenChecking(CancellationToken token)
        {
            try
            {
                await _mediator.Send(
                    new CommandWithCancTokenHandling(true), 
                    token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Request has been cancelled");
                return BadRequest("operation cancelled");
            }

            return Ok("operation succeeded");
        }


        [HttpGet("notchecktoken")]
        public async Task<IActionResult> WithoutCancellationTokenChecking(CancellationToken token)
        {
            try
            {
                await _mediator.Send(
                    new CommandWithCancTokenHandling(false), 
                    token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Request has been cancelled");
                return BadRequest("operation cancelled");
            }

            return Ok("operation succeeded");
        }


        [HttpGet("nothttprequesttoken")]
        public async Task<IActionResult> WithCancellationTokenWithDifferentSourceAndChecking()
        {
            try
            {
                await _mediator.Send(
                    new CommandWithCancTokenHandling(true));
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Request has been cancelled");
                return BadRequest("operation cancelled");
            }

            return Ok("operation succeeded");
        }


        [HttpGet("checktokenandpasstoothermethods")]
        public async Task<IActionResult> WithCancellationTokenCheckingAndPassingToOtherMethods(CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(
                    new CommandWithCancTokenHandling(
                        handleCancellationToken: true, 
                        passCancellationTokenToOtherMethods: true),
                    cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Request has been cancelled");
                return BadRequest("operation cancelled");
            }

            return Ok("operation succeeded");
        }
    }
}