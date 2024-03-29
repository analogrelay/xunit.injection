using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Xunit.Injection.Tests
{
    public class LoggingService : ILoggingService
    {
        private ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

        public void Log(string message)
        {
            _messages.Enqueue(message);
        }

        public IReadOnlyList<string> GetMessages() => _messages.ToArray();
    }
}
