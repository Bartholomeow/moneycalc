using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager
{
    class IncorrectDataException : ApplicationException
    {
        public IncorrectDataException() { }

        public IncorrectDataException(string message) : base(message) { }

        public IncorrectDataException(string message, Exception inner) : base(message, inner) { }

        protected IncorrectDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
