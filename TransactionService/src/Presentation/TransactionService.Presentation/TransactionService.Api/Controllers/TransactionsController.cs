using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Api.ExceptionHandlers;
using TransactionService.Application.Transactions.Commands.CreateTransaction;
using TransactionService.Application.Transactions.Models;
using TransactionService.Application.Transactions.Queries.GetTransactionByTransferId;

namespace TransactionService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ISender _mediator;
    public TransactionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{transferId:guid}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TransactionDto>> GetByTransferId([FromRoute] Guid transferId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetTransactionByTransferIdQuery(transferId), cancellationToken);
        return response;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateTransactionResponse>> Create([FromBody] CreateTransactionRequest transactionRequest,
        CancellationToken cancellationToken)
    {
        var command = new CreateTransactionCommand(transactionRequest);
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}
