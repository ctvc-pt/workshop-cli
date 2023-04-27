
5. Agora, vamos à função love.update() e alterar a linha que atualiza a posição do jogador para usar a variável de velocidade em vez do número diretamente.

Vamos substituir a linha 

    player.x=player.x + 3 

por 

    player.x=player.x + player.speed

