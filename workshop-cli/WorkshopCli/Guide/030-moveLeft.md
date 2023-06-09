
Por enquanto, não conseguiste verificar o que fizeste nestes últimos passos.
Como disse, por enquanto.
Vais precisar de mais duas funções que definem os diferentes movimentos dos inimigos.

1. Começamos com o meteoro, inserindo este código:

  function moveLeft(obj, dt)
    obj.xPos = obj.xPos - obj.speed * dt
    return moveLeft
  end

Esta função só faz com que o meteoro mova se para a esquerda, contra o jogador.

