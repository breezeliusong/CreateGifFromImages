using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

//https://msdn.microsoft.com/en-us/library/mt598521.aspx?f=255&MSPPError=-2147217396 render document

//https://github.com/Microsoft/Lumia-imaging-sdk   Lumia SDK in GIThub

namespace GifTest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var sourceFile0 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("pic.gif");
            var sourceFile1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("pic1.gif");
            var sourceFile2 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("pic2.gif");
            var sourceFile3 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("pic3.gif");

            BitmapImage bitmap;

            IBuffer buffer;
            using (var source0 = new StorageFileImageSource(sourceFile0))
            {
                var source1 = new StorageFileImageSource(sourceFile1);
                var source2 = new StorageFileImageSource(sourceFile2);
                var source3 = new StorageFileImageSource(sourceFile3);
                var sources = new List<IImageProvider>();
                sources.Add(source0);
                sources.Add(source1);
                sources.Add(source2);
                sources.Add(source3);
                using (var gifRenderer = new GifRenderer(sources))
                {
                    gifRenderer.Size = new Size(400, 400);
                    gifRenderer.Duration = 100;
                    buffer = await gifRenderer.RenderAsync();
                }
            }

            //save the gif 
            var filename = "myFile1.gif";
            var storageFile = await KnownFolders.SavedPictures.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await stream.WriteAsync(buffer);
            }

            //FileRandomAccessStream stream1 = (FileRandomAccessStream)await storageFile.OpenAsync(FileAccessMode.Read);
            //bitmap.SetSource(stream1);

            //show the gif
            bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(buffer.AsStream().AsRandomAccessStream());
            MyImage.Source = bitmap;

        }

    }
}

