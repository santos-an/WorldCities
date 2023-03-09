using MediatR;

namespace Application.Interfaces.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse> { }
