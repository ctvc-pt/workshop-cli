[color=white]
O jogo está a ganhar forma, mas ainda falta uma coisa importante: 
tu não consegues acertar nos inimigos, nem os inimigos conseguem 
acertar em ti.

Para saber se dois objetos se tocam, imaginamos um retângulo à 
volta de cada um (chama-se "hitbox" ou "caixa de colisão"). Há 
colisão quando os dois retângulos se sobrepõem ao mesmo tempo no 
eixo X e no eixo Y — é exatamente isto que a função "intercepta" 
verifica, com uma condição por cada lado do retângulo.

Vamos adicionar 3 funções para verificar colisões entre o jogador 
e os inimigos.

1. Adiciona este código no fim:
   [/color] [color=blue]
    function verificaJogadorInimigoColisao()
        for index, inimigo in ipairs(inimigos) do
            if intercepta(posicaoX, posicaoY, imagem:getWidth(), imagem:getHeight(), inimigo.posicaoX, inimigo.posicaoY, inimigo.width, inimigo.height) then
                table.remove(inimigos, index)
            end
        end
    end

    function intercepta(x1, y1, w1, h1, x2, y2, w2, h2)
        return x1 < x2 + w2 and
        x1 + w1 > x2 and
        y1 < y2 + h2 and
        y1 + h1 > y2
    end
   [/color] [color=white]
2. Adiciona no fim da função "update(dt)" por baixo da linha 
"atualizarInimigos(dt)":
   [/color] [color=blue]
    verificaJogadorInimigoColisao()
   [/color] [color=white]
Com estas funções que adicionaste agora, se os inimigos tocarem 
no jogador, os inimigos desaparecem, como se perdesses uma vida. 
Podes testar, executa o jogo com Alt + L.
   [/color]