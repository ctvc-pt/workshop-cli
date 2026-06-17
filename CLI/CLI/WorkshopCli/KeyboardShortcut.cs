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
        var shortcutName = "pixelbyte.love2d.run";
        var newShortcut = "alt+l";

        var userKeybindingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Code/User/keybindings.json"
        );

        Directory.CreateDirectory(Path.GetDirectoryName(userKeybindingsFilePath)!);

        var userKeybindingsJson = File.Exists(userKeybindingsFilePath)
            ? File.ReadAllText(userKeybindingsFilePath)
            : "[]";

        if (userKeybindingsJson.Contains($"\"command\": \"{shortcutName}\""))
        {
            userKeybindingsJson = userKeybindingsJson.Replace(
                "\"key\": \"alt+j\", \"command\": \"pixelbyte.love2d.run\"",
                $"\"key\": \"{newShortcut}\", \"command\": \"{shortcutName}\""
            );
            File.WriteAllText(userKeybindingsFilePath, userKeybindingsJson);
            return;
        }

        userKeybindingsJson = userKeybindingsJson.Replace(
            "[",
            $"[{{\"key\": \"{newShortcut}\", \"command\": \"{shortcutName}\"}},"
        );

        File.WriteAllText(userKeybindingsFilePath, userKeybindingsJson);
    }
}
