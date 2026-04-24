using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace workshopCli;

/// <summary>
/// No fim do guide, constroi o jogo, zipa (mygame + build) e faz POST multipart
/// para o portatil do monitor. Em caso de falha, deixa o zip no desktop do miudo.
/// Nunca bloqueia o fluxo nem atira excepcoes para cima.
/// </summary>
public class SendGameAction : IAction
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);

    public void Execute()
    {
        try
        {
            var session = LoadSession();
            if (session == null)
            {
                Console.WriteLine("Nao consegui ler a sessao. O teu jogo nao foi enviado.");
                return;
            }

            var userFolder = GetUserFolder(session);
            if (!Directory.Exists(userFolder))
            {
                Console.WriteLine($"Nao encontrei a tua pasta do jogo em {userFolder}.");
                return;
            }

            Console.WriteLine("A preparar o teu jogo para envio...");

            try
            {
                GameBuilder.Build(userFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nao consegui gerar o executavel do jogo ({ex.Message}). Vou enviar so o codigo.");
            }

            var tempZip = CreateGameZip(userFolder);
            var fallbackZip = Path.Combine(userFolder, "game.zip");

            var url = LoadReceiverUrl();
            if (url == null)
            {
                MoveToFallback(tempZip, fallbackZip);
                Console.WriteLine($"Receptor nao configurado. O teu jogo foi guardado em {fallbackZip}. Avisa o monitor.");
                return;
            }

            if (SendZip(url, tempZip, session))
            {
                Console.WriteLine("Jogo enviado! Em breve recebes por email.");
                TryDelete(tempZip);
            }
            else
            {
                MoveToFallback(tempZip, fallbackZip);
                Console.WriteLine($"Nao consegui enviar agora. O teu jogo esta guardado em {fallbackZip}. Chama o monitor.");
            }
        }
        catch (Exception ex)
        {
            // Nunca deixar o fim do workshop crashar por causa desta feature.
            Console.WriteLine($"Algo falhou no envio do jogo: {ex.Message}. O monitor vai ajudar-te.");
        }
    }

    private static Session? LoadSession()
    {
        var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
        if (!File.Exists(txtFilePath)) return null;
        var json = File.ReadAllText(txtFilePath);
        return JsonConvert.DeserializeObject<Session>(json);
    }

    private static string GetUserFolder(Session session)
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var username = string.IsNullOrWhiteSpace(session.Name) ? "default-user" : session.Name.Replace(" ", "-");
        var date = DateTime.Now.ToString("dd-MM-yyyy");
        return Path.Combine(desktop, "repoWorkshop", $"{username}_{date}");
    }

    private static string CreateGameZip(string userFolder)
    {
        // Zipar para %TEMP% em vez de dentro do userFolder — senao o proprio zip
        // fica dentro da pasta que estamos a zipar e o CreateFromDirectory bloqueia.
        var zipPath = Path.Combine(Path.GetTempPath(), $"workshop-game-{Guid.NewGuid():N}.zip");

        // includeBaseDirectory = false para ficar flat ("mygame/...", "build/...").
        ZipFile.CreateFromDirectory(userFolder, zipPath, CompressionLevel.Optimal, includeBaseDirectory: false);
        return zipPath;
    }

    private static void MoveToFallback(string from, string to)
    {
        try
        {
            if (File.Exists(to)) File.Delete(to);
            File.Move(from, to);
        }
        catch
        {
            // Se nem isto der, deixa o zip em temp — o monitor ainda o apanha por lá.
        }
    }

    private static string? LoadReceiverUrl()
    {
        var path = Path.Combine(GuideCli.ResourcesPath, "receiver.json");
        if (!File.Exists(path)) return null;
        try
        {
            var cfg = JsonConvert.DeserializeObject<ReceiverConfig>(File.ReadAllText(path));
            return string.IsNullOrWhiteSpace(cfg?.ReceiverUrl) ? null : cfg.ReceiverUrl;
        }
        catch
        {
            return null;
        }
    }

    private static bool SendZip(string url, string zipPath, Session session)
    {
        try
        {
            using var http = new HttpClient { Timeout = Timeout };
            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(session.Email ?? ""), "email");
            form.Add(new StringContent(session.Name ?? ""), "nome");
            form.Add(new StringContent(session.Mesa ?? ""), "mesa");

            var zipStream = File.OpenRead(zipPath);
            var zipContent = new StreamContent(zipStream);
            zipContent.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
            form.Add(zipContent, "zip", "game.zip");

            var response = http.PostAsync(url, form).GetAwaiter().GetResult();
            zipStream.Dispose();

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static void TryDelete(string path)
    {
        try { File.Delete(path); } catch { /* ignore */ }
    }

    private class ReceiverConfig
    {
        public string? ReceiverUrl { get; set; }
    }
}
