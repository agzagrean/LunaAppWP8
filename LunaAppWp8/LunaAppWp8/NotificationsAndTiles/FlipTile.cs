using LunaAppWp8.Resources;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.NotificationsAndTiles
{
    class FlipTile
    {
        private static Version TargetedVersion = new Version(7, 10, 8858);
        public static bool IsTargetedVersion { get { return Environment.OSVersion.Version >= TargetedVersion; } }

        private static string smallBackgroundImage = "/Assets/Tiles/FlipCycleTileSmall.png";
        private static string backgroundImage = "/Assets/Tiles/FlipCycleTileMedium.png";
        private static string backBackgroundImage = "/Assets/Tiles/TileBackground.png";
        private static string wideBackgroundImage = "/Assets/Tiles/FlipCycleTileLarge.png";
        private static string wideBackBackgroundImage = "/Assets/Tiles/TileBackground.png";


        public static void CreateOrUpdateFlipTile(int count, string countText)
        {
            Uri tileId = new Uri("/InitialSetupPage.xaml", UriKind.Relative);

            CreateOrUpdateFlipTile(
                AppResources.ApplicationTitle,
                AppResources.ApplicationTitle,
                string.Format("{0} {1}", count, countText),
                   string.Format("{0} {1}", count, countText),
                count,
                tileId,
                new Uri(smallBackgroundImage, UriKind.Relative),
                new Uri(backgroundImage, UriKind.Relative),
                new Uri(backBackgroundImage, UriKind.Relative),
                new Uri(wideBackgroundImage, UriKind.Relative),
                new Uri(wideBackBackgroundImage, UriKind.Relative)
                );
        }

        public static void CreateOrUpdateFlipTile(
            string title,
            string backTitle,
            string backContent,
            string wideBackContent,
            int count,
            Uri tileId,
            Uri smallBackgroundImage,
            Uri backgroundImage,
            Uri backBackgroundImage,
            Uri wideBackgroundImage,
            Uri wideBackBackgroundImage)
        {
            if (IsTargetedVersion)
            {
                
                // Get the new FlipTileData type.
                Type flipTileDataType = typeof(FlipTileData);

              //   Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                Type shellTileType = typeof(Microsoft.Phone.Shell.ShellTile);
              
                bool found = false;
                // Loop through any existing Tiles that are pinned to Start.
                foreach (var tileToUpdate in ShellTile.ActiveTiles)
                {
                    // Look for a match based on the Tile's NavigationUri (tileId).
                    if (tileToUpdate.NavigationUri.ToString() == tileId.ToString())
                    {
                        found = true;
                        
                       //  Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                        var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                       //  Set the properties. 
                        SetProperty(UpdateTileData, "Title", title);
                        SetProperty(UpdateTileData, "Count", count);
                        SetProperty(UpdateTileData, "BackTitle", backTitle);
                        SetProperty(UpdateTileData, "BackContent", backContent);
                        SetProperty(UpdateTileData, "SmallBackgroundImage", smallBackgroundImage);
                        SetProperty(UpdateTileData, "BackgroundImage", backgroundImage);
                        SetProperty(UpdateTileData, "BackBackgroundImage", backBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackgroundImage", wideBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackBackgroundImage", wideBackBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackContent", wideBackContent);

                        // Invoke the new version of ShellTile.Update.
                        shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });

                        break;
                    }
                    else
                        found = false ;
                }

                //if not existing, create it 
                if (!found)
                {
                    FlipTileData initialData = new FlipTileData()
                    {
                        Title = title,
                        BackTitle = backTitle,
                        BackContent = backContent,
                        WideBackContent = wideBackContent,
                        Count = count,
                        SmallBackgroundImage = smallBackgroundImage,
                        BackgroundImage = backgroundImage,
                        BackBackgroundImage = backBackgroundImage,
                        WideBackgroundImage = wideBackgroundImage,
                        WideBackBackgroundImage = wideBackBackgroundImage
                    };

                    ShellTile.Create(tileId, initialData, true);
                }
            }

        }



        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }

    }
}
