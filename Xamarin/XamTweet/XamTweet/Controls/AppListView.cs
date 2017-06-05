using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamTweet.Controls
{
    public class AppListView : ListView
    {
        #region Commands
        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(nameof(ItemTappedCommand),
            typeof(ICommand),
            typeof(AppListView),
            null);

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)GetValue(ItemTappedCommandProperty); }
            set
            {
                SetValue(ItemTappedCommandProperty, value);
            }
        }

        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand),
            typeof(ICommand),
            typeof(AppListView),
            null);

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set
            {
                SetValue(LoadMoreCommandProperty, value);
            }
        }
        #endregion

        public AppListView(ListViewCachingStrategy stategy) : base()
        {
            Initialize();
        }

        public AppListView() : this(ListViewCachingStrategy.RecycleElement)
        {
        }

        private void Initialize()
        {
            this.ItemSelected += (s, e) =>
            {
                if (e.SelectedItem == null || ItemTappedCommand == null) return;

                ItemTappedCommand.Execute(e.SelectedItem);

                Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                {
                    this.SelectedItem = null;
                    return false;
                });
            };

            this.ItemAppearing += (sender, e) =>
            {
                if (LoadMoreCommand == null) return;

                LoadMoreCommand.Execute(e.Item);
            };
        }
    }
}
