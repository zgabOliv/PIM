using System.Diagnostics;
using System.Text;

public class ChatbotService
{
    public static string ObterResposta(string pergunta)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "py",
            Arguments = $"Scripts/chatbot.py \"{pergunta}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,

            // AQUI É O PULO DO GATO:
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        var process = Process.Start(psi);

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrWhiteSpace(error))
            return $"Erro no chatbot: {error}";

        return output.Trim();
    }
}
