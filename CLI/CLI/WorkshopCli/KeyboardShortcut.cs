using System;
namespace workshopCli;

public class KeyboardShortcut
{
    public static void AddKeyboardShortcut()
    {
        var userKeybindingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Code",
            "User",
            "keybindings.json");

        Directory.CreateDirectory(Path.GetDirectoryName(userKeybindingsFilePath)!);

        File.WriteAllText(userKeybindingsFilePath,
            "[\n" +
            "  {\n" +
            "    \"key\": \"alt+l\",\n" +
            "    \"command\": \"workbench.action.tasks.build\"\n" +
            "  }\n" +
            "]\n");
    }
}
