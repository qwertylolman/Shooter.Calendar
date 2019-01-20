using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.ViewModels.Abstract
{
    public abstract class GenericListViewModel<TItem> : PageViewModel
    {
        protected GenericListViewModel()
        {
            ObservableCollection = new MvxObservableCollection<TItem>();

            ItemSelectedCommand = new MvxAsyncCommand<TItem>(ItemSelectedAsync);
        }

        public TItem SelectedItem { get; private set; }

        public ICommand ItemSelectedCommand { get; }

        public IEnumerable ItemsCollection 
            => ObservableCollection;

        public virtual MvxObservableCollection<TItem> ObservableCollection { get; private set; }

        protected override Task InitializeAsync()
        {
            ObservableCollection.CollectionChanged -= OnCollectionChanged;
            ObservableCollection.CollectionChanged += OnCollectionChanged;

            return base.InitializeAsync();
        }

        private Task ItemSelectedAsync([NotNull] TItem item)
        {
            SelectedItem = item;
            return OnItemSelectedAsync(item);
        }

        protected virtual async Task OnItemSelectedAsync(TItem item)
        {
            if (item is IListItem listItem)
            {
                await listItem.SelectCommand.ExecuteAsync(item);
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged();
        }

        protected virtual void OnCollectionChanged()
        {
        }

        protected override async Task LoadDataAsync(CancellationToken ct)
        {
            var items = await GetItemsAsync(ct);
            if (ct.IsCancellationRequested == true)
            {
                return;
            }

            ReplaceList(ObservableCollection, items);
        }

        protected virtual void ReplaceList(MvxObservableCollection<TItem> collection, IEnumerable<TItem> newItems)
        {
            collection.ReplaceWith(newItems);
        }

        protected virtual Task<IEnumerable<TItem>> GetItemsAsync(CancellationToken ct)
            => Task.FromResult(new TItem[0] as IEnumerable<TItem>);
    }
}
