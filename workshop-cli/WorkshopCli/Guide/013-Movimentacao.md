
Já tens a nave do jogador criada e o próximo passo é permitir que ela se mova.

Vamos começar a aprender por mover a nave para a direita e depois adicionaremos movimentação para os outros lados.

Na nossa função "update()" adiciona o seguinte código:

    -- Movimentação da nave
    if love.keyboard.isDown("right") then
        posic;oX = posicaoX + 1
    end

Executa o jogo (Alt + L) e pressiona a tecla da seta para a direita, a nave deverá movimentar-se nesse sentido.

     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/


Visto que já conseguimos fazer com que o jogador ande para o lado direito, desafio-te para o fazeres andar para o restos dos lados.

Dica:

Não te esqueças de verificar em que direção o jogador é movimentado, se for horizontal usa o posicaoX, se for vertical usa o posicaoY.

esquerda-> left
baixo -> down
cima -> up

Desafio extra:

Se reparares a nave sai fora do ecrã. Vamos ajustar o código do desafio anterior para que a nave não se mova além dos limites do ecrã.

1. Elimina o código anterior de movimentação e adiciona os seguintes:

- Adicionar o limite para a direita:

  if posicaoX < (love.graphics.getWidth() - imagem:getWidth()) then
    if love.keyboard.isDown("right") then
      posicaoX = posicaoX + 1
    end
  end

- Adicionar o limite para a esquerda:

  if posicaoX > 0 then
      if love.keyboard.isDown("left") then
          posicaoX = posicaoX - 1
      end
  end

- Para os sentidos verticais, descobre através deste código:

    "posicaoY <(love.graphics.getHeight() - imagem:getHeight())" para ter o limite para baixo.

    "posicaoY > 0" para ter o limite para cima

