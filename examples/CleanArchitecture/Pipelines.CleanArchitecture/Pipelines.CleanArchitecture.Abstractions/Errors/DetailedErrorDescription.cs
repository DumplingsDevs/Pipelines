namespace Pipelines.CleanArchitecture.Abstractions.Errors;

public record DetailedErrorDescription
{
    public string ErrorCode { get; }
    public string ErrorMessage { get; }
    public string? FieldName { get; }
    
    public DetailedErrorDescription(string errorCode, string errorMessage, string fieldName)
    {
        this.ErrorCode = errorCode;
        this.ErrorMessage = errorMessage;
        this.FieldName = fieldName;
    }
    
    public DetailedErrorDescription(string errorCode, string errorMessage)
    {
        this.ErrorCode = errorCode;
        this.ErrorMessage = errorMessage;
    }
}