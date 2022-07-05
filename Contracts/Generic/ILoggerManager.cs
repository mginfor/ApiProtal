using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Generic
{
    public interface ILoggerManager
    {
        void logInformation(string message);
        void logDebug(string message);
        void logWarning(string message);
        void logError(string message);
    }
}
