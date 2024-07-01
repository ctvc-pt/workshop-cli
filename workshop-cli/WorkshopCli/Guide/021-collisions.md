[color=white]
O jogo está a ganhar forma, mas ainda falta uma coisa importante: tu não 
consegues acertar nos inimigos, nem os inimigos conseguem acertar em ti.

Vamos adicionar 3 funções para verificar colisões entre o jogador e os inimigos.

1. Adiciona este código no fim:
   [/color] [color=blue]
    -- Função que verifica colisões entre o jogador e inimigos
    function verificaJogadorInimigoColisao()
        for index, inimigo in ipairs(inimigos) do
            if intercepta(posicaoX, posicaoY, imagem:getWidth(), imagem:getHeight(), inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
                table.remove(inimigos, index)
            end
        end
    end

    -- Função que verifica se os objetos se interceptam
    function intercepta(x1, y1, w1, h1, x2, y2, w2, h2)
        return x1 < x2 + w2 and
        x1 + w1 > x2 and
        y1 < y2 + h2 and
        y1 + h1 > y2
    end
   [/color] [color=white]
2. Adiciona no fim da função "update(dt)" por baixo da linha "atualizarInimigos(dt)":
   [/color] [color=blue]
    verificaJogadorInimigoColisao()
   [/color] [color=white]
Com estas funções que adicionaste agora, se os inimigos tocarem no jogador, o jogo recomeça. Podes testar, executa o jogo com Alt + L.
   [/color]