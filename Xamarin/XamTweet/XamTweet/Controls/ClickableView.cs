using System.Windows.Input;
using Xamarin.Forms;

namespace XamTweet.Controls
{
    public class ClickableView : StackLayout
    {
        #region Properties
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand),
            typeof(ICommand),
            typeof(ClickableView),
            null);

        public ICommand TappedCommand
        {
            get
            {
                return (ICommand)GetValue(TappedCommandProperty);
            }
            set
            {
                SetValue(TappedCommandProperty, value);
            }
        }
        
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ClickableView), null);

        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }
        #endregion

        public ClickableView() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += (s, e) => {
                TappedCommand.Execute(this.CommandParameter ?? this);
            };

            this.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
