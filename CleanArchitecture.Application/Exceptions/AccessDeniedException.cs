namespace CleanArchitecture.Application.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException()
        : base("You are not allowed to make this action") { }
    
}
