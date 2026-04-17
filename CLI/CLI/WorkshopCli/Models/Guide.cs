using System.Reflection;
using Newtonsoft.Json;

namespace workshopCli;

public class Guide
{
    public struct Step
    {
        public string Id;
        public string Type;
        public string Message;
        public int Delay;
    }

    public List<Step> Steps;
    public Dictionary<int, List<Step>> StepsByVersion;

    public Guide()
    {
        StepsByVersion = new Dictionary<int, List<Step>>();
        Steps = new List<Step>(); // Inicialização padrão
    }

    public Guide(List<Step> initialSteps)
    {
        StepsByVersion = new Dictionary<int, List<Step>>();
        Steps = initialSteps ?? new List<Step>();
        StepsByVersion[1] = new List<Step>(Steps); // Copia os passos do Guide-1
    }

    public void SetSteps(int guideIndex)
    {
        if (guideIndex >= 2 && !StepsByVersion.ContainsKey(guideIndex))
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"workshop_cli.Guide_{guideIndex}.Steps.json";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Resource '{resourceName}' not found in the assembly.");
                }
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    StepsByVersion[guideIndex] = JsonConvert.DeserializeObject<List<Step>>(json) ?? new List<Step>();
                }
            }
        }
        // Substituir completamente os Steps pelo guia selecionado
        Steps = new List<Step>(StepsByVersion[guideIndex]); // Cria uma nova cópia para evitar referências compartilhadas
    }

    // Manter compatibilidade com chamadas antigas
    public void SetSteps(bool hasParticipatedBefore)
    {
        SetSteps(hasParticipatedBefore ? 2 : 1);
    }
}