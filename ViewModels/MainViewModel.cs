using booklook_crudgui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace booklook_crudgui.ViewModels {
    public class MainViewModel : BaseViewModel {
        private List<Book> _books = new();
        public List<Book> Books {
            get => _books; set {
                _books = value;
                OnPropertyChanged();
                OnPropertyChanged("BookIndex");
            }
        }

        public int BookIndex {
            get {
                if (_selectedBookContext == null) return 0;

                Book selectedBook = _books.Find(book => book.Id == _selectedBookContext.Id)!;
                return _books.IndexOf(selectedBook);
            }
        }

        private bool _booksLoading = true;
        public bool BooksLoading {
            get => _booksLoading; set {
                _booksLoading = value;
                OnPropertyChanged();
            }
        }

        private BookViewModel? _selectedBookContext;
        public BookViewModel? SelectedBookContext {
            get => _selectedBookContext; set {
                _selectedBookContext = value;
                OnPropertyChanged();
            }
        }
    }
}
