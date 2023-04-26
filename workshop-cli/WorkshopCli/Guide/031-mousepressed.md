
5. Agora, precisamos de criar uma lógica para permitir que o jogador volte ao tamanho original depois de clicar no botão novamente. Vamos verificar se o valor atual de player.scale é igual a 1. Se for, definiremos player.scale para 3. Se não for, definiremos player.scale para 1:

	function love.mousepressed(x, y, button)
		if button == 1 then
			if player.scale == 1 then
				player.scale = 3
			else
				player.scale = 1
			end
		end
	end

