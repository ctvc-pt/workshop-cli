
Dentro da função "load()", verificamos se alguma tecla está pressionada. Por exemplo, se a tecla "direita" estiver pressionada, o jogador irá se mover para a direita. Isso é feito ao alterar a posição horizontal (xPos) do jogador. A fórmula player.xPos = player.xPos + dt * player.speed aumenta a posição horizontal do jogador com base na velocidade (speed) e no tempo decorrido (dt).

DESAFIO: Visto que já conseguimos fazer com que o jogador ande para o lado direito, desafio-te para o fazeres andar para o restos dos lados.

Dicas:
	esquerda-> left
	cima -> up
	baixo -> down

Não te esqueças de verifcar em que direção o jogador é movimentado, se for horizontal usa o xPos, se for vertical usa o yPos.

