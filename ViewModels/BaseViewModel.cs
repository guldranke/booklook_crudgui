﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace booklook_crudgui.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        /// <summary>
        ///     Notify on property change helper method
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string? name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
