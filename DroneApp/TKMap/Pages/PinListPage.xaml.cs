using System;
using System.Collections.Generic;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.TKMap.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinListPage : ContentPage
    {
        public event EventHandler<PinSelectedEventArgs> PinSelected;

        readonly IEnumerable<TKCustomMapPin> _pins;


        public PinListPage(IEnumerable<TKCustomMapPin> pins)
        {
            InitializeComponent();

            _pins = pins;
            BindingContext = _pins;

            _lvPins.ItemSelected += (o, e) =>
            {
                if (_lvPins.SelectedItem == null) return;

                OnPinSelected((TKCustomMapPin)_lvPins.SelectedItem);
            };
        }
        protected virtual void OnPinSelected(TKCustomMapPin pin)
        {
            PinSelected?.Invoke(this, new PinSelectedEventArgs(pin));
        }
    }
    public class PinSelectedEventArgs : EventArgs
    {
        public TKCustomMapPin Pin { get; set; }

        public PinSelectedEventArgs(TKCustomMapPin pin)
        {
            Pin = pin;
        }

    }
}