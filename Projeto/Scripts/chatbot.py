import json, sys
from rapidfuzz import fuzz
import unicodedata

def saida(texto):
    sys.stdout.buffer.write((texto + "\n").encode("utf-8"))

def normalizar(txt):
    return ''.join(
        c for c in unicodedata.normalize('NFKD', txt)
        if not unicodedata.combining(c)
    ).lower()

with open('Scripts/faq.json', 'r', encoding='utf-8') as f:
    faq1 = json.load(f)

with open('Scripts/faq.json2.json', 'r', encoding='utf-8') as f:
    faq2 = json.load(f)

faq = {**faq1, **faq2}
mapa_normalizado = {normalizar(k): k for k in faq.keys()}

def responder(pergunta):
    pergunta_norm = normalizar(pergunta)

    melhor_norm = None
    melhor_score = 0

    for chave_norm in mapa_normalizado.keys():
        score = fuzz.token_set_ratio(pergunta_norm, chave_norm)
        if score > melhor_score:
            melhor_score = score
            melhor_norm = chave_norm

    if melhor_score >= 50:
        chave_original = mapa_normalizado[melhor_norm]
        return faq[chave_original]

    return "Desculpe, não entendi sua pergunta. Pode reformular?"

if __name__ == "__main__":
    if len(sys.argv) > 1:
        txt = " ".join(sys.argv[1:])
        saida(responder(txt))
