using ImageBhandaar.ColorSliderHelper;
using Plugin.Screenshot;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ImageBhandaar.ViewModel
{
    public class Current : BaseViewModel
    {

        public delegate void CallbackEventHandler(string ImagePath);
        private ImageSource _Image;
        public ImageSource Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                OnPropertyChanged();
            }
        }
        public event CallbackEventHandler Callback;
        public string SelectedImage { get; set; }
        public string CommentColor { get; set; }
        public string StrokeColor { get; set; }
        public double ColorSliderValue { get; set; }
        public double ScratchSliderValue { get; set; }
        public ICommand SaveImageCommand { get; set; }
        public ICommand CropCommand { get; set; }
        public ICommand ClosePageCommand { get; set; }
        public Current(string _SelectedImage)
        {
            SelectedImage = _SelectedImage;
            CommentColor = StrokeColor = "Green";
            SaveImageCommand = new Command(SaveImageCommandExecute);
            CropCommand = new Command(CropImageCommandExecute);
            ClosePageCommand = new Command(ClosePageCommandExecute);
        }
        public Current()
        {
            SelectedImage = App.Currentpickedimage;
            CommentColor = StrokeColor = "Green";
            SaveImageCommand = new Command(SaveImageCommandExecute);
            CropCommand = new Command(CropImageCommandExecute);
            ClosePageCommand = new Command(ClosePageCommandExecute);
        }
        private async void ClosePageCommandExecute(object obj)
        {
            var selectedOption = await App.Current.MainPage.DisplayAlert("Discard Photo",
                 "if you discard now,you'll lose your photos and edits.", "Discard", "Keep");
            if (selectedOption) await App.Current.MainPage.Navigation.PopModalAsync();
        }
        private void OnScratchSliderValueChanged()
        {
            var colorvalue = Convert.ToInt32(ScratchSliderValue);
            var selectedcolor = SliderColorsList.SliderColors.FirstOrDefault(x => x.ID == colorvalue);
            StrokeColor = selectedcolor.ColorOnHEX;
        }
        private void OnColorSliderValueChanged()
        {
            var colorvalue = Convert.ToInt32(ColorSliderValue);
            var selectedcolor = SliderColorsList.SliderColors.FirstOrDefault(x => x.ID == colorvalue);
            CommentColor = selectedcolor.ColorOnHEX;
        }
        private async void SaveImageCommandExecute(object obj)
        {
            using (var image = await Plugin.ImageEdit.CrossImageEdit.Current.CreateImageAsync(App.ImageToEdit.GetStream()))
            {
                var data = image.Resize(200).ToPng();
                App.EditedPicture = data;
                Image = ImageSource.FromStream(() => new MemoryStream(data));
            }
            //var editorPage = obj as EditImage;

            //var signaturepad = editorPage.Content.FindByName("signaturepad") as SignaturePadView;
            //signaturepad.ClearLabel.IsVisible = false;

            //var gritoolbar = editorPage.Content.FindByName("gridtoolbar") as Grid;
            //gritoolbar.IsVisible = false;

            //var savebutton = editorPage.Content.FindByName("savebutton") as Button;
            //savebutton.IsVisible = false;

            //var imgcolors = editorPage.Content.FindByName("imgcolors") as Image;
            //imgcolors.IsVisible = false;

            //var commentslider = editorPage.Content.FindByName("commentcolorslider") as Slider;
            //commentslider.IsVisible = false;

            //var scratchslider = editorPage.Content.FindByName("scratchcolorslider") as Slider;
            //scratchslider.IsVisible = false;

            //string path = await CrossScreenshot.Current.CaptureAndSaveAsync();
            //App.Currentpickedimages = path;
            //// await App.Current.MainPage.Navigation.PopModalAsync();

            //Callback?.Invoke(path);
        }
        public async void CropImageCommandExecute()
        {
            using (var image = await Plugin.ImageEdit.CrossImageEdit.Current.CreateImageAsync(App.ImageToEdit.GetStream()))
            {
                var data = image.Crop(10, 20, 250, 100);
                //Image = ImageSource.FromStream(() => new MemoryStream(data));
            }
        }
        public static async Task EditImage(string ImagePath, CallbackEventHandler callbackEventHandler)
        {
            var imgviewmodel = new Current(ImagePath);
            imgviewmodel.Callback += callbackEventHandler;
            var imgpage = new EditImage(ImagePath);
            imgpage.BindingContext = imgviewmodel;
            await App.Current.MainPage.Navigation.PushModalAsync(imgpage, true);
        }
    }
}
