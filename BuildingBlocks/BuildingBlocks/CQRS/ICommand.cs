using MediatR;

namespace BuildingBlocks.CQRS
{
    // unit represents void type for the mediator
    // empty command that doesn't return any response
    public interface ICommand : ICommand<Unit> { }

    // for commands that produce a response
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
