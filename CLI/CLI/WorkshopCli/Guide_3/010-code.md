function love.load()
    -- Janela
    love.window.setTitle("Skater Runner")
    love.window.setMode(900, 400)

    -- Medidas do jogo
    LARGURA = 900
    ALTURA = 400
    CHAO_Y = 330

    -- Texto
    fonte = love.graphics.newFont(20)
    love.graphics.setFont(fonte)

    -- Imagens
    imagemFundo = love.graphics.newImage("background.png")
    imagemSkater = love.graphics.newImage("skater.png")
    imagemBanco = love.graphics.newImage("bench.png")
    imagemGaivota = love.graphics.newImage("seagull.png")

    -- Skater: 3 poses no spritesheet
    local larguraSkater = 1536
    local alturaSkater = 1024
    local larguraPose = larguraSkater / 3

    local recorteX = 155
    local recorteY = 357
    local recorteL = 233
    local recorteA = 280

    poseAndar = love.graphics.newQuad(0 * larguraPose + recorteX, recorteY, recorteL, recorteA, larguraSkater, alturaSkater)
    poseSaltar = love.graphics.newQuad(1 * larguraPose + recorteX, recorteY, recorteL, recorteA, larguraSkater, alturaSkater)
    poseAgachar = love.graphics.newQuad(2 * larguraPose + recorteX - 80, recorteY, recorteL, recorteA, larguraSkater, alturaSkater)

    escalaSkater = 70 / recorteA

    -- Jogador
    jogador = {
        x = 90,
        largura = math.floor(recorteL * escalaSkater * 0.6),
        altura = math.floor(recorteA * escalaSkater * 0.85),
        velocidadeY = 0,
        gravidade = 950,
        forcaSalto = -430,
        noChao = true,
        agachado = false,
    }
    jogador.y = CHAO_Y - jogador.altura
    jogador.alturaAgachar = math.floor(jogador.altura * 0.75)

    -- Banco (obstaculo de salto)
    local bancoX = 349
    local bancoY = 288
    local bancoL = 778
    local bancoA = 377

    recorteBanco = love.graphics.newQuad(bancoX, bancoY, bancoL, bancoA, 1536, 1024)
    escalaBanco = 40 / bancoA
    bancoLargura = math.floor(bancoL * escalaBanco)
    bancoAltura = math.floor(bancoA * escalaBanco)

    -- Gaivota (obstaculo de agachar) com 2 frames
    larguraFrameGaivota = imagemGaivota:getWidth() / 2
    alturaFrameGaivota = imagemGaivota:getHeight()

    gaivotaFrame1 = love.graphics.newQuad(0, 0, larguraFrameGaivota, alturaFrameGaivota + 10, imagemGaivota:getWidth(), imagemGaivota:getHeight())
    gaivotaFrame2 = love.graphics.newQuad(larguraFrameGaivota, 0, larguraFrameGaivota, alturaFrameGaivota + 10, imagemGaivota:getWidth(), imagemGaivota:getHeight())

    -- Estado do jogo
    fimDeJogo = false
    pontuacao = 0
    velocidade = 260

    -- Lista de obstaculos
    obstaculos = {}
    timerObstaculo = 0
    intervalo = 1.4
end

function love.update(dt)
end

function love.draw()
    -- Fundo
    love.graphics.setColor(1, 1, 1)
    love.graphics.draw(imagemFundo, 0, 0, 0,
        LARGURA / imagemFundo:getWidth(),
        ALTURA / imagemFundo:getHeight()
    )
end
