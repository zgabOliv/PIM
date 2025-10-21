using System;
using System.Collections.Generic;
using System.Threading;

class AlunoAtividades
{
    // Simulação de tarefas que o professor teria criado
    static List<string> tarefasDisponiveis = new List<string>()
    {
        "📘 Tarefa 1 - Fazer resumo do capítulo 2",
        "📘 Tarefa 2 - Resolver 5 exercícios de matemática",
        "📘 Tarefa 3 - Escrever uma redação sobre meio ambiente"
    };

    // Lista para guardar as respostas do aluno
    static List<string> respostasAluno = new List<string>();

    static void Main()
    {
        int opcao;

        INICIO:
        Console.Clear();
        Console.WriteLine("PAINEL DO ALUNO");
        Console.WriteLine("1 - Ver tarefas disponíveis");
        Console.WriteLine("2 - Enviar resposta de tarefa");
        Console.WriteLine("3 - Ver respostas enviadas");
        Console.WriteLine("4 - Sair");
        Console.Write("Escolha uma opção: ");

        bool conversao = int.TryParse(Console.ReadLine(), out opcao);
        if (!conversao)
        {
            Console.WriteLine("Digite apenas números!");
            Thread.Sleep(1000);
            goto INICIO;
        }

        switch (opcao)
        {
            case 1:
                Console.Clear();
                Console.WriteLine("TAREFAS DISPONÍVEIS ");
                foreach (var tarefa in tarefasDisponiveis)
                {
                    Console.WriteLine(tarefa);
                }
                Console.WriteLine("\nPressione qualquer tecla para voltar...");
                Console.ReadKey();
                goto INICIO;

            case 2:
                Console.Clear();
                Console.WriteLine("ENVIAR RESPOSTA");
                Console.Write("Digite o número da tarefa que deseja responder (1, 2 ou 3): ");
                int numTarefa;
                int.TryParse(Console.ReadLine(), out numTarefa);

                if (numTarefa < 1 || numTarefa > tarefasDisponiveis.Count)
                {
                    Console.WriteLine("Essa tarefa não existe!");
                    Thread.Sleep(1500);
                    goto INICIO;
                }

                Console.WriteLine($"Você escolheu: {tarefasDisponiveis[numTarefa - 1]}");
                Console.Write("Digite sua resposta: ");
                string resposta = Console.ReadLine();

                respostasAluno.Add($" Resposta para {tarefasDisponiveis[numTarefa - 1]}: {resposta}");
                Console.WriteLine("Resposta enviada com sucesso! ");
                Thread.Sleep(1500);
                goto INICIO;

            case 3:
                Console.Clear();
                Console.WriteLine("SUAS RESPOSTAS");
                if (respostasAluno.Count == 0)
                {
                    Console.WriteLine("Você ainda não enviou nenhuma resposta. 📭");
                }
                else
                {
                    foreach (var r in respostasAluno)
                    {
                        Console.WriteLine(r);
                    }
                }
                Console.WriteLine("\nPressione qualquer tecla para voltar...");
                Console.ReadKey();
                goto INICIO;

            case 4:
                Console.WriteLine("Saindo... até logo aluno! ");
                Thread.Sleep(1000);
                break;

            default:
                Console.WriteLine("Opção inválida!");
                Thread.Sleep(1000);
                goto INICIO;
                break;
        }
    }
}
