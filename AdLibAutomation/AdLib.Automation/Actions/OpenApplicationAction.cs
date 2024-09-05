using System;
using System.Diagnostics;

namespace AdLib.Automation.Actions
{
    public class OpenApplicationAction : BaseAction
    {
        private string _applicationPath;

        public OpenApplicationAction()
            : base("Open Application Action", "Opens a specified application.")
        {
        }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(_applicationPath))
                throw new InvalidOperationException("Application path is not set.");

            try
            {
                Process.Start(_applicationPath);
                OnActionCompleted(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open application: {ex.Message}");
            }
        }

        public override void Validate()
        {
            base.Validate();
            if (string.IsNullOrEmpty(_applicationPath))
                throw new InvalidOperationException("Application path is not set.");
        }

        public void SetApplicationPath(string applicationPath)
        {
            _applicationPath = applicationPath;
        }

        public override object GetConfiguration()
        {
            // Return properties to be configured for this action
            return new { ApplicationPath = _applicationPath };
        }
    }
}
