import json
from rapidfuzz import fuzz


with open('faq.json', 'r', encoding='utf-8') as f:
    faq_data = json.load(f)

def responder(pergunta):
    pergunta = pergunta.lower()
    melhor_pergunta = None
    melhor_similaridade = 0

    for q, resposta in faq_data.items():
        similaridade = fuzz.ratio(pergunta, q.lower())
        if similaridade > melhor_similaridade:
            melhor_similaridade = similaridade
            melhor_pergunta = q

    
    if melhor_similaridade >= 50:
        return faq_data[melhor_pergunta]
    else:
        return "Desculpe, não entendi sua pergunta. Pode reformular?"

def chat():
    print("🤖 Chatbot FAQ - digite 'sair' para encerrar.\n")
    while True:
        user_input = input("Você: ")
        if user_input.lower() == 'sair':
            print("Chatbot: Até mais! 👋")
            break
        resposta = responder(user_input)
        print("Chatbot:", resposta)

if __name__ == "__main__":
    chat()

