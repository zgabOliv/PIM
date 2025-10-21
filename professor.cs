using System;
using System.Collections.Generic;
using System.Threading;

class ProfessorTarefas
{
    // Lista para guardar todas as tarefas criadas pelo professor
    static List<string> tarefas = new List<string>();

    static void Main()
    {
        int opcao;

        INICIO:
        Console.Clear();
        Console.WriteLine(" PAINEL DO PROFESSOR ");
        Console.WriteLine("1 - Criar nova tarefa");
        Console.WriteLine("2 - Ver todas as tarefas criadas");
        Console.WriteLine("3 - Sair");
        Console.Write("Escolha uma opção: ");

        bool conversao = int.TryParse(Console.ReadLine(), out opcao);
        if (!conversao)
        {
            Console.WriteLine("Digite apenas números");
            Thread.Sleep(1000);
            goto INICIO;
        }

        switch (opcao)
        {
            case 1:
                // Aqui o professor vai digitar o título e descrição da tarefa
                Console.Write("Digite o título da tarefa: ");
                string titulo = Console.ReadLine();

                Console.Write("Digite a descrição ou o que o aluno deve fazer: ");
                string descricao = Console.ReadLine();

                // Guarda a tarefa dentro da lista
                tarefas.Add($"📘 {titulo} - {descricao}");

                Console.WriteLine("\nTarefa criada com sucesso! 🎉");
                Thread.Sleep(1500);
                goto INICIO;

            case 2:
                Console.Clear();
                Console.WriteLine("=== LISTA DE TAREFAS CRIADAS ===");

                // Mostra todas as tarefas, se tiver alguma
                if (tarefas.Count == 0)
                {
                    Console.WriteLine("Nenhuma tarefa criada ainda.");
                }
                else
                {
                    int numero = 1;
                    foreach (string tarefa in tarefas)
                    {
                        Console.WriteLine($"{numero}. {tarefa}");
                        numero++;
                    }
                }

                Console.WriteLine("\nPressione qualquer tecla para voltar...");
                Console.ReadKey();
                goto INICIO;

            case 3:
                Console.WriteLine("Saindo... ");
                Thread.Sleep(1000);
                break;

            default:
                Console.WriteLine("Opção inválida!");
                Thread.Sleep(1000);
                goto INICIO;
        }
    }
}