namespace CleanArchitecture.Application.Exceptions;

public class FileSizeException : Exception
{
    public FileSizeException()
        : base("You can't upload file more than 5 MB")
    { }
}
