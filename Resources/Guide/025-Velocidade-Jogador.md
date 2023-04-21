
4. Agora vamos tornar a velocidade do nosso jogador mais flexível. Em vez de definir a velocidade diretamente no código, vamos criar uma variável para armazenar a velocidade do jogador. Desta forma, se quisermos mudar a velocidade, só precisamos alterar o valor da variável em vez de procurar em várias partes do código.

Para isso, vamos voltar para a função love.load() e adicionar o seguinte código:
    
    player.speed = 3

