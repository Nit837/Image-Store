using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImageBhandaar.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private byte[] editedimage=null;
        #region Properties
        public byte[] EditedImage
        {
            get
            {
                return editedimage;
            }
            set
            {
                editedimage = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region INotifypropertychanged Event raised
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public BaseViewModel()
        {

        }
    }
}
