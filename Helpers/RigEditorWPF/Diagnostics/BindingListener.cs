using System;
using System.Diagnostics;
using System.Reflection;

namespace s3piwrappers.RigEditor.Diagnostics
{
    public sealed class BindingListener : DefaultTraceListener
    {
        private string Callstack { get; set; }
        private string DateTime { get; set; }
        private int InformationPropertyCount { get; set; }
        private bool IsFirstWrite { get; set; }
        private string LogicalOperationStack { get; set; }
        private string Message { get; set; }
        private string ProcessId { get; set; }
        private string ThreadId { get; set; }
        private string Timestamp { get; set; }

        public BindingListener(TraceOptions options)
            : base()
        {
            this.IsFirstWrite = true;
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(this);
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            this.TraceOutputOptions = options;
            this.DetermineInformationPropertyCount();
        }

        private void DetermineInformationPropertyCount()
        {
            foreach (TraceOptions traceOptionValue in Enum.GetValues(typeof(TraceOptions)))
            {
                if (traceOptionValue != TraceOptions.None)
                {
                    this.InformationPropertyCount += this.GetTraceOptionEnabled(traceOptionValue);
                }
            }
        }

        private int GetTraceOptionEnabled(TraceOptions option)
        {
            return (this.TraceOutputOptions & option) == option ? 1 : 0;
        }

        public override void WriteLine(string message)
        {
            if (this.IsFirstWrite)
            {
                this.Message = message;
                this.IsFirstWrite = false;
            }
            else
            {
                var propertyInformation = message.Split(new string[] { "=" }, StringSplitOptions.None);

                if (propertyInformation.Length == 1)
                {
                    this.LogicalOperationStack = propertyInformation[0];
                }
                else
                {
                    this.GetType().GetProperty(propertyInformation[0],
                        BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance)
                        .SetValue(this, propertyInformation[1], null);
                }

                this.InformationPropertyCount--;
            }

            this.Flush();

            if (this.InformationPropertyCount == 0)
            {
                PresentationTraceSources.DataBindingSource.Listeners.Remove(this);
                throw new BindingException(this.Message,
                    new BindingExceptionInformation(this.Callstack,
                        System.DateTime.Parse(this.DateTime),
                        this.LogicalOperationStack, int.Parse(this.ProcessId),
                        int.Parse(this.ThreadId), long.Parse(this.Timestamp)));
            }
        }
    }
    
}
