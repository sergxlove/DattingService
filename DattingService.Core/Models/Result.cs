namespace DattingService.Core.Models
{
    public class Result<T>
    {
        public T Value { get; }
        public string Error { get; }
        public bool IsSuccess
        {
            get
            {
                if (Error == string.Empty) return true;
                return false;
            }
        }

        private Result(T value, string error)
        {
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(value, string.Empty);

        public static Result<T> Failure(string error) => new Result<T>(default!, error);
    }
}
