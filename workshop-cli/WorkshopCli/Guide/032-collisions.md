
O jogo está quase acabado, mas ainda falta uma coisa importante. Como podes ver, tu não consegues acertar nos inimigos, nem os inimigos conseguem acertar em ti.

1. Para isso, precisamos de 3 funções para verificar colisões. Adiciona este código no fim: 

function verificaJogadorInimigoColisao()
    for index, inimigo in ipairs(inimigos) do
        if intercepta(posicaoX, posicaoY, 47, 50, inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
            posicaoX = 0
            posicaoX = 0
            misseis = {}
            inimigos = {}
            podeDisparar = true
            missilTempo = missilTempoMax
            geraInimigoTempo = 0
        end
    end
end

function intercepta(x1, y1, w1, h1, x2, y2, w2, h2)
    return x1 < x2 + w2 and
    x1 + w1 > x2 and
    y1 < y2 + h2 and
    y1 + h1 > y2
end

2. Agora, adiciona no fim da função "update(dt)" por baixo da linha "atualizarInimigos(dt)" o seguinte código:

    verificaJogadorInimigoColisao()

Estas funções verificam a posição do jogador e dos inimigos.
Se os inimigos tocarem no jogador, recomeça.

Agora corre o jogo (alt+l).

