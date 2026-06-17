[color=white]
Agora vamos fazer o jogador perder vidas.

No passo anterior criaste esta variavel:
   [/color] [color=blue]
    vidas = 3
   [/color] [color=white]
Agora vamos usar essa variavel quando um inimigo toca na nave.

1. Procura a funcao:
   [/color] [color=blue]
    function verificaJogadorInimigoColisao()
   [/color] [color=white]
2. Dentro dessa funcao, procura esta linha:
   [/color] [color=blue]
    table.remove(inimigos, index)
   [/color] [color=white]
3. Mesmo antes dessa linha, cola isto:
   [/color] [color=blue]
    vidas = vidas - 1
   [/color] [color=white]
Isto quer dizer: quando um inimigo bate na nave, o jogador perde 1 vida.

4. Agora, mesmo depois da linha:
   [/color] [color=blue]
    table.remove(inimigos, index)
   [/color] [color=white]
cola este codigo:
   [/color] [color=blue]
    if vidas <= 0 then
        estadoJogo = "perder"
    end
   [/color] [color=white]
Isto quer dizer: se as vidas chegarem a zero, o jogo muda para "perder".

5. Agora vai ate ao fim do ficheiro "main.lua".
Depois do ultimo "end", cola esta funcao nova:
   [/color] [color=blue]
    function love.keypressed(key)
        if key == 'r' and (estadoJogo == "perder" or estadoJogo == "vitoria") then
            estadoJogo = "a jogar"
            pontuacao = 0
            vidas = 3
            posicaoX = 100
            posicaoY = 500
            misseis = {}
            inimigos = {}
        end
    end
   [/color] [color=white]
Esta funcao ve quando carregas numa tecla.
Se carregares na tecla R depois de perder ou ganhar, o jogo reinicia.

Atencao:

- A linha "vidas = vidas - 1" fica antes de apagar o inimigo.
- O if "vidas <= 0" fica depois de apagar o inimigo.
- A funcao "love.keypressed(key)" fica fora das outras funcoes, no fim do ficheiro.

Quando terminares, guarda e testa com Alt + L.
Agora, se 3 inimigos acertarem na nave, aparece Game Over.
[/color]
