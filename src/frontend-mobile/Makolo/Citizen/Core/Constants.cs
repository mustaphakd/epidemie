using Citizen.Framework.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Core
{
    public static class Constants
    {
        public const string Products = "products";
        public const string Analytics = "analytics";
        public const string AddressBooks = "address_book";
        public const string Shop = "shop";

        public const string Separator = "**";

    }
    public static class Routes
    {
        public const string Home = "home";
        public const string Products = "products";
        public const string AddressBooks = "addressbooks";
        public const string Analytics = "analytics";
        public const string Shop = "shop";
        public const string Articles = "articles";
        public const string Categories = "categories";
    }

    public static class StoragePathes
    {
        public const string Products = "Images"+ Constants.Separator +"Products";
    }
}
