using Citizen.Framework;
using Citizen.Feature.Landing;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Citizen.Feature.Landing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : BaseContentPage<HomeViewModel>
    {
        public HomePage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            App.NavigationRoot = this;
            BindingContext = new HomeViewModel();
        }

        private void OncanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var mobileColor = (string) Application.Current.Resources["PrimaryDarkFormHomeBgImageString"];
            var solidColor = SkiaSharp.SKColor.Parse(mobileColor);

            var transparentColor = solidColor.WithAlpha(Convert.ToByte(0));
            var shader = SkiaSharp.SKShader.CreateLinearGradient(
                new SkiaSharp.SKPoint(0, 0),
                new SkiaSharp.SKPoint(0, e.Info.Height),
                new[] { solidColor, transparentColor },
                new[] { 0f, 3f },
                SkiaSharp.SKShaderTileMode.Clamp);
            var paint = new SkiaSharp.SKPaint { Shader = shader };
            canvas.DrawPaint(paint);

        }
    }
}
/*
 if (DesignMode.IsDesignModeEnabled)
{
  // Previewer only code
}*/
