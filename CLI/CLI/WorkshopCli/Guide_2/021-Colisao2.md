[color=white]
Neste passo vamos adicionar colisão com o teto e o chão do jogo. Se reparares o 
passaro sobe e caí infinitamente, vamos alterar isso.

1. Dentro da função "update(dt)", adiciona o seguinte:

   [/color] [color=blue]
    if passaro.y < 0 then
       passaro.y = 0
       passaro.velocidade = 0
    end

	if passaro.y + passaro.altura > love.graphics.getHeight() then
		passaro.y = love.graphics.getHeight() - passaro.altura
		gameOver = true
		somColisao:play()
		somGameOver:play()
	end
   [/color] [color=white]

Agora quando tocares no chão o jogo acaba, e também já não podes subir infinitamente.
[/color] 