using System;
using Microsoft.Office.Interop.Outlook;
using OlDocPublish.Snipping;

namespace OlDocPublish
{
    public interface IDocumentProcessor
    {
        event EventHandler<SnipInfo> SnipRequested;
        event EventHandler<SnipInfo> SnipIterated;
        event EventHandler SnippingComplete;
        event EventHandler<SnipInfo> SnipAfter;
        event EventHandler<SnipInfo> BeforeSnipping;

        string SO {get;}
        bool ProcessMailItem(MailItem mail);
    }
}