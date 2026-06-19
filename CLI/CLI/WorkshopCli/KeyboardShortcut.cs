using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace workshopCli;

public class KeyboardShortcut
{
    public static void AddKeyboardShortcut()
    {
        // Specify the shortcut keybinding to add
        var shortcutName = "pixelbyte.love2d.run";

        // Specify the new keybinding
        var newShortcut = "alt+l";

        // Find the user keybindings file
        var userKeybindingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Code/User/keybindings.json"
        );

        if ( !File.Exists( userKeybindingsFilePath ) )
        {
            File.WriteAllText( userKeybindingsFilePath, "[]" );
        }

        var keybindings = JArray.Parse( File.ReadAllText( userKeybindingsFilePath ) );

        for ( var i = keybindings.Count - 1; i >= 0; i-- )
        {
            var keybinding = keybindings[ i ];
            var key = keybinding[ "key" ]?.ToString();
            var command = keybinding[ "command" ]?.ToString();

            if ( key == newShortcut && command != shortcutName )
            {
                keybindings.RemoveAt( i );
            }
        }

        // Check if the keybinding already exists
        if ( keybindings.Any( keybinding => keybinding[ "command" ]?.ToString() == shortcutName ) )
        {
            File.WriteAllText( userKeybindingsFilePath, keybindings.ToString() );
            return;
        }

        keybindings.Insert( 0, new JObject
        {
            [ "key" ] = newShortcut,
            [ "command" ] = shortcutName
        } );

        File.WriteAllText( userKeybindingsFilePath, keybindings.ToString() );
    }
}
