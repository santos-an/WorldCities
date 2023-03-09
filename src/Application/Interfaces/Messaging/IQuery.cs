using MediatR;

namespace Application.Interfaces.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse> { }