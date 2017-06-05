using Xamarin.Forms;

namespace XamTweet.Controls
{
    public class LimitedEntry : Entry
    {
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength),
            typeof(int),
            typeof(LimitedEntry),
            140);

        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }

        public LimitedEntry() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.TextChanged += (sender, args) =>
            {
                string _text = this.Text;
                if (_text.Length > MaxLength)
                {
                    _text = _text.Remove(_text.Length - 1);
                    this.Text = _text;
                }
            };
        }
    }
}
