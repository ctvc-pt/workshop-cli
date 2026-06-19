[color=white]
Agora ja tens vidas e pontuacao guardadas no jogo.

Mas ainda falta uma coisa importante:
o jogador tem de conseguir ver essa informacao no ecra.

Tambem vamos fazer a pontuacao subir quando destruis um inimigo.
[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/

[/color] [color=white]
1. Primeiro, procura a funcao:
[/color] [color=blue]
    function love.draw()
[/color] [color=white]
2. Dentro dessa funcao, perto do fim, cola estas duas linhas:
[/color] [color=blue]
    love.graphics.print("Vida do jogador: " .. vidas, 100, 100)
    love.graphics.print("Pontuacao: " .. pontuacao, 100, 120)
[/color] [color=white]
Estas linhas escrevem a vida e a pontuacao no ecra.

3. Agora procura a funcao:
[/color] [color=blue]
    function verificaMissilInimigoColisao()
[/color] [color=white]
4. Dentro dessa funcao, procura esta parte:
[/color] [color=blue]
    table.remove(inimigos, index)
    table.remove(misseis, index2)
[/color] [color=white]
5. Logo por baixo dessas duas linhas, cola esta linha:
[/color] [color=blue]
    pontuacao = pontuacao + 10
[/color] [color=white]
O codigo deve ficar parecido com isto:
[/color] [color=blue]
    table.remove(inimigos, index)
    table.remove(misseis, index2)
    pontuacao = pontuacao + 10
    break
[/color] [color=white]
Isto quer dizer:

- Quando um missil acerta num inimigo, o inimigo desaparece.
- O missil tambem desaparece.
- A pontuacao aumenta 10 pontos.

Guarda o ficheiro e testa com Alt + L.
Quando destruires inimigos, a pontuacao deve subir.
[/color]
