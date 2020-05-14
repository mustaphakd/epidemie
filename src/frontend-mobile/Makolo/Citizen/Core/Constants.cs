using Citizen.Framework.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Core
{
    public static class Constants
    {
        public const string Home = "home";
        public const string Login = "login";
        public const string Register = "register";
        public const string PhysicalContacts = "physical_contacts";
        public const string Map = "map";
        public const string Cowrie = "cowrie";
        public const string HomeMenu = "menu";

        public const string Separator = "**";

    }
    public static class Routes
    {
        public const string Home = "home";
        public const string Login = "login";
        public const string Register = "register";
        public const string HomeMenu = "menu";
        public const string PhysicalContacts = "physicalcontacts";
        public const string Map = "map";
        public const string Cowrie = "cowrie";
    }

    public static class RestEndpoints
    {
        public const string Login = "security/login";
        public const string Registation = "security/login";
        public const string Logout = "security/logout";
    }

    public static class CacheKeys
    {
        public const string User_Auth = "auth_token";
        public const string User_Profile = "user_prof";
        public const string User_phyContacts = "user_physical_contacts";
        public const string User_LastLogin = "user_last_login";
    }

    public static class StoragePathes
    {
        public const string Products = "Images"+ Constants.Separator +"Products";
    }
}
