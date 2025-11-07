namespace PosterBoi.Core.Configs
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        private Result(bool success, T? data, string? message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static Result<T> Ok(T data) => new Result<T>(true, data, null);
        public static Result<T> Fail(string message) => new Result<T>(false, default, message);
    }

}
