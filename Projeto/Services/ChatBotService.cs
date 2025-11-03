using System.Diagnostics;
using System.Threading;

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
            CreateNoWindow = true
        };

        var process = Process.Start(psi);
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
            return $"Erro no chatbot: {error}";

        return output.Trim();
    }
}