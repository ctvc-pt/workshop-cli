
Agora temos tudo sobre o jogador feito, passaremos para os inimigos.
Vamos ter 2 tipos de inimigos, um meteoro e um mísseil, que vai seguir o jogador.

Por isso, como já fizemos algumas vezes vamos fazer o seguinte:
1. Escrever na função "load()" as imagens de cada inimigo:

    meteorImage = love.graphics.newImage("meteoro.png")
    eneImage = love.graphics.newImage("inimigo1.png")
    meteorSpeed = 200
    eneSpeed = 250
    chargeSpeed = 500
    enemies = {}
    spawnTimer = 0
    spawnTimerMax = 0.5

    