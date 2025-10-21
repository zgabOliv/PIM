import json

with open('faq.json', 'r', encoding='utf-8') as f:
    faq_data = json.load(f)

def responder(pergunta):
    pergunta = pergunta.lower()
    for q, resposta in faq_data.items():
        if q in pergunta or pergunta in q:
            return resposta
    return "Desculpe, não entendi sua pergunta. Pode reformular?"

def chat():
    print("🤖 Chatbot FAQ da Turma - digite 'sair' para encerrar.\n")
    while True:
        user_input = input("Você: ")
        if user_input.lower() == 'sair':
            print("Chatbot: Até mais! 👋")
            break
        resposta = responder(user_input)
        print("Chatbot:", resposta)

if __name__ == "__main__":
    chat()
