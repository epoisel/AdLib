using System;

namespace AdLib.Automation.Actions
{
    public class InputTextAction : BaseAction
    {
        private string _inputText;


        public InputTextAction()
            : base("Input Text Action", "Inputs specified text into a field.")
        {
        }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(_inputText))
                throw new InvalidOperationException("Input text is not set.");

            // Logic to input text
            Console.WriteLine($"Inputting text: {_inputText}");
            OnActionCompleted(EventArgs.Empty);
        }

        public override void Validate()
        {
            base.Validate();
            if (string.IsNullOrEmpty(_inputText))
                throw new InvalidOperationException("Input text is not set.");
        }

        public void SetInputText(string inputText)
        {
            _inputText = inputText;
        }

        public override object GetConfiguration()
        {
            // Return properties to be configured for this action
            return new { InputText = _inputText };
        }
    }
}
