using HV.BLL.Exceptions.Abstractions;

namespace HV.BLL.Exceptions;

public sealed class NotFoundException(string message) : CustomExceptionBase(message);