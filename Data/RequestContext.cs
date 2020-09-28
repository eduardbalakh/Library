using System;
using System.Collections.Generic;

namespace Library.Data
{
    class RequestContext
    {
        public readonly string Message;
        public readonly DateTime Dt;
        public readonly bool IsError;
        public readonly List<WordEntity> WordList;

        public RequestContext()
        {
            Dt = DateTime.Now;
            Message = String.Empty;
            IsError = false;
        }

        public RequestContext(string inMsg, bool error)
        {
            Message = inMsg;
            Dt = DateTime.Now;
            IsError = error;
        }

        public RequestContext(string inMsg, bool error, List<WordEntity> list)
        {
            Message = inMsg;
            Dt = DateTime.Now;
            IsError = error;
            WordList = list;
        }

        public override string ToString()
        {
            if (WordList==null) return $"{Dt.ToShortTimeString()}: {Message}";
            return WordList.Count > 0 ? string.Join("\n", WordList) : $"нет подходящих слов";
        }
    }
}
