import json, sys
from rapidfuzz import fuzz

with open('Scripts/faq.json', 'r', encoding='utf-8') as f:
    faq_data = json.load(f)

def responder(pergunta):
    pergunta = pergunta.lower()
    melhor_pergunta, melhor_similaridade = None, 0

    for q, resposta in faq_data.items():
        similaridade = fuzz.ratio(pergunta, q.lower())
        if similaridade > melhor_similaridade:
            melhor_similaridade = similaridade
            melhor_pergunta = q

    if melhor_similaridade >= 50:
        return faq_data[melhor_pergunta]
    return "Desculpe, não entendi sua pergunta. Pode reformular?"

if __name__ == "__main__":
    if len(sys.argv) > 1:
        pergunta = " ".join(sys.argv[1:])
        print(responder(pergunta))