namespace Kiero.Net;

public enum KieroStatus
{
    UnknownError = -1,
    NotSupportedError = -2,
    ModuleNotFoundError = -3,
    AlreadyInitializedError = -4,
    NotInitializedError = -5,
    Success = 0,
}