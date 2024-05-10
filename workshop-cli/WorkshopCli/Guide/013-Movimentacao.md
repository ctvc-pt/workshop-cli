
Já tens o jogador criado e o próximo passo é fazer a movimentação da nossa nave.
Vamos começar com a movimentação para a direita.
Na nossa função "update()" adiciona o seguinte código:

    if love.keyboard.isDown("right") then
        posicaoX = posicaoX + 1
    end

Agora corre o código usando 'Alt+L' e pressiona na tecla da seta para a direita


     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/



Visto que já conseguimos fazer com que o jogador ande para o lado direito, desafio-te para o fazeres andar para o restos dos lados.

Dicas:
esquerda-> left
baixo -> down
cima -> up

Não te esqueças de verificar em que direção o jogador é movimentado, se for horizontal usa o posicaoX, se for vertical usa o posicaoY.

Testa o jogo(Alt+L).

Desafio extra:
Se reparares a nave sai fora do ecrã, para impedir que a nave saia retira o codigo de cima e usa o seguinte codigo:

Para ter o limite para a direita:

    if posicaoX < (love.graphics.getWidth() - imagem:getWidth()) then 
        if love.keyboard.isDown("right") then
            posicaoX = posicaoX + 1
        end
    end

Para ter o limite para a direita:

    if posicaoX > 0 then 
        if love.keyboard.isDown("left") then
            posicaoX = posicaoX - 1
        end
    end

Para os sentido vertical, descobre e usa este codigo:


    "posicaoY <(love.graphics.getHeight() - imagem:getHeight())" para ter o limite para baixo.

    "posicaoY > 0" para ter o limite para cima

