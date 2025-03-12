namespace ASH.IntegrationTests.Fakes;
public static class ErrorDictionaryExtensions
{
    public static FakeChatClientError? GetErrorForMethod(this Dictionary<string, FakeChatClientError> errors, string methodName)
    {
        if (errors.ContainsKey(methodName))
        {
            var error = errors[methodName];
            errors.Remove(methodName);
            return error;
        }
        return null;
    }
}