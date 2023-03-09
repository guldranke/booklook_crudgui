using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using booklook_crudgui.Helpers;
using booklook_crudgui.ViewModels;
using booklook_crudgui.Models;
using System.Diagnostics;
using System.Linq;

namespace booklook_crudgui {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        /// <summary>
        ///     On window load, GET the books and update the context
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WindowLoaded(object sender, RoutedEventArgs e) {
            RestService restService = new();

            try {
                List<Book> books = await restService.GetBooks();
                Book firstBook = books.First();
                MainViewModel context = new() {
                    Books = books,
                    SelectedBookContext = new BookViewModel().CreateFromBook(firstBook)
                };
                DataContext = context;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                Application.Current.Shutdown();
            } finally {
                MainViewModel context = (MainViewModel)DataContext;
                context.BooksLoading = false;
                DataContext = context;
            }
        }

        /// <summary>
        ///     On book change, update the content view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedBookChanged(object sender, SelectionChangedEventArgs e) {
            Book book = (Book)((ListBox)sender).SelectedItem;

            if (book == null) return;

            Trace.WriteLine($"BOOK -> {book.Id} : {book.Title}");

            BookViewModel selectedBookContext = new BookViewModel().CreateFromBook(book);

            MainViewModel context = (MainViewModel)DataContext;
            context.SelectedBookContext = selectedBookContext;
            DataContext = context;
        }

        /// <summary>
        ///     Make the window draggable as we have WindowStyle="None" in MainWindow.xaml
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        /// <summary>
        ///     Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButtonClicked(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}
