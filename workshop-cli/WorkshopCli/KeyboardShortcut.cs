using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshopCli;

public class KeyboardShortcut
{
    public static void AddKeyboardShortcut()
    {
        // Specify the shortcut keybinding to add
        var shortcutName = "pixelbyte.love2d.run";

        // Specify the new keybinding
        var newShortcut = "alt+j";

        // Find the user keybindings file
        var userKeybindingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Code/User/keybindings.json"
        );

        // Read the user keybindings file into a string
        var userKeybindingsJson = File.ReadAllText(userKeybindingsFilePath);

        // Check if the keybinding already exists
        if (userKeybindingsJson.Contains($"\"command\": \"{shortcutName}\""))
        {
            //Console.WriteLine($"The keybinding \"{shortcutName}\" already exists.");
            return;
        }

        // Add the new keybinding
        userKeybindingsJson = userKeybindingsJson.Replace(
            "[",
            $"[{{\"key\": \"{newShortcut}\", \"command\": \"{shortcutName}\"}},"
        );

        // Write the modified keybindings back to the file
        File.WriteAllText(userKeybindingsFilePath, userKeybindingsJson);
    }
}
