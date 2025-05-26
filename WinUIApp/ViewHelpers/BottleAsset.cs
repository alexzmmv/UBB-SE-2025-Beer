// <copyright file="BottleAsset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ViewHelpers
{
    using System.ComponentModel;
    using WinUIApp.ViewHelpers;

    /// <summary>
    /// Represents a bottle asset with an image source. Used for binding in XAML.
    /// </summary>
    public class BottleAsset : INotifyPropertyChanged
    //    /// <summary>
    //    /// The image source of the bottle asset.
    //    /// </summary>
    //    private string bottleImageSource = AssetConstants.EmptyBottlePath;

    //    /// <summary>
    //    /// Implements the interface and is used for binding.
    //    /// </summary>
    //    public event PropertyChangedEventHandler? PropertyChanged;

    //    /// <summary>
    //    /// Gets or sets the image source for the bottle asset.
    //    /// </summary>
    //    public string ImageSource
    //    {
    //        get => this.bottleImageSource;
    //        set
    //        {
    //            this.bottleImageSource = value;
    //            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ImageSource)));
    //        }
    //    }
        {
            private string _imageSource;

            public string ImageSource
            {
                get => _imageSource;
                set
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }
    }