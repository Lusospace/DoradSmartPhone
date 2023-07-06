namespace DoradSmartphone.Helpers
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(NumericValidationBehavior), double.MaxValue);

        public double MaximumValue
        {
            get { return (double)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                // Remove non-numeric characters
                string newValue = new string(e.NewTextValue?.Replace(",", "").Where(char.IsDigit).ToArray());

                // Limit the value to the specified maximum
                if (!string.IsNullOrWhiteSpace(newValue) && double.TryParse(newValue, out double numericValue))
                {
                    if (numericValue > MaximumValue)
                        newValue = MaximumValue.ToString();

                    entry.Text = newValue;
                }
                else
                {
                    entry.Text = string.Empty;
                }
            }
        }
    }
}
