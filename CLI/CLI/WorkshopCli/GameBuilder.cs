using System.IO.Compression;

namespace workshopCli;

/// <summary>
/// Gera game.exe a partir da pasta mygame do miudo.
/// Port do zip_to_exe.py: concatena love.exe + .love (zip do mygame) e copia as DLLs.
/// </summary>
public static class GameBuilder
{
    private const string LoveDir = @"C:\Program Files\LOVE";

    private static readonly string[] LoveFiles =
    {
        "love.dll",
        "SDL2.dll",
        "lua51.dll",
        "OpenAL32.dll",
        "mpg123.dll",
        "msvcp120.dll",
        "msvcr120.dll",
        "license.txt"
    };

    /// <summary>
    /// Gera "build/game.exe" + DLLs dentro de userFolder. Devolve o caminho da pasta build.
    /// Lanca excepcao se o Love2D nao estiver instalado ou se a pasta mygame nao existir.
    /// </summary>
    public static string Build(string userFolder)
    {
        var mygame = Path.Combine(userFolder, "mygame");
        if (!Directory.Exists(mygame))
            throw new DirectoryNotFoundException($"Pasta 'mygame' nao encontrada em {userFolder}");

        var loveExe = Path.Combine(LoveDir, "love.exe");
        if (!File.Exists(loveExe))
            throw new FileNotFoundException($"Love2D nao encontrado em {LoveDir}. Instala o Love2D antes de construir.");

        var build = Path.Combine(userFolder, "build");
        if (Directory.Exists(build))
            Directory.Delete(build, recursive: true);
        Directory.CreateDirectory(build);

        // 1) zipar mygame -> game.love
        var loveFile = Path.Combine(build, "game.love");
        ZipFile.CreateFromDirectory(mygame, loveFile, CompressionLevel.Optimal, includeBaseDirectory: false);

        // 2) concatenar love.exe + game.love -> game.exe
        var gameExe = Path.Combine(build, "game.exe");
        using (var output = File.Create(gameExe))
        {
            using (var love = File.OpenRead(loveExe)) love.CopyTo(output);
            using (var game = File.OpenRead(loveFile)) game.CopyTo(output);
        }

        File.Delete(loveFile);

        // 3) copiar DLLs
        foreach (var file in LoveFiles)
        {
            var src = Path.Combine(LoveDir, file);
            if (!File.Exists(src)) continue;
            File.Copy(src, Path.Combine(build, file), overwrite: true);
        }

        return build;
    }
}
