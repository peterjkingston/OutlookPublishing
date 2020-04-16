using System;

namespace OlDocPublish.Snipping
{
    public interface IPublisher
    {
        event EventHandler<string> FileCreated;
        void Publish(string sourcePath, string so, string label, int start, int end, bool eof = false);
    }
}