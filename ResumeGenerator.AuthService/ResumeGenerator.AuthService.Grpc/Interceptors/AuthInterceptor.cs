using System.Threading.Tasks;
using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using ResumeGenerator.AuthService.Application.Exceptions;

public sealed class AuthInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>   (
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (UserNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (InvalidCredentialsException ex)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
        }
        catch (InvalidTokenException ex)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
        }
        catch (UserNotActiveException ex)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
        }
        catch (UserAlreadyExistsException ex)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (UserAlreadyActiveException ex)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }
}