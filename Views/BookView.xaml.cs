using booklook_crudgui.Helpers;
using booklook_crudgui.ViewModels;
using booklook_crudgui.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}
