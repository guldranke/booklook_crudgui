using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using booklook_crudgui.Models;

namespace booklook_crudgui.ViewModels {
    public class BookViewModel : BaseViewModel {
        private long _id;
        public long Id {
            get => _id; set {
                _id = value;
                OnPropertyChanged();
            }
        }
        private string _title = string.Empty;
        public string Title {
            get => _title; set {
                _title = value;
                OnPropertyChanged();
            }
        }
        private string _authors = string.Empty;
        public string Authors {
            get => _authors; set {
                _authors = value;
                OnPropertyChanged();
            }
        }
        private string _imageSource = string.Empty;
        public string ImageSource {
            get => _imageSource; set {
                _imageSource = value;
                OnPropertyChanged();
            }
        }
        private string _isbn13 = string.Empty ;
        public string Isbn13 {
            get => _isbn13; set {
                _isbn13 = value;
                OnPropertyChanged();
            }
        }
        private string _isbn10 = string.Empty;
        public string Isbn10 {
            get => _isbn10; set {
                _isbn10 = value;
                OnPropertyChanged();
            }
        }
        private string _releaseDate = string.Empty;
        public string ReleaseDate {
            get => _releaseDate; set {
                _releaseDate = value;
                OnPropertyChanged();
            }
        }
        private string _bookLink = string.Empty;
        public string BookLink {
            get => _bookLink; set {
                _bookLink = value;
                OnPropertyChanged();
            }
        }
        private string _location = string.Empty;
        public string Location {
            get => _location; set {
                _location = value;
                OnPropertyChanged();
            }
        }

        public static BookViewModel CreateFromBook(Book book) {
            return new BookViewModel() {
                Id = book.Id,
                Title = book.Title,
                Authors = book.Authors,
                Isbn13 = book.Isbn13,
                Isbn10 = book.Isbn10,
                ImageSource = book.ImageSource,
                ReleaseDate = book.ReleaseDate,
                BookLink = book.BookLink,
                Location = book.Location,
            };
        }
    }
}
