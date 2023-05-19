
3. O proximo passo é meter a nossa nave a disparar misseis e tal como a nave o primeiro passo é indicar a imagem e os seus atributos na função load.

Primeiro escolhe na pasta uma imagem para ser o missel da tua nave.
Implementa o seguinte codigo na função load()

    missilImage = love.graphics.newImage("nome_imagem.png")
    missilTimerMax = 0.2
    missilStartSpeed = 100
    missilMaxSpeed = 300
    missiles = {}

    canFire = true
    missilTimer = missilTimerMax

