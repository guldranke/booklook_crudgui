﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using booklook_crudgui.Helpers;
using booklook_crudgui.ViewModels;
using booklook_crudgui.Models;
using System.Diagnostics;

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
                MainViewModel context = new() {
                    Books = books,
                };
                DataContext = context;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                Application.Current.Shutdown();
            } finally {
                MainViewModel context = (MainViewModel)DataContext;
                context.BooksLoading = false;
            }
        }

        /// <summary>
        ///     On book change, update the content view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedBookChanged(object sender, SelectionChangedEventArgs e) {
            MainViewModel context = (MainViewModel)DataContext;
            Book book = (Book)((ListBox)sender).SelectedItem;

            if (book == null) return;
            bool sameBook = context.SelectedBookContext != null && context.SelectedBookContext.Id == book.Id;
            if (sameBook) return;

            Trace.WriteLine($"BOOK -> {book.Id} : {book.Title}");

            BookViewModel selectedBookContext = BookViewModel.CreateFromBook(book);

            context.SelectedBookContext = selectedBookContext;
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
