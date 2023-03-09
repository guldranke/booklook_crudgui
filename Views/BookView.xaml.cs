using booklook_crudgui.Helpers;
using booklook_crudgui.ViewModels;
using booklook_crudgui.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System;

namespace booklook_crudgui.Views {
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : UserControl {
        public BookView() {
            InitializeComponent();
        }

        /// <summary>
        ///     On button click, delete the selected book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButtonClick(object sender, RoutedEventArgs e) {
            long targetId = ((BookViewModel)DataContext).Id;

            try {
                RestService restService = new();
                await restService.DeleteBook(targetId);

                MainWindow mw = ((MainWindow)Application.Current.MainWindow);
                mw.Dispatcher.Invoke(() => {
                    MainViewModel mwContext = (MainViewModel)mw.DataContext;
                    Book selectedBook = mwContext.Books.Find(book => book.Id == targetId)!;

                    int index = mwContext.Books.IndexOf(selectedBook);
                    if (index == -1) return;

                    List<Book> filteredBooks = mwContext.Books.Where(book => book.Id != targetId).ToList();

                    if (filteredBooks.Count != 0) {
                        // -1 the index if != 0 as we filter out the book and all indices shift
                        Book newSelectedBook = filteredBooks[index != 0 ? --index : index];
                        mwContext.SelectedBookContext = BookViewModel.CreateFromBook(newSelectedBook);
                    } else {
                        mwContext.SelectedBookContext = null;
                    }


                    mwContext.Books = filteredBooks;
                    mw.DataContext = mwContext;
                });

                ((BookViewModel)DataContext).NotificationContent = "Succesfully deleted the book";
            } catch {
                MessageBox.Show("There was an error deleting the book", "Error", MessageBoxButton.OK);
            }
        }

        /// <summary>
        ///     On button click, update the selected book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateButtonClick(object sender, RoutedEventArgs e) {
            BookViewModel context = (BookViewModel)DataContext;
            RestService restService = new();

            try {
                await restService.PutBook(Book.CreateFromContext(context));
                MainWindow mw = ((MainWindow)Application.Current.MainWindow);
                mw.Dispatcher.Invoke(() => {
                    MainViewModel mwContext = (MainViewModel)mw.DataContext;

                    Book selectedBook = mwContext.Books.Find(book => book.Id == context.Id)!;
                    int index = mwContext.Books.IndexOf(selectedBook);
                    if (index == -1) return;

                    // clone the list so it rerenders
                    List<Book> books = new(mwContext.Books) {
                        [index] = Book.CreateFromContext(context)
                    };

                    mwContext.Books = books;
                    mw.DataContext = mwContext;
                });

                context.NotificationContent = "Succesfully updated the book";
            } catch {
                MessageBox.Show("There was an error updating the book", "Error", MessageBoxButton.OK);
            }
        }

        /// <summary>
        ///     Remove the notifcation after 5 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            Border notification = (Border)sender;

            if (!((bool)e.NewValue) || notification.Visibility == Visibility.Hidden) {
                return;
            }

            BookViewModel context = (BookViewModel)DataContext;
            Delay(5000, () => context.NotificationContent = null);
        }

        /// <summary>
        ///     Delay helper method, only used for NotificationIsVisibleChanged
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="action"></param>
        private static void Delay(int milliseconds, Action action) {
            var t = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(milliseconds) };
            t.Tick += (o, e) => { t.Stop(); action.Invoke(); };
            t.Start();
        }
    }
}
