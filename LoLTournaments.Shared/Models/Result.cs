using System;
using System.Collections.Generic;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    public readonly struct Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }


        public Result(string error)
        {
            IsSuccess = false;
            Error = error;
            LogIfError();
        }
        
        public Result(bool success)
        {
            IsSuccess = success;
            Error = null;
            LogIfError();
        }
        
        public Result(bool success, string error)
        {
            IsSuccess = success;
            Error = error;
            LogIfError();
        }

        public Result(Exception error) : this(error?.Message) {}

        public static Result Null()
        {
            return new Result(new NullReferenceException("Data is null"));
        }
        
        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Failure(object error = null)
        {
            return new Result(error?.ToString());
        }
        
        public static Result Cancelled(object error = null)
        {
            return error == null ? new Result("Operation was cancelled") : new Result(error.ToString());
        }

        public static implicit operator bool(Result data)
        {
            return data.IsSuccess;
        }

        public bool Equals(Result other)
        {
            return IsSuccess == other.IsSuccess && Error == other.Error;
        }

        public override bool Equals(object obj)
        {
            return obj is Result other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (IsSuccess.GetHashCode() * 397) ^ (Error != null ? Error.GetHashCode() : 0);
            }
        }


        public override string ToString()
        {
            return Error;
        }
        
        private void LogIfError()
        {
            if (string.IsNullOrEmpty(Error))
                return;
            
            DefaultSharedLogger.Error(this);
        }
    }

    public readonly struct Result<TResponce>
    {
        public bool Success { get; }
        public string Error { get; }

        public TResponce Value { get; }

        public Result(TResponce data)
        {
            Value = data;
            Error = null;
            Success = true;
            LogIfError();
        }
        
        public Result(bool success, string error)
        {
            Success = success;
            Error = error;
            Value = default;
            LogIfError();
        }
        
        public Result(bool success, TResponce value)
        {
            Success = success;
            Error = value == null 
                ? $"{nameof(NullReferenceException)} : Result from {typeof(TResponce).Name} is missing, result of {nameof(Success)} : {success}"
                : null;
            Value = value;
            LogIfError();
        }
        
        public Result(bool success, TResponce value, string error)
        {
            Success = success;
            Error = error;
            Value = value;
            LogIfError();
        }

        public Result(Exception error) : this(false, error?.Message) {}
        

        public static implicit operator bool(Result<TResponce> data)
        {
            return data.Success && data.Value != null;
        }

        public static implicit operator Result(Result<TResponce> data)
        {
            return new Result(data.Success, data.Error);
        }

        public static implicit operator Result<TResponce>(Result data)
        {
            return new Result<TResponce>(data.IsSuccess, data.Error);
        }

        public static implicit operator Result<TResponce>(TResponce data)
        {
            return new Result<TResponce>(data);
        }

        public bool Equals(Result<TResponce> other)
        {
            return Success == other.Success
                   && Error == other.Error
                   && EqualityComparer<TResponce>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Result<TResponce> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Success.GetHashCode();
                hashCode = (hashCode * 397) ^ (Error != null ? Error.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? EqualityComparer<TResponce>.Default.GetHashCode(Value) : 0);
                return hashCode;
            }
        }


        public override string ToString()
        {
            return Success
                ? $"{nameof(Success)} : {Success}, {typeof(TResponce).Name} : {Value}"
                : $"{nameof(Success)} : {Success}, {typeof(TResponce).Name} : {Value}, {nameof(Error)} : {Error}";
        }

        private void LogIfError()
        {
            if (string.IsNullOrEmpty(Error))
                return;
            
            DefaultSharedLogger.Error(Error);
        }
    }
}