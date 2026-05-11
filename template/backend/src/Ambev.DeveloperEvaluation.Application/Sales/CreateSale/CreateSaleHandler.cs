using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using Ambev.DeveloperEvaluation.Application.Sales.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = new Sale
        {
            SaleNumber = request.SaleNumber,
            Customer = request.Customer,
            Branch = request.Branch,
            Date = DateTime.UtcNow
        };

        foreach (var itemCommand in request.Items)
        {
            var item = new SaleItem(itemCommand.Product, itemCommand.Quantity, itemCommand.UnitPrice);
            sale.AddItem(item);
        }

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Differential: Publish Event
        await _mediator.Publish(new SaleCreatedEvent(createdSale), cancellationToken);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}
