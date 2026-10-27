using System;
using System.Collections.Generic;
using System.Text;

namespace WPO.Application.Exceptions
{
    public class ValidationException(string message, string prop) : Exception(message)
    {
        public string Property { get; protected set; } = prop;
    }
}
