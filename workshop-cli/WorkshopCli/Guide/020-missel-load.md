
1. O proximo passo é meter a nossa nave a disparar mísseis e tal como 
a nave o primeiro passo é indicar a imagem e os seus atributos na 
função "load()".

Primeiro escolhe na pasta uma imagem para ser o míssel da tua nave.
Implementa o seguinte código na função "load()"

    missilImageM = love.graphics.newImage("nome_imagem.png")
    missilTimerMax = 0.2
    missilStartSpeed = 100
    missilMaxSpeed = 300
    missiles = {}

    canFire = true
    missilTimer = missilTimerMax

