using System;
using System.Threading;

class Program
{
    static void Main()
    {
        //  DECLARAÇÃO DE VARIÁVEIS 
        // Variáveis que serão usadas em todo o programa
        int opcao; // guarda a opção do menu (1 ou 2)
        string nome, emailAluno, senhaAluno, emailProfessor, senhaProfessor, numeroIdentificacao, curso;
        int periodo; // 1 = matutino, 2 = noturno

    // INÍCIO DO MENU PRINCIPAL 
    INICIO:
        Console.WriteLine(" MENU DE CADASTRO");
        Console.WriteLine("Digite 1 para cadastro de Aluno");
        Console.WriteLine("Digite 2 para cadastro de Professor");
        Console.Write("Escolha uma opção: ");

        // Lê o valor digitado e tenta converter para inteiro
        bool conversaoValida = int.TryParse(Console.ReadLine(), out opcao);

        // Se o usuário digitar algo que não seja número, volta pro menu
        if (!conversaoValida)
        {
            Console.Clear();
            Console.WriteLine("Opção inválida! Digite apenas números.");
            Thread.Sleep(1000); // pausa de 1 segundo
            Console.Clear();
            goto INICIO; // volta ao início do menu
        }

        Thread.Sleep(800); // pequena pausa pra deixar mais suave
        Console.Clear();

        // MENU DE ESCOLHA (SWITCH CASE) 
        switch (opcao)
        {
            //  CADASTRO DE ALUNO 
            case 1:
                Console.WriteLine(" CADASTRO DE ALUNO ");

                // Solicita e formata o nome do aluno
                Console.Write("Digite seu nome completo: ");
                nome = Console.ReadLine().Trim().ToUpper(); // remove espaços extras e deixa em maiúsculas

                // Solicita o curso
                Console.Write("Digite seu curso: ");
                curso = Console.ReadLine().Trim();

                // Pergunta o período do curso
                Console.Write("Qual o período do seu curso (1 = matutino / 2 = noturno): ");
                int.TryParse(Console.ReadLine(), out periodo);

                // Verifica se o período é válido
                if (periodo != 1 && periodo != 2)
                {
                    Console.WriteLine("Período não cadastrado!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    goto INICIO; // volta para o menu principal
                }

                // Solicita o e-mail do aluno
                Console.Write("Digite seu e-mail: ");
                emailAluno = Console.ReadLine().Trim().ToLower(); // formata em minúsculas

                // Solicita a senha do aluno
                Console.Write("Digite sua senha: ");
                senhaAluno = Console.ReadLine();

                // Verifica se o e-mail tem o domínio correto
                if (emailAluno.EndsWith("@aluno.com"))
                {
                    Console.WriteLine("\nCadastro de aluno realizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("\nE-mail inválido! Utilize o formato: nome@aluno.com");
                    Thread.Sleep(1500);
                    Console.Clear();
                    goto INICIO; // volta pro menu se o e-mail estiver errado
                }
                break;

            //  CADASTRO DE PROFESSOR 
            case 2:
                Console.WriteLine(" CADASTRO DE PROFESSOR");

                // Solicita e formata o nome do professor
                Console.Write("Digite seu nome completo: ");
                nome = Console.ReadLine().Trim().ToUpper();

                // Solicita CPF ou RG
                Console.Write("Digite seu RG ou CPF: ");
                numeroIdentificacao = Console.ReadLine().Trim();

                // Solicita o e-mail
                Console.Write("Digite seu e-mail: ");
                emailProfessor = Console.ReadLine().Trim().ToLower();

                // Solicita a senha
                Console.Write("Digite sua senha: ");
                senhaProfessor = Console.ReadLine();

                // Verifica se o e-mail tem o domínio correto
                if (emailProfessor.EndsWith("@professor.com"))
                {
                    Console.WriteLine("\nCadastro de professor realizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("\nE-mail inválido! Utilize o formato: nome@professor.com");
                    Thread.Sleep(1500);
                    Console.Clear();
                    goto INICIO; // volta pro menu se o e-mail estiver errado
                }
                break;

            // CASO DIGITE UMA OPÇÃO INVÁLIDA 
            default:
                Console.Clear();
                Console.WriteLine("Opção inválida! Escolha 1 ou 2.");
                Thread.Sleep(1000);
                Console.Clear();
                goto INICIO; // volta para o menu principal
        }
        Console.ReadKey(); // aguarda o usuário apertar algo antes de fechar
    }
}