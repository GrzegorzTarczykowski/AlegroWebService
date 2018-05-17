using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AlegroWebService.serviceWebapi;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AlegroWebService
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        servicePortClient service;
        string key = "5d404fca";
        int countryCode = 1;
        public MainPage()
        {
            this.InitializeComponent();
            service = new servicePortClient();
        }

        private async void btSzukaj_Click(object sender, RoutedEventArgs e)
        {
            lbxPobrane.Items.Clear();
            FilterOptionsType[] filtr =     
            {
                new FilterOptionsType()
                {
                    filterId ="search",
                    filterValueId =new string []{tbSzukane.Text}
                }
            };
            doGetItemsListRequest pytanie = new doGetItemsListRequest()
            {
                filterOptions = filtr,
                countryId = countryCode,
                webapiKey = key,
                resultOffset = 0,
                resultScope = 0,
                resultSize = 0
            };
            doGetItemsListResponse wynik = null;
            try
            {
                wynik = await service.doGetItemsListAsync(pytanie);
            }
            catch (Exception ex)
            {
                ContentDialog dlg = new ContentDialog()
                {
                    Title = "Transmisja danych",
                    Content = "problem z czytaniem z allegro.pl: " + ex.Message
                };
                await dlg.ShowAsync();
            }
            if (wynik == null) return;
            if (wynik.itemsCount == 0) lbxPobrane.Items.Add("Brak przedmiotow");
            //foreach (var item in wynik.categoriesList.categoriesTree)
            //    lbxPobrane.Items.Add(string.Format("{0,12}: {1,40}", item.categoryId, item.categoryName));
            foreach (var item in wynik.itemsList)
                lbxPobrane.Items.Add(string.Format("{0,12}: {1,40}: {2,15}: {3,15:F2}", item.itemId, item.itemTitle, item.sellerInfo.userLogin, item.priceInfo[0].priceValue));
        }
    }
}
